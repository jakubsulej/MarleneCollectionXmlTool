using FluentResults;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Services.ClientSevices;
using MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;
using MarleneCollectionXmlTool.Domain.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarleneCollectionXmlTool.Domain.Commands.SyncProductStocksWithWholesales;

public class SyncProductStocksWithWholesalerRequestHandler : IRequestHandler<SyncProductStocksWithWholesalerRequest, Result<SyncProductStocksWithWholesalerResponse>>
{
    private readonly ISyncWoocommerceProductsWithWholesalerService _syncProductsWithWholesalerService;
    private readonly IGetXmlDocumentFromWholesalerService _wholesalerService;
    private readonly IUpdateProductPriceService _productPriceService;
    private readonly WoocommerceDbContext _dbContext;
    private readonly List<string> _notUpdatableSkus;

    public SyncProductStocksWithWholesalerRequestHandler(
        ISyncWoocommerceProductsWithWholesalerService syncProductsWithWholesalerService,
        IGetXmlDocumentFromWholesalerService wholesalerService,
        IConfigurationArrayProvider configurationArrayProvider,
        IUpdateProductPriceService productPriceService,
        WoocommerceDbContext dbContext)
    {
        _syncProductsWithWholesalerService = syncProductsWithWholesalerService;
        _wholesalerService = wholesalerService;
        _productPriceService = productPriceService;
        _notUpdatableSkus = configurationArrayProvider.GetNotUpdatableSkus();
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
                .Where(x => x.PostType == WpPostConstans.Product)
                .ToListAsync(cancellationToken);

            var ids = parentProducts.Select(x => x.Id).ToList();

            var variantProducts = await _dbContext
                .WpPosts
                .Where(x => ids.Contains(x.PostParent))
                .Where(x => x.PostType == WpPostConstans.ProductVariation)
                .ToListAsync(cancellationToken);

            ids.AddRange(variantProducts.Select(x => x.Id).ToList());

            var productMetaDetails = await _dbContext
                .WpPostmeta
                .Where(x => ids.Contains(x.PostId))
                .Where(x => MetaKeyConstans.AcceptableMetaKeys.Contains(x.MetaKey))
                .ToListAsync(cancellationToken);

            var xmlProducts = xmlDoc.GetElementsByTagName(HurtIvonXmlConstans.Produkt);

            var syncedPostIdsWithWholesaler = await _syncProductsWithWholesalerService.SyncXmlProductsWithWoocommerceDb(
                parentProducts, productMetaDetails, xmlProducts, cancellationToken);

            var updatedPrices = await _productPriceService.UpdateProductPrices(parentProducts, variantProducts, productMetaDetails, xmlProducts);

            var updatedProductsOutOfStock = await UpdateProductsAndVariantsOutOfStock(
                parentProducts, variantProducts, productMetaDetails, syncedPostIdsWithWholesaler);

            await _dbContext.SaveChangesAsync(cancellationToken);

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
