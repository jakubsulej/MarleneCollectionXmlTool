﻿using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Commands.SyncProductStocksWithWholesales.Models;
using MarleneCollectionXmlTool.Domain.Enums;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.Extensions.Configuration;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;

public interface ISyncWoocommerceProductsWithWholesalerService
{
    Task<Dictionary<ulong, List<ulong>>> SyncXmlProductsWithWoocommerceDb(
        List<WpPost> parentProducts, List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts, CancellationToken cancellationToken);
}

public class SyncWoocommerceProductsWithWholesalerService : ISyncWoocommerceProductsWithWholesalerService
{
    private readonly IProductAttributeService _productAttributeService;
    private readonly WoocommerceDbContext _dbContext;
    private readonly List<string> _categoriesToSkip;
    private readonly string _baseClientUrl;

    public SyncWoocommerceProductsWithWholesalerService(
        IConfiguration configuration,
        IProductAttributeService productAttributeService,
        IConfigurationArrayProvider configurationArrayProvider,
        WoocommerceDbContext dbContext)
    {
        _categoriesToSkip = configurationArrayProvider.GetCategoriesToSkip();
        _baseClientUrl = configuration.GetValue<string>(ConfigurationKeyConstans.BaseClientUrl);
        _productAttributeService = productAttributeService;
        _dbContext = dbContext;
    }

    public async Task<Dictionary<ulong, List<ulong>>> SyncXmlProductsWithWoocommerceDb(
        List<WpPost> parentProducts, List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts, CancellationToken cancellationToken)
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
                if (child.Name == HurtIvonXmlConstans.Category && _categoriesToSkip.Contains(child.InnerText))
                {
                    productCanBeAdded = false;
                    continue;
                }

