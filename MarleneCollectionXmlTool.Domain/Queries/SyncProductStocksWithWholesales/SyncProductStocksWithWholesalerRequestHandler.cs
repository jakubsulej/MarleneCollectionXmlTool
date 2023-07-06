using FluentResults;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;
using MarleneCollectionXmlTool.Domain.Services;
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

            var xmlProducts = xmlDoc.GetElementsByTagName("produkt");

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
        foreach (var item in parentProducts)
        {
            syncedProductIdsWithWholesaler.TryGetValue(item.Id, out var syncedVariantWithCatalogIds);
            var variantProductsMissingInWholesalerCatalog = new List<WpPost>();

            var isParentProductMissingInCatalog = syncedProductIdsWithWholesaler.ContainsKey(item.Id) == false;

            if (isParentProductMissingInCatalog)
            {
                var parentProductMeta = productMetaDetails?
                    .Where(x => x.PostId == item.Id)?
                    .Where(x => x.MetaKey == "_stock_status")?
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
                    || variantProductMeta.Any(x => x.MetaKey == "_stock") == false
                    || variantProductMeta.Any(x => x.MetaKey == "_stock_status") == false) 
                    continue;

                variantProductMeta.FirstOrDefault(x => x.MetaKey == "_stock").MetaValue = "0";
                variantProductMeta.FirstOrDefault(x => x.MetaKey == "_stock_status").MetaValue = "outofstock";
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
            var canBeAdded = true;

            foreach (XmlNode child in xmlProduct.ChildNodes)
            {
                if (child.Name == "category" && child.InnerText == "Odzież / Damska / DODATKI")
                {
                    canBeAdded = false;
                    continue;
                }      

                if (child.Name == "nazwa") parentProductWpPostDto.PostTitle = child.InnerText.Trim();
                if (child.Name == "kod_katalogowy") parentProductWpPostDto.Sku = child.InnerText.Trim();
                if (child.Name == "opis") parentProductWpPostDto.PostContent = child.InnerText.Trim();
                if (child.Name == "kolor") parentProductWpPostDto.AttributeKolor = child.InnerText.Trim();
                if (child.Name == "fason") parentProductWpPostDto.AttributeFason = child.InnerText.Trim();
                if (child.Name == "dlugosc") parentProductWpPostDto.AttributeDlugosc = child.InnerText.Trim();
                if (child.Name == "wzor") parentProductWpPostDto.AttributeWzor = child.InnerText.Trim();
                if (child.Name == "rozmiar") parentProductWpPostDto.AttributeRozmiar = child.InnerText.Trim();
                if (child.Name == "cena_katalogowa") parentProductWpPostDto.RegularPrice = child.InnerText.Trim();
                if (child.Name == "zdjecia") foreach (XmlNode photo in child.ChildNodes) parentProductWpPostDto.ImageUrls?.Add(photo.InnerText.Trim());
                if (child.Name == "warianty") variants = child.ChildNodes;
            }

            if (canBeAdded == false) 
                continue;

            foreach (XmlNode variant in variants)
            {
                var variantProductWpPostDto = new WpPostDto();

                foreach (XmlNode child in variant.ChildNodes)
                {
                    if (child.Name == "ean") variantProductWpPostDto.Sku = child.InnerText.Trim();
                    if (child.Name == "rozmiar") variantProductWpPostDto.AttributeRozmiar = child.InnerText.Trim();
                    if (child.Name == "dostepna_ilosc") variantProductWpPostDto.Stock = child.InnerText.Trim();
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
                        .Where(x => x.MetaKey == "_sku")
                        .Where(x => x.MetaValue == parentProductWpPostDto.Sku)
                        .First()
                        .PostId;

                    var parentPost = parentProducts.First(x => x.Id == parentPostId);
                    var regularPrice = productMetaDetails?
                        .FirstOrDefault(x => x.PostId == parentPostId && x.MetaKey == "_price")?
                        .MetaValue ?? "0";

                    variantProductWpPostDto.PostTitle = parentPost.PostTitle;
                    variantProductWpPostDto.RegularPrice = regularPrice;
                    missingVariantsWpPostDtos.Add(variantProductWpPostDto);
                }
                else //Only update existing variant
                {
                    var variantPostId = productMetaDetails
                        .Where(x => x.MetaKey == "_sku")
                        .Where(x => x.MetaValue == variantProductWpPostDto.Sku)
                        .First()
                        .PostId;

                    variantPostIds.Add(variantPostId);

                    var variantProductMeta = productMetaDetails?
                        .Where(x => x.PostId == variantPostId)?
                        .ToList();

                    variantProductMeta.First(x => x.MetaKey == "_stock").MetaValue = variantProductWpPostDto.Stock;
                    variantProductMeta.First(x => x.MetaKey == "_stock_status").MetaValue = variantProductWpPostDto.StockStatus;
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
                    .Where(x => x.MetaKey == "_sku")
                    .Where(x => x?.MetaValue == parentProductWpPostDto.Sku)
                    .First()
                    .PostId;

                var parentProductMeta = productMetaDetails?
                    .Where(x => x.PostId == parentPostId)?
                    .ToList();

                parentProductMeta.First(x => x.MetaKey == "_stock_status").MetaValue = parentProductWpPostDto.StockStatus;
                
                var newVariantPostIds = await StoreNewVariantProducts(missingVariantsWpPostDtos, parentPostId, cancellationToken);
                variantPostIds.AddRange(newVariantPostIds);
            }

            syncedPostIdsWithWholesaler.TryAdd(parentPostId, variantPostIds);
        }

        return syncedPostIdsWithWholesaler;
    }

    private static bool IsNewParent(List<WpPostmetum> productMetaDetails, WpPostDto parentProductWpPostDto) 
        => !productMetaDetails.Any(x => x.MetaKey == "_sku" && x.MetaValue == parentProductWpPostDto.Sku);

    private static bool IsNewVariantInExistingParent(List<WpPostmetum> productMetaDetails, WpPostDto variantProductWpPostDto) 
        => !productMetaDetails.Any(x => x.MetaKey == "_sku" && x.MetaValue == variantProductWpPostDto.Sku);

    private static bool IsNewVariantInNewParent(List<WpPostmetum> productMetaDetails, WpPostDto parentProductWpPostDto, WpPostDto variantProductWpPostDto) 
        => !(productMetaDetails.Any(x => x.MetaKey == "_sku" && x.MetaValue == variantProductWpPostDto.Sku)
            || productMetaDetails.Any(x => x.MetaKey == "_sku" && x.MetaValue == parentProductWpPostDto.Sku));

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
            new WpPostmetum(parentPostId, "_sku", parentWpPostDto.Sku),
            new WpPostmetum(parentPostId, "total_sales", "0"),
            new WpPostmetum(parentPostId, "_tax_status", "taxable"),
            new WpPostmetum(parentPostId, "_tax_class", string.Empty),
            new WpPostmetum(parentPostId, "_manage_stock", "no"),
            new WpPostmetum(parentPostId, "_backorders", "no"),
            new WpPostmetum(parentPostId, "_sold_individually", "no"),
            new WpPostmetum(parentPostId, "_virtual", "no"),
            new WpPostmetum(parentPostId, "_download_limit", "-1"),
            new WpPostmetum(parentPostId, "_download_expiry", "-1"),
            new WpPostmetum(parentPostId, "_stock", null),
            new WpPostmetum(parentPostId, "_stock_status", parentWpPostDto.StockStatus),
            new WpPostmetum(parentPostId, "_wc_average_rating", "0"),
            new WpPostmetum(parentPostId, "_wc_review_count", "0"),
            new WpPostmetum(parentPostId, "_product_attributes", productAttributesString),
            new WpPostmetum(parentPostId, "_product_version", "7.7.0"),
            new WpPostmetum(parentPostId, "has_parent", "no"),
            new WpPostmetum(parentPostId, "resync", "yes"),
            new WpPostmetum(parentPostId, "_woosea_exclude_product", "no"),
            new WpPostmetum(parentPostId, "_price", (decimal.Parse(parentWpPostDto.RegularPrice)).ToString("0")),
            //new WpPostmetum(postId, "_thumbnail_id", "5832"),
            //new WpPostmetum(postId, "_product_image_gallery", "5833,5835,5834"),
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
                new WpPostmetum(variantPostId, "_variation_description", string.Empty),
                new WpPostmetum(variantPostId, "total_sales", "0"),
                new WpPostmetum(variantPostId, "_tax_status", "taxable"),
                new WpPostmetum(variantPostId, "_tax_class", "parent"),
                new WpPostmetum(variantPostId, "_manage_stock", "yes"),
                new WpPostmetum(variantPostId, "_backorders", "no"),
                new WpPostmetum(variantPostId, "_sold_individually", "no"),
                new WpPostmetum(variantPostId, "_virtual", "no"),
                new WpPostmetum(variantPostId, "_downloadable", "no"),
                new WpPostmetum(variantPostId, "_download_limit", "-1"),
                new WpPostmetum(variantPostId, "_download_expiry", "-1"),
                new WpPostmetum(variantPostId, "_stock", variantWpPostDto.Stock),
                new WpPostmetum(variantPostId, "_stock_status", variantWpPostDto.StockStatus),
                new WpPostmetum(variantPostId, "_wc_average_rating", "0"),
                new WpPostmetum(variantPostId, "_wc_review_count", "0"),
                new WpPostmetum(variantPostId, "attribute_pa_rozmiar", variantWpPostDto.AttributeRozmiar.Replace(" ", "-").Replace("/", "-").ToLower()),
                new WpPostmetum(variantPostId, "_product_version", "7.7.0"),
                new WpPostmetum(variantPostId, "_sku", variantWpPostDto.Sku),
                new WpPostmetum(variantPostId, "_regular_price", (decimal.Parse(variantWpPostDto.RegularPrice)).ToString("0")),
                new WpPostmetum(variantPostId, "_thumbnail_id", "0"),
                new WpPostmetum(variantPostId, "_price", (decimal.Parse(variantWpPostDto.RegularPrice)).ToString("0")),
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
