﻿using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;

public interface IProductPriceService
{
    Task<int> UpdateProductPrices(List<WpPost> parentProducts, List<WpPost> allVariantProducts, List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts);
}

public class ProductPriceService : IProductPriceService
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly IProductPromoPriceValueProvider _priceValueProvider;
    private readonly IProductCategoryService _productCategoryService;

    public ProductPriceService(
        WoocommerceDbContext dbContext, 
        IProductPromoPriceValueProvider priceValueProvider,
        IProductCategoryService productCategoryService)
    {
        _dbContext = dbContext;
        _priceValueProvider = priceValueProvider;
        _productCategoryService = productCategoryService;
    }

    public async Task<int> UpdateProductPrices(
        List<WpPost> parentProducts, List<WpPost> allVariantProducts,
        List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts)
    {
        var productIds = parentProducts.Select(x => (long)x.Id).ToList();
        productIds.AddRange(allVariantProducts.Select(x => (long)x.Id));

        var metaLookups = await _dbContext.WpWcProductMetaLookups
            .Where(x => productIds.Contains(x.ProductId))
            .ToListAsync();

        var index = 0;
        foreach (XmlNode xmlProduct in xmlProducts)
        {
            var sku = string.Empty;
            var catalogRegularPrice = 0m;
            var catalogPromoPrice = (decimal?)null;

            foreach (XmlNode child in xmlProduct.ChildNodes)
            {
                if (child.Name == HurtIvonXmlConstans.KodKatalogowy) sku = child.InnerText.Trim();
                else if (child.Name == HurtIvonXmlConstans.CenaKatalogowa) catalogRegularPrice = decimal.Parse(child.InnerText.Trim());
                else if (child.Name == HurtIvonXmlConstans.CenaPromo) catalogPromoPrice = decimal.Parse(child.InnerText.Trim());
            }

            var parentMetaLookup = metaLookups.FirstOrDefault(x => x.Sku == sku);
            var productId = parentMetaLookup.ProductId;

            var parentPost = parentProducts.FirstOrDefault(x => x.Id == (ulong)productId);

            var parentPriceMeta = productMetaDetails?
                .Where(x => x.PostId == (ulong)productId)?
                .Where(x => x.MetaKey == MetaKeyConstans.Price)?
                .FirstOrDefault();

            _ = decimal.TryParse(parentPriceMeta?.MetaValue, out var currentParentPrice);

            if (parentPriceMeta != null) parentPriceMeta.MetaValue = catalogRegularPrice.ToString();
            parentMetaLookup.MaxPrice = catalogRegularPrice;
            parentMetaLookup.MinPrice = catalogPromoPrice ?? catalogRegularPrice;

            var variantProducts = allVariantProducts.Where(x => x.PostParent == (ulong)productId).ToList();

            foreach (var variantProduct in variantProducts)
            {
                var priceMeta = productMetaDetails
                    .Where(x => x.PostId == variantProduct.Id)
                    .Where(x => x.MetaKey == MetaKeyConstans.Price)
                    .FirstOrDefault();

                var regularPriceMeta = productMetaDetails
                    .Where(x => x.PostId == variantProduct.Id)
                    .Where(x => x.MetaKey == MetaKeyConstans.RegularPrice)
                    .FirstOrDefault();

                var salesPriceMeta = productMetaDetails?
                    .Where(x => x.PostId == variantProduct.Id)?
                    .Where(x => x.MetaKey == MetaKeyConstans.SalePrice)?
                    .FirstOrDefault();

                var variantMetaLookup = metaLookups.FirstOrDefault(x => x.ProductId == (long)variantProduct.Id);

                _ = decimal.TryParse(regularPriceMeta.MetaValue, out var currentRegularPrice);
                decimal? currentPromoPrice = decimal.TryParse(salesPriceMeta?.MetaValue, out var tempVal1) ? tempVal1 : null;

                var (newRegularPrice, newPromoPrice) =
                    _priceValueProvider.GetNewProductPrice(catalogRegularPrice, catalogPromoPrice, currentRegularPrice, currentPromoPrice);

                priceMeta.MetaValue = newPromoPrice != null ? newPromoPrice.ToString() : newRegularPrice.ToString();

                regularPriceMeta.MetaValue = newRegularPrice.ToString();

                if (salesPriceMeta != null) 
                    salesPriceMeta.MetaValue = newPromoPrice.ToString();

                variantMetaLookup.MaxPrice = newRegularPrice;
                variantMetaLookup.MinPrice = newPromoPrice ?? newRegularPrice;

                if (newPromoPrice != null && salesPriceMeta != null)
                {
                    salesPriceMeta.MetaValue = newPromoPrice.ToString();
                }
                else if (newPromoPrice != null && salesPriceMeta == null) 
                {
                    await _dbContext.AddAsync(new WpPostmetum
                    {
                        PostId = variantProduct.Id,
                        MetaKey = MetaKeyConstans.SalePrice,
                        MetaValue = newPromoPrice.ToString(),
                    });
                }
                else if (salesPriceMeta != null)
                {
                    _dbContext.Remove(salesPriceMeta);
                }

                index++;
            }

            index++;
        }

        return index;
    }
}
