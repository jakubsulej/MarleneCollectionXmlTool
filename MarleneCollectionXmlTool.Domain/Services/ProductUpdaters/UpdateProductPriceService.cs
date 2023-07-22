using Google.Protobuf.WellKnownTypes;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;

public interface IUpdateProductPriceService
{
    Task<int> UpdateProductPrices(List<WpPost> parentProducts, List<WpPost> allVariantProducts, List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts);
}

public class UpdateProductPriceService : IUpdateProductPriceService
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly IProductPromoPriceValueProvider _priceValueProvider;
    private readonly IProductCategoryService _productCategoryService;

    public UpdateProductPriceService(
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
            var catalogPrice = 0m;
            var promoPrice = (decimal?)null;

            foreach (XmlNode child in xmlProduct.ChildNodes)
            {
                if (child.Name == HurtIvonXmlConstans.KodKatalogowy) sku = child.InnerText.Trim();
                else if (child.Name == HurtIvonXmlConstans.CenaKatalogowa) catalogPrice = decimal.Parse(child.InnerText.Trim());
                else if (child.Name == HurtIvonXmlConstans.CenaPromo) promoPrice = decimal.Parse(child.InnerText.Trim());
            }

            var parentMetaLookup = metaLookups.FirstOrDefault(x => x.Sku == sku);
            var productId = parentMetaLookup.ProductId;

            var parentPost = parentProducts.FirstOrDefault(x => x.Id == (ulong)productId);

            //if (promoPrice != null)
            //    await _productCategoryService.UpdateProductCategory(parentPost, WpTermSlugConstrains.Promocje);
            //else if (promoPrice == null && parentMetaLookup.MinPrice < parentMetaLookup.MaxPrice)
            //    await _productCategoryService.RemoveProductCategory(parentPost, WpTermSlugConstrains.Promocje);

            var parentPriceMeta = productMetaDetails?
                .Where(x => x.PostId == (ulong)productId)?
                .Where(x => x.MetaKey == MetaKeyConstans.Price)?
                .FirstOrDefault();

            _ = decimal.TryParse(parentPriceMeta?.MetaValue, out var currentParentPrice);

            if (parentPriceMeta != null) parentPriceMeta.MetaValue = catalogPrice.ToString();
            parentMetaLookup.MaxPrice = catalogPrice;
            parentMetaLookup.MinPrice = promoPrice ?? catalogPrice;

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

                _ = decimal.TryParse(regularPriceMeta.MetaValue, out var currentPrice);
                decimal? currentPromoPrice = decimal.TryParse(salesPriceMeta?.MetaValue, out var tempVal1) ? tempVal1 : null;

                var (newRegularPrice, newPromoPrice) =
                    _priceValueProvider.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

                priceMeta.MetaValue = newPromoPrice != null ? newPromoPrice.ToString() : newRegularPrice.ToString();

                regularPriceMeta.MetaValue = newRegularPrice.ToString();

                if (salesPriceMeta != null) 
                    salesPriceMeta.MetaValue = newPromoPrice.ToString();

                variantMetaLookup.MaxPrice = newRegularPrice;
                variantMetaLookup.MinPrice = promoPrice ?? newRegularPrice;

                if (newPromoPrice != null && salesPriceMeta != null)
                {
                    salesPriceMeta.MetaValue = newPromoPrice.ToString();
                    //await _productCategoryService.UpdateProductCategory(variantProduct, WpTermSlugConstrains.Promocje);
                }
                else if (newPromoPrice != null && salesPriceMeta == null) 
                {
                    await _dbContext.AddAsync(new WpPostmetum
                    {
                        PostId = variantProduct.Id,
                        MetaKey = MetaKeyConstans.SalePrice,
                        MetaValue = newPromoPrice.ToString(),
                    });
                    //await _productCategoryService.UpdateProductCategory(variantProduct, WpTermSlugConstrains.Promocje);
                }
                else if (salesPriceMeta != null)
                {
                    _dbContext.Remove(salesPriceMeta);
                    //await _productCategoryService.RemoveProductCategory(variantProduct, WpTermSlugConstrains.Promocje);
                }

                index++;
            }

            index++;
        }

        return index;
    }
}
