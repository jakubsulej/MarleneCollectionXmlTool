using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;

namespace MarleneCollectionXmlTool.Domain.Services;

public interface IProductMetaService
{
    WpWcProductMetaLookup GenerateParentProductMetaLookup(WpPostDto parentPost, ulong productId);
    WpWcProductMetaLookup GenerateVariantProductMetaLookup(WpPostDto variantPost, ulong productId);
}

public class ProductMetaService : IProductMetaService
{
    public WpWcProductMetaLookup GenerateParentProductMetaLookup(WpPostDto parentPost, ulong productId)
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
            TaxStatus = "taxable",
            TaxClass = string.Empty
        };

        return metaLookup;
    }

    public WpWcProductMetaLookup GenerateVariantProductMetaLookup(WpPostDto variantPost, ulong productId)
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
            TaxStatus = "taxable",
            TaxClass = "parent"
        };

        return metaLookup;
    }
}
