using FluentResults;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarleneCollectionXmlTool.Domain.Queries.UpdateMissingMetaLookups;

public class UpdateMetaLookupsRequestHandler : IRequestHandler<UpdateMetaLookupsRequest, Result<UpdateMetaLookupsResponse>>
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly IProductMetaService _productMetaService;

    public UpdateMetaLookupsRequestHandler(WoocommerceDbContext dbContext, IProductMetaService productMetaService)
    {
        _dbContext = dbContext;
        _productMetaService = productMetaService;
    }

    public async Task<Result<UpdateMetaLookupsResponse>> Handle(UpdateMetaLookupsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existingMetaLookupProductIds = await _dbContext.WpWcProductMetaLookups
                .Select(x => (ulong)x.ProductId)
                .ToListAsync(cancellationToken);

            var missingProductMetasFromLookup = await _dbContext.WpPostmeta
                .Where(x => existingMetaLookupProductIds.Contains(x.PostId) == false)
                .ToListAsync(cancellationToken);

            var missingProductMetasFromLookupIds = missingProductMetasFromLookup
                .Select(x => x.PostId)
                .Distinct()
                .ToList();

            var metaLookups = new List<WpWcProductMetaLookup>();

            foreach (var postId in missingProductMetasFromLookupIds)
            {
                var productMetas = missingProductMetasFromLookup
                    .Where(x => x.PostId == postId)
                    .ToList();

                if (productMetas.Any(x => x.MetaKey == "_sku") == false) continue;

                var sku = productMetas.FirstOrDefault(x => x.MetaKey == "_sku").MetaValue;
                var price = decimal.Parse(productMetas.FirstOrDefault(x => x.MetaKey == "_price").MetaValue);
                var stockStatus = productMetas.FirstOrDefault(x => x.MetaKey == "_stock_status").MetaValue;
                var parent = productMetas.FirstOrDefault(x => x.MetaKey == "has_parent")?.MetaValue?.ToUpper() == "NO";

                var lookup = new WpWcProductMetaLookup
                {
                    ProductId = (long)postId,
                    Sku = productMetas.FirstOrDefault(x => x.MetaKey == "_sku").MetaValue,
                    Virtual = false,
                    Downloadable = false,
                    MinPrice = price,
                    MaxPrice = price,
                    Onsale = false,
                    StockStatus = stockStatus,
                    RatingCount = 0,
                    AverageRating = 0,
                    TotalSales = 0,
                    TaxStatus = "taxable",
                    TaxClass = parent ? string.Empty : "parent"
                };

                metaLookups.Add(lookup);
            }

            await _dbContext.WpWcProductMetaLookups.AddRangeAsync(metaLookups, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail("Error");
        }
    }
}
