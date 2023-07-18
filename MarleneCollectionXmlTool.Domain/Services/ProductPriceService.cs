using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Services;

public interface IProductPriceService
{
    Task UpdateProductPrices(List<WpPost> parentProducts, List<WpPost> allVariantProducts, List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts);
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

    public async Task UpdateProductPrices(
        List<WpPost> parentProducts, List<WpPost> allVariantProducts,
        List<WpPostmetum> productMetaDetails, XmlNodeList xmlProducts)
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
            var promoPrice = (decimal?)null;

            foreach (XmlNode child in xmlProduct.ChildNodes)
            {
                if (child.Name == HurtIvonXmlConstrains.KodKatalogowy) sku = child.InnerText.Trim();
                else if (child.Name == HurtIvonXmlConstrains.CenaKatalogowa) catalogPrice = decimal.Parse(child.InnerText.Trim());
                else if (child.Name == HurtIvonXmlConstrains.CenaPromo) promoPrice = decimal.Parse(child.InnerText.Trim());
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

            var (newRegularPrice, newPromoPrice) = _priceValueProvider.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

            regularPriceProductMeta.MetaValue = newRegularPrice.ToString();
            promoPriceProductMeta.MetaValue = newPromoPrice.ToString();
            metaLookup.MaxPrice = newRegularPrice;
            metaLookup.MinPrice = newPromoPrice;
        }

        //_dbContext.SaveChanges();
    }
}
