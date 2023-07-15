using FluentResults;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;
using MarleneCollectionXmlTool.Domain.Services;
using MarleneCollectionXmlTool.Domain.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales;

public class SyncProductStocksWithWholesalerRequestHandler : IRequestHandler<SyncProductStocksWithWholesalerRequest, Result<SyncProductStocksWithWholesalerResponse>>
{
    private readonly IGetXmlDocumentFromWholesalerService _wholesalerService;
    private readonly IProductAttributeService _productAttributeService;
    private readonly IProductMetaService _metaService;
    private readonly ICacheProvider _cacheProvider;
    private readonly string _baseClientUrl;
    private readonly WoocommerceDbContext _dbContext;
    private readonly string[] _metaKeys = 
    { 
        "_variation_description", "total_sales", "_tax_status", "_tax_class", "_manage_stock", "_backorders", "_sold_individually", "_virtual", "_downloadable",
        "_download_limit", "_download_expiry", "_stock", "_stock_status", "_wc_average_rating", "_wc_review_count", "attribute_kolor", "attribute_rozmiar",
        "uniqid", "_product_version", "import_uid", "import_started_at", "_sku", "_regular_price", "_price"
    };
    private readonly string[] _notUpdatableSkus =
    {
        "1 - torebka foglia", "1 - plecak marrone"
    };
    private readonly string[] _categoriesToSkip =
    {
        "Odzież / Damska / DODATKI", "Odzież / Damska / PROMOCJE", "Odzież / Damska / Plus Size"
    };

    public SyncProductStocksWithWholesalerRequestHandler(
        IGetXmlDocumentFromWholesalerService wholesalerService,
        IProductAttributeService productAttributeService, 
        IProductMetaService metaService,
        ICacheProvider cacheProvider,
        IConfiguration configuration,
        WoocommerceDbContext dbContext)
    {
        _wholesalerService = wholesalerService;
        _productAttributeService = productAttributeService;
        _metaService = metaService;
        _cacheProvider = cacheProvider;
        _baseClientUrl = configuration.GetValue<string>("BaseClientUrl");
        _dbContext = dbContext;
    }