                if (child.Name == HurtIvonXmlConstans.Nazwa) parentProductWpPostDto.PostTitle = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.KodKatalogowy) parentProductWpPostDto.Sku = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.Opis) parentProductWpPostDto.PostContent = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.Kolor) parentProductWpPostDto.AttributeKolor = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.Fason) parentProductWpPostDto.AttributeFason = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.Dlugosc) parentProductWpPostDto.AttributeDlugosc = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.Wzor) parentProductWpPostDto.AttributeWzor = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.Rozmiar) parentProductWpPostDto.AttributeRozmiar = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.CenaKatalogowa) parentProductWpPostDto.RegularPrice = child.InnerText.Trim();
                if (child.Name == HurtIvonXmlConstans.Zdjecia) foreach (XmlNode photo in child.ChildNodes) parentProductWpPostDto.ImageUrls?.Add(photo.InnerText.Trim());
                if (child.Name == HurtIvonXmlConstans.Warianty) variants = child.ChildNodes;
            }

            if (productCanBeAdded == false)
                continue;

            foreach (XmlNode variant in variants)
            {
                var variantProductWpPostDto = new WpPostDto();

                foreach (XmlNode child in variant.ChildNodes)
                {
                    if (child.Name == HurtIvonXmlConstans.Ean) variantProductWpPostDto.Sku = ProductSkuProvider.GetVariantProductSku(variant.ChildNodes, child.InnerText);
                    if (child.Name == HurtIvonXmlConstans.Rozmiar) variantProductWpPostDto.AttributeRozmiar = child.InnerText.Trim();
                    if (child.Name == HurtIvonXmlConstans.DostepnaIlosc) variantProductWpPostDto.Stock = child.InnerText.Trim();
                }

                if (string.IsNullOrWhiteSpace(variantProductWpPostDto.Sku))
                    continue;

                variantProductWpPostDto.StockStatus = variantProductWpPostDto.StockInt > 0 ? MetaValueConstans.InStock : MetaValueConstans.OutOfStock;
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
                        .Where(x => x.MetaKey == MetaKeyConstans.Sku)
                        .Where(x => x.MetaValue == parentProductWpPostDto.Sku)
                        .First()
                        .PostId;

                    var parentPost = parentProducts.First(x => x.Id == parentPostId);
                    var regularPrice = productMetaDetails?
                        .FirstOrDefault(x => x.PostId == parentPostId && x.MetaKey == MetaKeyConstans.Price)?
                        .MetaValue ?? "0";

                    variantProductWpPostDto.PostTitle = parentPost.PostTitle;
                    variantProductWpPostDto.RegularPrice = regularPrice;
                    missingVariantsWpPostDtos.Add(variantProductWpPostDto);
                }
                else //Only update existing variant
                {
                    var variantPostId = productMetaDetails
                        .Where(x => x.MetaKey == MetaKeyConstans.Sku)
                        .Where(x => x.MetaValue == variantProductWpPostDto.Sku)
                        .First()
                        .PostId;

                    variantPostIds.Add(variantPostId);

                    var variantProductMeta = productMetaDetails?
                        .Where(x => x.PostId == variantPostId)?
                        .ToList();

                    variantProductMeta.First(x => x.MetaKey == MetaKeyConstans.Stock).MetaValue = variantProductWpPostDto.Stock;
                    variantProductMeta.First(x => x.MetaKey == MetaKeyConstans.StockStatus).MetaValue = variantProductWpPostDto.StockStatus;
                }
            }

            parentProductWpPostDto.StockStatus = variantProductWpPostDtos
                .Sum(x => x.StockInt) > 0 ? MetaValueConstans.InStock : MetaValueConstans.OutOfStock;

            if (IsNewParent(productMetaDetails, parentProductWpPostDto))
            {
                parentPostId = await StoreNewParentProduct(parentProductWpPostDto, variantProductWpPostDtos, cancellationToken);

                var newVariantPostIds = await StoreNewVariantProducts(missingVariantsWpPostDtos, parentPostId, cancellationToken);
                variantPostIds.AddRange(newVariantPostIds);
            }
            else //Only update existing parent
            {
                parentPostId = productMetaDetails
                    .Where(x => x.MetaKey == MetaKeyConstans.Sku)
                    .Where(x => x?.MetaValue == parentProductWpPostDto.Sku)
                    .First()
                    .PostId;

                var parentProductMeta = productMetaDetails?
                    .Where(x => x.PostId == parentPostId)?
                    .ToList();

                parentProductMeta.First(x => x.MetaKey == MetaKeyConstans.StockStatus).MetaValue = parentProductWpPostDto.StockStatus;

                var newVariantPostIds = await StoreNewVariantProducts(missingVariantsWpPostDtos, parentPostId, cancellationToken);
                variantPostIds.AddRange(newVariantPostIds);
            }

            syncedPostIdsWithWholesaler.TryAdd(parentPostId, variantPostIds);
        }

        return syncedPostIdsWithWholesaler;
    }

    private static bool IsNewParent(List<WpPostmetum> productMetaDetails, WpPostDto parentProductWpPostDto)
        => !productMetaDetails.Any(x => x.MetaKey == MetaKeyConstans.Sku && x.MetaValue == parentProductWpPostDto.Sku);

    private static bool IsNewVariantInExistingParent(List<WpPostmetum> productMetaDetails, WpPostDto variantProductWpPostDto)
        => !productMetaDetails.Any(x => x.MetaKey == MetaKeyConstans.Sku && x.MetaValue == variantProductWpPostDto.Sku);

    private static bool IsNewVariantInNewParent(List<WpPostmetum> productMetaDetails, WpPostDto parentProductWpPostDto, WpPostDto variantProductWpPostDto)
        => !(productMetaDetails.Any(x => x.MetaKey == MetaKeyConstans.Sku && x.MetaValue == variantProductWpPostDto.Sku)
            || productMetaDetails.Any(x => x.MetaKey == MetaKeyConstans.Sku && x.MetaValue == parentProductWpPostDto.Sku));

    private async Task<ulong> StoreNewParentProduct(
        WpPostDto parentWpPostDto,
        List<WpPostDto> variantProducts,
        CancellationToken cancellationToken)
    {
        var wpPost = new WpPost
        {
            PostDate = DateTime.Now,
            PostAuthor = (int)PostAuthorEnum.Sulejmedia,
            PostDateGmt = DateTime.UtcNow,
            PostContent = parentWpPostDto.PostContent,
            PostTitle = parentWpPostDto.PostTitle,
            PostExcerpt = string.Empty,
            PostStatus = WpPostConstans.Draft,
            CommentStatus = WpPostConstans.Open,
            PingStatus = WpPostConstans.Closed,
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
            PostType = WpPostConstans.Product,
            PostMimeType = string.Empty,
            CommentCount = 0
        };

        await _dbContext.WpPosts.AddAsync(wpPost, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var parentPostId = wpPost.Id;
        wpPost.Guid = WpPostConstans.GetWpParentPostGuid(_baseClientUrl, parentPostId);

        var productAttributesString = _productAttributeService.CreateProductAttributesString(parentWpPostDto, variantProducts);
        var (attributesLookups, termRelationships) = await _productAttributeService.MapParentProductTaxonomyValues(parentPostId, parentWpPostDto, variantProducts);
        var parentMetaLookup = GenerateParentProductMetaLookup(parentWpPostDto, parentPostId);

        var postMetas = new List<WpPostmetum>
        {
            new WpPostmetum(parentPostId, MetaKeyConstans.Sku, parentWpPostDto.Sku),
            new WpPostmetum(parentPostId, MetaKeyConstans.TotalSales, 0),
            new WpPostmetum(parentPostId, MetaKeyConstans.TaxStatus, MetaValueConstans.Taxable),
            new WpPostmetum(parentPostId, MetaKeyConstans.TaxClass, string.Empty),
            new WpPostmetum(parentPostId, MetaKeyConstans.ManageStock, MetaValueConstans.No),
            new WpPostmetum(parentPostId, MetaKeyConstans.Backorders, MetaValueConstans.No),
            new WpPostmetum(parentPostId, MetaKeyConstans.SoldIndividually, MetaValueConstans.No),
            new WpPostmetum(parentPostId, MetaKeyConstans.Virtual, MetaValueConstans.No),
            new WpPostmetum(parentPostId, MetaKeyConstans.DownloadLimit, -1),
            new WpPostmetum(parentPostId, MetaKeyConstans.DownloadExpiry, -1),
            new WpPostmetum(parentPostId, MetaKeyConstans.Stock, null),
            new WpPostmetum(parentPostId, MetaKeyConstans.StockStatus, parentWpPostDto.StockStatus),
            new WpPostmetum(parentPostId, MetaKeyConstans.WcAverageRating, 0),
            new WpPostmetum(parentPostId, MetaKeyConstans.WcReviewCount, 0),
            new WpPostmetum(parentPostId, MetaKeyConstans.ProductAttributes, productAttributesString),
            new WpPostmetum(parentPostId, MetaKeyConstans.ProductVersion, MetaValueConstans.ProductVersion),
            new WpPostmetum(parentPostId, MetaKeyConstans.HasParent, MetaValueConstans.No),
            new WpPostmetum(parentPostId, MetaKeyConstans.Resync, MetaValueConstans.Yes),
            new WpPostmetum(parentPostId, MetaKeyConstans.WooseaExcludeProduct, MetaValueConstans.No),
            new WpPostmetum(parentPostId, MetaKeyConstans.Price, MetaValueConstans.GetPriceFormat(parentWpPostDto.RegularPrice)),
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
                PostTitle = WpPostConstans.GetPostTitle(variantWpPostDto.PostTitle, variantWpPostDto.AttributeRozmiar),
                PostExcerpt = WpPostConstans.GetPostExcerpt("Rozmiar", variantWpPostDto.AttributeRozmiar),
                PostStatus = WpPostConstans.Publish,
                CommentStatus = WpPostConstans.Closed,
                PingStatus = WpPostConstans.Closed,
                PostPassword = string.Empty,
                PostName = WpPostConstans.GetPostName(variantWpPostDto.PostTitle, variantWpPostDto.AttributeRozmiar),
                ToPing = string.Empty,
                Pinged = string.Empty,
                PostModified = DateTime.Now,
                PostModifiedGmt = DateTime.UtcNow,
                PostContentFiltered = string.Empty,
                PostParent = parentPostId,
                Guid = string.Empty,
                MenuOrder = menuOrder++,
                PostType = WpPostConstans.ProductVariation,
                PostMimeType = string.Empty,
                CommentCount = 0
            };

            await _dbContext.WpPosts.AddAsync(wpPost, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var variantPostId = wpPost.Id;
            wpPost.Guid = WpPostConstans.GetWpVariantPostGuid(_baseClientUrl, variantPostId);
            variantPostIds.Add(variantPostId);

            var taxonomy = await _productAttributeService.MapVariableProductTaxonomyValues(variantPostId, parentPostId, variantWpPostDto);
            var relationships = taxonomy.Relationships;
            var attributeLookups = taxonomy.AttributesLookups;
            var variantMetaLookup = GenerateVariantProductMetaLookup(variantWpPostDto, variantPostId);

            var postMetas = new List<WpPostmetum>
            {
                new WpPostmetum(variantPostId, MetaKeyConstans.VariantDescription, string.Empty),
                new WpPostmetum(variantPostId, MetaKeyConstans.TotalSales, 0),
                new WpPostmetum(variantPostId, MetaKeyConstans.TaxStatus, MetaValueConstans.Taxable),
                new WpPostmetum(variantPostId, MetaKeyConstans.TaxClass, MetaValueConstans.Parent),
                new WpPostmetum(variantPostId, MetaKeyConstans.ManageStock, MetaValueConstans.Yes),
                new WpPostmetum(variantPostId, MetaKeyConstans.Backorders, MetaValueConstans.No),
                new WpPostmetum(variantPostId, MetaKeyConstans.SoldIndividually, MetaValueConstans.No),
                new WpPostmetum(variantPostId, MetaKeyConstans.Virtual, MetaValueConstans.No),
                new WpPostmetum(variantPostId, MetaKeyConstans.Downloadable, MetaValueConstans.No),
                new WpPostmetum(variantPostId, MetaKeyConstans.DownloadLimit, -1),
                new WpPostmetum(variantPostId, MetaKeyConstans.DownloadExpiry, -1),
                new WpPostmetum(variantPostId, MetaKeyConstans.Stock, variantWpPostDto.Stock),
                new WpPostmetum(variantPostId, MetaKeyConstans.StockStatus, variantWpPostDto.StockStatus),
                new WpPostmetum(variantPostId, MetaKeyConstans.WcAverageRating, 0),
                new WpPostmetum(variantPostId, MetaKeyConstans.WcReviewCount, 0),
                new WpPostmetum(variantPostId, MetaKeyConstans.AttributePaRozmiar, variantWpPostDto.AttributeRozmiar.Replace(" ", "-").Replace("/", "-").ToLower()),
                new WpPostmetum(variantPostId, MetaKeyConstans.ProductVersion, MetaValueConstans.ProductVersion),
                new WpPostmetum(variantPostId, MetaKeyConstans.Sku, variantWpPostDto.Sku),
                new WpPostmetum(variantPostId, MetaKeyConstans.RegularPrice, MetaValueConstans.GetPriceFormat(variantWpPostDto.RegularPrice)),
                new WpPostmetum(variantPostId, MetaKeyConstans.ThumbnailId, 0),
                new WpPostmetum(variantPostId, MetaKeyConstans.Price, MetaValueConstans.GetPriceFormat(variantWpPostDto.RegularPrice))
            };

            await _dbContext.WpPostmeta.AddRangeAsync(postMetas, cancellationToken);
            await _dbContext.WpTermRelationships.AddRangeAsync(relationships, cancellationToken);
            await _dbContext.WpWcProductAttributesLookups.AddRangeAsync(attributeLookups, cancellationToken);
            await _dbContext.WpWcProductMetaLookups.AddRangeAsync(variantMetaLookup);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return variantPostIds;
    }

    private static WpWcProductMetaLookup GenerateParentProductMetaLookup(WpPostDto parentPost, ulong productId)
    {
        var metaLookup = new WpWcProductMetaLookup
        {
            ProductId = (long)productId,
            Sku = parentPost.Sku,
            Virtual = false,
            Downloadable = false,
            MinPrice = decimal.Parse(parentPost.RegularPrice),
            MaxPrice = decimal.Parse(parentPost.RegularPrice),
            Onsale = false,
            StockQuantity = null,
            StockStatus = parentPost.StockStatus,
            RatingCount = 0,
            AverageRating = 0,
            TotalSales = 0,
            TaxStatus = MetaValueConstans.Taxable,
            TaxClass = string.Empty
        };

        return metaLookup;
    }

    private static WpWcProductMetaLookup GenerateVariantProductMetaLookup(WpPostDto variantPost, ulong productId)
    {
        var metaLookup = new WpWcProductMetaLookup
        {
            ProductId = (long)productId,
            Sku = variantPost.Sku,
            Virtual = false,
            Downloadable = false,
            MinPrice = decimal.Parse(variantPost.RegularPrice),
            MaxPrice = decimal.Parse(variantPost.RegularPrice),
            Onsale = false,
            StockQuantity = variantPost.StockInt,
            StockStatus = variantPost.StockStatus,
            RatingCount = 0,
            AverageRating = 0,
            TotalSales = 0,
            TaxStatus = MetaValueConstans.Taxable,
            TaxClass = MetaValueConstans.Parent
        };

        return metaLookup;
    }
}
