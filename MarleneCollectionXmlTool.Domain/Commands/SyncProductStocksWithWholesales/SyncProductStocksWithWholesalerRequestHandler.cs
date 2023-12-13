using FluentResults;
using MarleneCollectionXmlTool.DBAccessLayer;
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
    private readonly IProductStockStatusService _productStockStatusService;
    private readonly IProductPriceService _productPriceService;
    private readonly IProductCategoryService _productCategoryService;
    private readonly IProductDeleteService _productDeleteService;
    private readonly WoocommerceDbContext _dbContext;

    public SyncProductStocksWithWholesalerRequestHandler(
        ISyncWoocommerceProductsWithWholesalerService syncProductsWithWholesalerService,
        IGetXmlDocumentFromWholesalerService wholesalerService,
        IProductStockStatusService productStockStatusService,
        IProductPriceService productPriceService,
        IProductCategoryService productCategoryService,
        IProductDeleteService productDeleteService,
        WoocommerceDbContext dbContext)
    {
        _syncProductsWithWholesalerService = syncProductsWithWholesalerService;
        _wholesalerService = wholesalerService;
        _productStockStatusService = productStockStatusService;
        _productPriceService = productPriceService;
        _productCategoryService = productCategoryService;
        _productDeleteService = productDeleteService;
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

            await _productPriceService.UpdateProductPrices(parentProducts, variantProducts, productMetaDetails, xmlProducts);
            await _productCategoryService.AddProductsToPromoCategory(parentProducts, variantProducts, productMetaDetails);

            var updatedProductsOutOfStock = _productStockStatusService.UpdateProductsAndVariantsOutOfStock(
                parentProducts, variantProducts, productMetaDetails, syncedPostIdsWithWholesaler);

            var removedVariants = _productDeleteService.DeleteReduntantVariants(
                variantProducts, productMetaDetails, syncedPostIdsWithWholesaler);

            var savedEntities = await _dbContext.SaveChangesAsync(cancellationToken);

            var response = new SyncProductStocksWithWholesalerResponse(
                syncedPostIdsWithWholesaler.Count, updatedProductsOutOfStock);

            return Result.Ok(response);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
