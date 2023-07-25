using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Utils;

namespace MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;

public interface IProductStockStatusService
{
    int UpdateProductsAndVariantsOutOfStock(List<WpPost> parentProducts, List<WpPost> variantProducts, List<WpPostmetum> productMetaDetails, Dictionary<ulong, List<ulong>> syncedProductIdsWithWholesaler);
}

public class ProductStockStatusService : IProductStockStatusService
{
    private readonly List<string> _notUpdatableSkus;

    public ProductStockStatusService(IConfigurationArrayProvider configurationArrayProvider)
    {
        _notUpdatableSkus = configurationArrayProvider.GetNotUpdatableSkus();
    }

    public int UpdateProductsAndVariantsOutOfStock(
        List<WpPost> parentProducts,
        List<WpPost> variantProducts,
        List<WpPostmetum> productMetaDetails,
        Dictionary<ulong, List<ulong>> syncedProductIdsWithWholesaler)
    {
        var notUpdatableProductId = productMetaDetails
            .Where(x => x.MetaKey == MetaKeyConstans.Sku)
            .Where(x => _notUpdatableSkus.Contains(x.MetaValue))
            .Select(x => x.PostId)
            .ToList();

        var filteredProducts = parentProducts
            .Where(x => !notUpdatableProductId.Contains(x.Id))
            .ToList();

        var updated = 1;
        foreach (var item in filteredProducts)
        {
            syncedProductIdsWithWholesaler.TryGetValue(item.Id, out var syncedVariantWithCatalogIds);
            var variantProductsMissingInWholesalerCatalog = new List<WpPost>();

            var isParentProductMissingInCatalog = syncedProductIdsWithWholesaler.ContainsKey(item.Id) == false;

            if (isParentProductMissingInCatalog)
            {
                var parentProductMeta = productMetaDetails?
                    .Where(x => x.PostId == item.Id)?
                    .Where(x => x.MetaKey == MetaKeyConstans.StockStatus)?
                    .FirstOrDefault();

                if (parentProductMeta == null) continue;

                parentProductMeta.MetaValue = MetaValueConstans.OutOfStock;

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
                    || variantProductMeta.Any(x => x.MetaKey == MetaKeyConstans.Stock) == false
                    || variantProductMeta.Any(x => x.MetaKey == MetaKeyConstans.StockStatus) == false)
                    continue;

                variantProductMeta.FirstOrDefault(x => x.MetaKey == MetaKeyConstans.Stock).MetaValue = "0";
                variantProductMeta.FirstOrDefault(x => x.MetaKey == MetaKeyConstans.StockStatus).MetaValue = MetaValueConstans.OutOfStock;

                updated++;
            }

            updated++;
        }

        return updated;
    }
}