    public async Task<Result<SyncProductStocksWithWholesalerResponse>> Handle(
        SyncProductStocksWithWholesalerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var xmlDoc = await _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(cancellationToken);

            var parentProducts = await _dbContext
                .WpPosts
                .Where(x => x.PostType == "product")
                .ToListAsync(cancellationToken);

            var ids = parentProducts.Select(x => x.Id).ToList();

            var variantProducts = await _dbContext
                .WpPosts
                .Where(x => ids.Contains(x.PostParent))
                .ToListAsync(cancellationToken);

            ids.AddRange(variantProducts.Select(x => x.Id).ToList());

            var productMetaDetails = await _dbContext
                .WpPostmeta
                .Where(x => ids.Contains(x.PostId))
                .Where(x => _metaKeys.Contains(x.MetaKey))
                .ToListAsync(cancellationToken);

            var xmlProducts = xmlDoc.GetElementsByTagName(HurtIvonXmlConstrains.Produkt);

            var syncedPostIdsWithWholesaler = await SyncXmlProductsWithWoocommerceDb(
                parentProducts, productMetaDetails, xmlProducts, cancellationToken);

            var updatedProductsOutOfStock = await UpdateProductsAndVariantsOutOfStock(
                parentProducts, variantProducts, productMetaDetails, syncedPostIdsWithWholesaler);

            var response = new SyncProductStocksWithWholesalerResponse(
                syncedPostIdsWithWholesaler.Count, updatedProductsOutOfStock);

            return Result.Ok(response);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    private async Task<int> UpdateProductsAndVariantsOutOfStock(
        List<WpPost> parentProducts, List<WpPost> variantProducts, List<WpPostmetum> productMetaDetails, Dictionary<ulong, List<ulong>> syncedProductIdsWithWholesaler)
    {
        var notUpdatableProductId = productMetaDetails
            .Where(x => x.MetaKey == MetaKeyConstrains.Sku)
            .Where(x => _notUpdatableSkus.Contains(x.MetaValue))
            .Select(x => x.PostId) 
            .ToList();

        var filteredProducts = parentProducts
            .Where(x => !notUpdatableProductId.Contains(x.Id))
            .ToList();

        foreach (var item in filteredProducts)
        {
            syncedProductIdsWithWholesaler.TryGetValue(item.Id, out var syncedVariantWithCatalogIds);
            var variantProductsMissingInWholesalerCatalog = new List<WpPost>();

            var isParentProductMissingInCatalog = syncedProductIdsWithWholesaler.ContainsKey(item.Id) == false;

            if (isParentProductMissingInCatalog)
            {
                var parentProductMeta = productMetaDetails?
                    .Where(x => x.PostId == item.Id)?
                    .Where(x => x.MetaKey == MetaKeyConstrains.StockStatus)?
                    .FirstOrDefault();

                if (parentProductMeta == null) continue;

                parentProductMeta.MetaValue = "outofstock";

                var variantProductsAffected = variantProducts
                    .Where(x => x.PostParent == item.Id)
                    .ToList();

                variantProductsMissingInWholesalerCatalog.AddRange(variantProductsAffected);
            }
            else
            {
                variantProductsMissingInWholesalerCatalog = variantProducts
                    .Where(x => x.PostParent == item.Id)
                    .Where(x => !syncedVariantWithCatalogIds.Contains(x.Id))
                    .ToList();
            }

            foreach (var variantProductMissingInCatalog in variantProductsMissingInWholesalerCatalog)
            {
                var variantProductMeta = productMetaDetails
                    .Where(x => x.PostId == variantProductMissingInCatalog.Id)
                    .ToList();

                if (variantProductMeta.Any() == false 
                    || variantProductMeta.Any(x => x.MetaKey == MetaKeyConstrains.Stock) == false
                    || variantProductMeta.Any(x => x.MetaKey == MetaKeyConstrains.StockStatus) == false) 
                    continue;

                variantProductMeta.FirstOrDefault(x => x.MetaKey == MetaKeyConstrains.Stock).MetaValue = "0";
                variantProductMeta.FirstOrDefault(x => x.MetaKey == MetaKeyConstrains.StockStatus).MetaValue = "outofstock";
            }
        }

        var updatedRecords = await _dbContext.SaveChangesAsync();
        return updatedRecords;
    }

    private async Task<Dictionary<ulong, List<ulong>>> SyncXmlProductsWithWoocommerceDb(List<WpPost> parentProducts, List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts, CancellationToken cancellationToken)
    {
        var syncedPostIdsWithWholesaler = new Dictionary<ulong, List<ulong>>();

        foreach (XmlNode xmlProduct in xmlProducts)
        {
            var parentProductWpPostDto = new WpPostDto();
            var variantProductWpPostDtos = new List<WpPostDto>();
            var missingVariantsWpPostDtos = new List<WpPostDto>();
            var variants = (XmlNodeList)null;
            var parentPostId = (ulong)0;
            var variantPostIds = new List<ulong>();
            var productCanBeAdded = true;

            foreach (XmlNode child in xmlProduct.ChildNodes)
            {
                if (child.Name == HurtIvonXmlConstrains.Category && _categoriesToSkip.Contains(child.InnerText))
                {
                    productCanBeAdded = false;
                    continue;
                }      

                if (child.Name == HurtIvonXmlConstrains.Nazwa) parentProductWpPostDto.PostTitle = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.KodKatalogowy) parentProductWpPostDto.Sku = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.Opis) parentProductWpPostDto.PostContent = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.Kolor) parentProductWpPostDto.AttributeKolor = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.Fason) parentProductWpPostDto.AttributeFason = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.Dlugosc) parentProductWpPostDto.AttributeDlugosc = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.Wzor) parentProductWpPostDto.AttributeWzor = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.Rozmiar) parentProductWpPostDto.AttributeRozmiar = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.CenaKatalogowa) parentProductWpPostDto.RegularPrice = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstrains.Zdjecia) foreach (XmlNode photo in child.ChildNodes) parentProductWpPostDto.ImageUrls?.Add(photo.InnerText.Trim());
                if (child.Name == HurtIvonXmlConstrains.Warianty) variants = child.ChildNodes;
            }

            if (productCanBeAdded == false) 
                continue;

            foreach (XmlNode variant in variants)
            {
                var variantProductWpPostDto = new WpPostDto();

                foreach (XmlNode child in variant.ChildNodes)
                {
                    if (child.Name == HurtIvonXmlConstrains.Ean) variantProductWpPostDto.Sku = child.InnerText.Trim();
                    if (child.Name == HurtIvonXmlConstrains.Rozmiar) variantProductWpPostDto.AttributeRozmiar = child.InnerText.Trim();
                    if (child.Name == HurtIvonXmlConstrains.DostepnaIlosc) variantProductWpPostDto.Stock = child.InnerText.Trim();
                }

                if (string.IsNullOrEmpty(variantProductWpPostDto.Sku))
                    continue;

                variantProductWpPostDto.StockStatus = variantProductWpPostDto.StockInt > 0 ? "instock" : "outofstock";
                variantProductWpPostDtos.Add(variantProductWpPostDto);

                if (IsNewVariantInNewParent(productMetaDetails, parentProductWpPostDto, variantProductWpPostDto))
                {
                    variantProductWpPostDto.PostTitle = parentProductWpPostDto.PostTitle;
                    variantProductWpPostDto.RegularPrice = parentProductWpPostDto.RegularPrice;
                    missingVariantsWpPostDtos.Add(variantProductWpPostDto);
                }
                else if (IsNewVariantInExistingParent(productMetaDetails, variantProductWpPostDto))
                {
                    parentPostId = productMetaDetails
                        .Where(x => x.MetaKey == MetaKeyConstrains.Sku)
                        .Where(x => x.MetaValue == parentProductWpPostDto.Sku)
                        .First()
                        .PostId;

                    var parentPost = parentProducts.First(x => x.Id == parentPostId);
                    var regularPrice = productMetaDetails?
                        .FirstOrDefault(x => x.PostId == parentPostId && x.MetaKey == MetaKeyConstrains.Price)?
                        .MetaValue ?? "0";

                    variantProductWpPostDto.PostTitle = parentPost.PostTitle;
                    variantProductWpPostDto.RegularPrice = regularPrice;
                    missingVariantsWpPostDtos.Add(variantProductWpPostDto);
                }
                else //Only update existing variant
                {
                    var variantPostId = productMetaDetails
                        .Where(x => x.MetaKey == MetaKeyConstrains.Sku)
                        .Where(x => x.MetaValue == variantProductWpPostDto.Sku)
                        .First()
                        .PostId;

                    variantPostIds.Add(variantPostId);

                    var variantProductMeta = productMetaDetails?
                        .Where(x => x.PostId == variantPostId)?
                        .ToList();

                    variantProductMeta.First(x => x.MetaKey == MetaKeyConstrains.Stock).MetaValue = variantProductWpPostDto.Stock;
                    variantProductMeta.First(x => x.MetaKey == MetaKeyConstrains.StockStatus).MetaValue = variantProductWpPostDto.StockStatus;
                }
            }

            parentProductWpPostDto.StockStatus = variantProductWpPostDtos
                .Sum(x => x.StockInt) > 0 ? "instock" : "outofstock";

            if (IsNewParent(productMetaDetails, parentProductWpPostDto))
            {
                parentPostId = await StoreNewParentProduct(parentProductWpPostDto, variantProductWpPostDtos, cancellationToken);
                
                var newVariantPostIds = await StoreNewVariantProducts(missingVariantsWpPostDtos, parentPostId, cancellationToken);
                variantPostIds.AddRange(newVariantPostIds);
            }
            else //Only update existing parent
            {
                parentPostId = productMetaDetails
                    .Where(x => x.MetaKey == MetaKeyConstrains.Sku)
                    .Where(x => x?.MetaValue == parentProductWpPostDto.Sku)
                    .First()
                    .PostId;

                var parentProductMeta = productMetaDetails?
                    .Where(x => x.PostId == parentPostId)?
                    .ToList();

                parentProductMeta.First(x => x.MetaKey == MetaKeyConstrains.StockStatus).MetaValue = parentProductWpPostDto.StockStatus;
                
                var newVariantPostIds = await StoreNewVariantProducts(missingVariantsWpPostDtos, parentPostId, cancellationToken);
                variantPostIds.AddRange(newVariantPostIds);
            }

            syncedPostIdsWithWholesaler.TryAdd(parentPostId, variantPostIds);
        }

        return syncedPostIdsWithWholesaler;
    }

    private static bool IsNewParent(List<WpPostmetum> productMetaDetails, WpPostDto parentProductWpPostDto) 
        => !productMetaDetails.Any(x => x.MetaKey == MetaKeyConstrains.Sku && x.MetaValue == parentProductWpPostDto.Sku);

    private static bool IsNewVariantInExistingParent(List<WpPostmetum> productMetaDetails, WpPostDto variantProductWpPostDto) 
        => !productMetaDetails.Any(x => x.MetaKey == MetaKeyConstrains.Sku && x.MetaValue == variantProductWpPostDto.Sku);

    private static bool IsNewVariantInNewParent(List<WpPostmetum> productMetaDetails, WpPostDto parentProductWpPostDto, WpPostDto variantProductWpPostDto) 
        => !(productMetaDetails.Any(x => x.MetaKey == MetaKeyConstrains.Sku && x.MetaValue == variantProductWpPostDto.Sku)
            || productMetaDetails.Any(x => x.MetaKey == MetaKeyConstrains.Sku && x.MetaValue == parentProductWpPostDto.Sku));

    private async Task<ulong> StoreNewParentProduct(
        WpPostDto parentWpPostDto, 
        List<WpPostDto> variantProducts,
        CancellationToken cancellationToken)
    {
        var wpPost = new WpPost
        {
            PostDate = DateTime.Now,
            PostAuthor = 2,
            PostDateGmt = DateTime.UtcNow,
            PostContent = parentWpPostDto.PostContent,
            PostTitle = parentWpPostDto.PostTitle,
            PostExcerpt = string.Empty,
            PostStatus = "draft", //publish
            CommentStatus = "open",
            PingStatus = "closed",
            PostPassword = string.Empty,
            PostName = string.Empty,
            ToPing = string.Empty,
            Pinged = string.Empty,
            PostModified = DateTime.Now,
            PostModifiedGmt = DateTime.UtcNow,
            PostContentFiltered = string.Empty,
            PostParent = 0,
            Guid = string.Empty,
            MenuOrder = 0,
            PostType = "product",
            PostMimeType = string.Empty,
            CommentCount = 0
        };

        await _dbContext.WpPosts.AddAsync(wpPost, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var parentPostId = wpPost.Id;
        wpPost.Guid = $"{_baseClientUrl}/?post_type=product&p={parentPostId}";

        var productAttributesString = _productAttributeService.CreateProductAttributesString(parentWpPostDto, variantProducts);
        var terms = _cacheProvider.GetAllWpTerms();
        var (attributesLookups, termRelationships) = await _productAttributeService.MapParentProductTaxonomyValues(parentPostId, parentWpPostDto, variantProducts, terms);
        var parentMetaLookup = _metaService.GenerateParentProductMetaLookup(parentWpPostDto, parentPostId);

        var postMetas = new List<WpPostmetum>
        {
            new WpPostmetum(parentPostId, MetaKeyConstrains.Sku, parentWpPostDto.Sku),
            new WpPostmetum(parentPostId, MetaKeyConstrains.TotalSales, "0"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.TaxStatus, "taxable"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.TaxClass, string.Empty),
            new WpPostmetum(parentPostId, MetaKeyConstrains.ManageStock, "no"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.Backorders, "no"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.SoldIndividually, "no"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.Virtual, "no"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.DownloadLimit, "-1"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.DownloadExpiry, "-1"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.Stock, null),
            new WpPostmetum(parentPostId, MetaKeyConstrains.StockStatus, parentWpPostDto.StockStatus),
            new WpPostmetum(parentPostId, MetaKeyConstrains.WcAverageRating, "0"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.WcReviewCount, "0"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.ProductAttributes, productAttributesString),
            new WpPostmetum(parentPostId, MetaKeyConstrains.ProductVersion, "7.7.0"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.HasParent, "no"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.Resync, "yes"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.WooseaExcludeProduct, "no"),
            new WpPostmetum(parentPostId, MetaKeyConstrains.Price, (decimal.Parse(parentWpPostDto.RegularPrice)).ToString("0")),
        };

        await _dbContext.WpPostmeta.AddRangeAsync(postMetas, cancellationToken);
        await _dbContext.WpTermRelationships.AddRangeAsync(termRelationships, cancellationToken);
        await _dbContext.WpWcProductAttributesLookups.AddRangeAsync(attributesLookups, cancellationToken);
        await _dbContext.WpWcProductMetaLookups.AddRangeAsync(parentMetaLookup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return parentPostId;
    }

    private async Task<List<ulong>> StoreNewVariantProducts(List<WpPostDto> variantWpPostDtos, ulong parentPostId, CancellationToken cancellationToken)
    {
        var variantPostIds = new List<ulong>();
        var menuOrder = 1;
        
        foreach (var variantWpPostDto in variantWpPostDtos)
        {
            var wpPost = new WpPost
            {
                PostDate = DateTime.Now,
                PostAuthor = 2,
                PostDateGmt = DateTime.UtcNow,
                PostContent = string.Empty,
                PostTitle = $"{variantWpPostDto.PostTitle} - {variantWpPostDto.AttributeRozmiar}",
                PostExcerpt = $"Rozmiar: {variantWpPostDto.AttributeRozmiar}",
                PostStatus = "publish",
                CommentStatus = "closed",
                PingStatus = "closed",
                PostPassword = string.Empty,
                PostName = $"{variantWpPostDto.PostTitle} {variantWpPostDto.AttributeRozmiar}".Replace(" ", "-").Replace("/", "-").ToLower(),
                ToPing = string.Empty,
                Pinged = string.Empty,
                PostModified = DateTime.Now,
                PostModifiedGmt = DateTime.UtcNow,
                PostContentFiltered = string.Empty,
                PostParent = parentPostId,
                Guid = string.Empty,
                MenuOrder = menuOrder++,
                PostType = "product_variation",
                PostMimeType = string.Empty,
                CommentCount = 0
            };

            await _dbContext.WpPosts.AddAsync(wpPost, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var variantPostId = wpPost.Id;
            wpPost.Guid = $"{_baseClientUrl}/?post_type=product_variation&p={variantPostId}";
            variantPostIds.Add(variantPostId);

            var terms = _cacheProvider.GetAllWpTerms();
            var taxonomy = await _productAttributeService.MapVariableProductTaxonomyValues(variantPostId, parentPostId, variantWpPostDto, terms);
            var relationships = taxonomy.Relationships;
            var attributeLookups = taxonomy.AttributesLookups;
            var variantMetaLookup = _metaService.GenerateVariantProductMetaLookup(variantWpPostDto, variantPostId);

            var postMetas = new List<WpPostmetum>
            {
                new WpPostmetum(variantPostId, MetaKeyConstrains.VariantDescription, string.Empty),
                new WpPostmetum(variantPostId, MetaKeyConstrains.TotalSales, "0"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.TaxStatus, "taxable"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.TaxClass, "parent"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.ManageStock, "yes"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.Backorders, "no"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.SoldIndividually, "no"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.Virtual, "no"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.Downloadable, "no"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.DownloadLimit, "-1"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.DownloadExpiry, "-1"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.Stock, variantWpPostDto.Stock),
                new WpPostmetum(variantPostId, MetaKeyConstrains.StockStatus, variantWpPostDto.StockStatus),
                new WpPostmetum(variantPostId, MetaKeyConstrains.WcAverageRating, "0"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.WcReviewCount, "0"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.AttributePaRozmiar, variantWpPostDto.AttributeRozmiar.Replace(" ", "-").Replace("/", "-").ToLower()),
                new WpPostmetum(variantPostId, MetaKeyConstrains.ProductVersion, "7.7.0"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.Sku, variantWpPostDto.Sku),
                new WpPostmetum(variantPostId, MetaKeyConstrains.RegularPrice, (decimal.Parse(variantWpPostDto.RegularPrice)).ToString("0")),
                new WpPostmetum(variantPostId, MetaKeyConstrains.ThumbnailId, "0"),
                new WpPostmetum(variantPostId, MetaKeyConstrains.Price, (decimal.Parse(variantWpPostDto.RegularPrice)).ToString("0")),
            };

            await _dbContext.WpPostmeta.AddRangeAsync(postMetas, cancellationToken);
            await _dbContext.WpTermRelationships.AddRangeAsync(relationships, cancellationToken);
            await _dbContext.WpWcProductAttributesLookups.AddRangeAsync(attributeLookups, cancellationToken);
            await _dbContext.WpWcProductMetaLookups.AddRangeAsync(variantMetaLookup);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return variantPostIds;
    }
}
