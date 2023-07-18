using Google.Protobuf.WellKnownTypes;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Services;

public interface IProductPriceService
{
    Task<int> UpdateProductPrices(List<WpPost> parentProducts, List<WpPost> allVariantProducts, List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts);
}

public class ProductPriceService : IProductPriceService
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly IProductPromoPriceValueProvider _priceValueProvider;

    public ProductPriceService(WoocommerceDbContext dbContext, IProductPromoPriceValueProvider priceValueProvider)
    {
        _dbContext = dbContext;
        _priceValueProvider = priceValueProvider;
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
                if (child.Name == HurtIvonXmlConstrains.KodKatalogowy) sku = child.InnerText.Trim();
                else if (child.Name == HurtIvonXmlConstrains.CenaKatalogowa) catalogPrice = decimal.Parse(child.InnerText.Trim());
                else if (child.Name == HurtIvonXmlConstrains.CenaPromo) promoPrice = decimal.Parse(child.InnerText.Trim());
            }

            var metaLookup = metaLookups.FirstOrDefault(x => x.Sku == sku);
            var productId = metaLookup.ProductId;

            var parentPriceMeta = productMetaDetails?
                .Where(x => x.PostId == (ulong)productId)?
                .Where(x => x.MetaKey == MetaKeyConstrains.Price)?
                .FirstOrDefault();

            _ = decimal.TryParse(parentPriceMeta.MetaValue, out var currentParentPrice);

            parentPriceMeta.MetaValue = catalogPrice.ToString();
            metaLookup.MaxPrice = catalogPrice;
            metaLookup.MinPrice = promoPrice ?? catalogPrice;

            var variantProducts = allVariantProducts.Where(x => x.PostParent == (ulong)productId).ToList();

            foreach (var variantProduct in variantProducts)
            {
                var priceMeta = productMetaDetails
                    .Where(x => x.PostId == variantProduct.Id)
                    .Where(x => x.MetaKey == MetaKeyConstrains.Price)
                    .FirstOrDefault();

                var regularPriceMeta = productMetaDetails
                    .Where(x => x.PostId == variantProduct.Id)
                    .Where(x => x.MetaKey == MetaKeyConstrains.RegularPrice)
                    .FirstOrDefault();

                var salesPriceMeta = productMetaDetails?
                    .Where(x => x.PostId == variantProduct.Id)?
                    .Where(x => x.MetaKey == MetaKeyConstrains.SalePrice)?
                    .FirstOrDefault();

                _ = decimal.TryParse(regularPriceMeta.MetaValue, out var currentPrice);
                decimal? currentPromoPrice = decimal.TryParse(salesPriceMeta.MetaValue, out var tempVal1) ? tempVal1 : null;

                var (newRegularPrice, newPromoPrice) = 
                    _priceValueProvider.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

                priceMeta.MetaValue = newPromoPrice != null ? newPromoPrice.ToString() : newRegularPrice.ToString();
                regularPriceMeta.MetaValue = newRegularPrice.ToString();
                salesPriceMeta.MetaValue = newPromoPrice.ToString();
                metaLookup.MaxPrice = newRegularPrice;
                metaLookup.MinPrice = promoPrice ?? newRegularPrice;

                if (newPromoPrice != null) salesPriceMeta.MetaValue = newPromoPrice.ToString();
                else productMetaDetails.Remove(salesPriceMeta);
            }

            index++;
        }

        return index;
    }
}
