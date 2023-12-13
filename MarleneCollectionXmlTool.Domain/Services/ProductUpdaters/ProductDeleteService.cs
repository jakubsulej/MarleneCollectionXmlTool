using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;

public interface IProductDeleteService
{
    Task<int> DeleteReduntantVariants(List<WpPost> variantProducts, List<WpPostmetum> productMetaDetails, Dictionary<ulong, List<ulong>> syncedProductWithWholesaler);
}

public class ProductDeleteService : IProductDeleteService
{
    private readonly WoocommerceDbContext _dbContext;

    public ProductDeleteService(WoocommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> DeleteReduntantVariants(
        List<WpPost> variantProducts,
        List<WpPostmetum> existingProductMetaDetails,
        Dictionary<ulong, List<ulong>> syncedProductWithWholesaler)
    {
        var synchedVariantProductIds = syncedProductWithWholesaler.SelectMany(x => x.Value).ToList();
        var missingVariantsInCatalog = variantProducts
            .Where(x => !synchedVariantProductIds.Contains(x.Id))
            .ToList();

        var missingProductInCatalogIds = missingVariantsInCatalog.Select(x => x.Id).ToList();
        var missingProductInCatalogSizeMetaDetails = existingProductMetaDetails
            .Where(x => missingProductInCatalogIds.Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstans.AttributePaRozmiar)
            .ToList();

        var synchedProductSizeMetaDetails = await _dbContext.WpPostmeta
            .Where(x => synchedVariantProductIds.Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstans.AttributePaRozmiar)
            .ToListAsync();

        var removedProducts = 0;
        foreach (var variant in missingVariantsInCatalog)
        {
            syncedProductWithWholesaler.TryGetValue(variant.PostParent, out var synchedVariantIdsForCurrentParent);
            var synchedMetaDetails = synchedProductSizeMetaDetails
                .Where(x => synchedVariantIdsForCurrentParent.Contains(x.PostId))
                .Select(x => x.MetaValue)
                .ToList();

            var currentMetaValue = missingProductInCatalogSizeMetaDetails
                .FirstOrDefault(x => x.PostId == variant.Id)
                .MetaValue;

            if (synchedMetaDetails.Any(attribute => attribute == currentMetaValue))
            {
                _dbContext.Remove(variant);

                existingProductMetaDetails
                    .Where(x => x.PostId == variant.Id)
                    .ToList()
                    .ForEach(meta =>
                    {
                        _dbContext.Remove(meta);
                    });

                removedProducts++;
            }
        }

        return removedProducts;
    }
}
