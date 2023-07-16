using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Services;

public class ProductPriceService
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly decimal _priceMarginFactor;

    public ProductPriceService(WoocommerceDbContext dbContext)
    {
        _dbContext = dbContext;
        _priceMarginFactor = 1.0m;
    }

    public async Task UpdateProductPrices(
        List<WpPost> parentProducts, List<WpPost> allVariantProducts, 
        List<WpPostmetum> productMetaDetails, XmlNode xmlProducts)
    {
        var productIds = parentProducts.Select(x => (long)x.Id).ToList();
        productIds.AddRange(allVariantProducts.Select(x => (long)x.Id));

        var metaLookups = await _dbContext.WpWcProductMetaLookups
            .Where(x => productIds.Contains(x.ProductId))
            .ToListAsync();

        foreach (XmlNode xmlProduct in xmlProducts)
        {
            var sku = string.Empty;
            var catalogPrice = 0m;
            var promoPrice = 0m;

            foreach (XmlNode child in xmlProduct.ChildNodes)
            {
                if (child.Name == HurtIvonXmlConstrains.KodKatalogowy) sku = child.InnerText.Trim();
                else if (child.Name == HurtIvonXmlConstrains.CenaKatalogowa) catalogPrice = decimal.Parse(child.InnerText.Trim());
                else if (child.Name == HurtIvonXmlConstrains.CenaKatalogowa) promoPrice = decimal.Parse(child.InnerText.Trim());
            }

            var metaLookup = metaLookups.FirstOrDefault(x => x.Sku == sku);
            var productId = metaLookup.ProductId;
            
            var regularPriceProductMeta = productMetaDetails?
                .Where(x => x.PostId == (ulong)productId)?
                .Where(x => x.MetaKey == MetaKeyConstrains.RegularPrice)?
                .FirstOrDefault();

            var promoPriceProductMeta = productMetaDetails?
                .Where(x => x.PostId == (ulong)productId)?
                .Where(x => x.MetaKey == MetaKeyConstrains.SalePrice)?
                .FirstOrDefault();

            var variantProducts = allVariantProducts.Where(x => x.PostParent == (ulong)productId).ToList();

            _ = decimal.TryParse(regularPriceProductMeta.MetaValue, out var currentPrice);
            _ = decimal.TryParse(promoPriceProductMeta.MetaValue, out var currentPromoPrice);

            var (newRegularPrice, newPromoPrice) = GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

            regularPriceProductMeta.MetaValue = newRegularPrice.ToString();
            promoPriceProductMeta.MetaValue = newPromoPrice.ToString();
            metaLookup.MaxPrice = newRegularPrice;
            metaLookup.MinPrice = newPromoPrice;
        }

        _dbContext.SaveChanges();
    }

    public (decimal newRegularPrice, decimal? newPromoPrice) GetNewProductPrice(
        decimal catalogPrice, decimal? promoPrice, decimal currentPrice, decimal? currentPromoPrice)
    {
        if (catalogPrice == 0 && promoPrice == 0) 
            return (currentPrice, currentPromoPrice);

        if (currentPrice < catalogPrice)
            currentPrice = catalogPrice;

        currentPromoPrice = promoPrice;

        var currentPromoPriceWithMargin = currentPromoPrice * _priceMarginFactor;
        var currentPriceWithMargin = currentPrice * _priceMarginFactor;

        return (currentPriceWithMargin, currentPromoPriceWithMargin);
    }
}
