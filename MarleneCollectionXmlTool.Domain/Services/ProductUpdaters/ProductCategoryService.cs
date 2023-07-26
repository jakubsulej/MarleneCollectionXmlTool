using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;

public interface IProductCategoryService
{
    Task AddProductsToPromoCategory(List<WpPost> parentProducts, List<WpPost> allVariantProducts, List<WpPostmetum> productMetaDetails);
}

public class ProductCategoryService : IProductCategoryService
{
    private readonly List<WpTerm> _wpTerms;
    private readonly WoocommerceDbContext _dbContext;

    public ProductCategoryService(ICacheProvider cacheProvider, WoocommerceDbContext dbContext)
    {
        _wpTerms = cacheProvider.GetAllWpTerms();
        _dbContext = dbContext;
    }

    public async Task AddProductsToPromoCategory(List<WpPost> parentProducts, List<WpPost> allVariantProducts, List<WpPostmetum> productMetaDetails)
    {
        var termCategoryId = _wpTerms.FirstOrDefault(x => x.Slug == WpTermSlugConstans.Promocje.ToLower()).TermId;
        var currentlyInPromoCategory = await _dbContext.WpTermRelationships
            .Where(x => x.TermTaxonomyId == termCategoryId)
            .ToListAsync();

        var currentlyInPromoCategoryIds = currentlyInPromoCategory.Select(x => x.ObjectId).ToList();

        var onSaleVariantIds = productMetaDetails
            .Where(x => !currentlyInPromoCategoryIds.Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstans.SalePrice)
            .Select(x => x.PostId)
            .ToList();

        var onSaleVariantProducts = allVariantProducts
            .Where(x => !currentlyInPromoCategoryIds.Contains(x.Id))
            .Where(x => onSaleVariantIds.Contains(x.Id))
            .Distinct()
            .ToList();

        var onSaleParentIds = onSaleVariantProducts.Select(y => y.PostParent).Distinct();
        var onSaleParentProducts = parentProducts
            .Where(x => onSaleParentIds.Contains(x.Id))
            .Distinct()
            .ToList();

        var onSaleProducts = new List<WpPost>();
        onSaleProducts.AddRange(onSaleVariantProducts);
        onSaleProducts.AddRange(onSaleParentProducts);

        foreach (var product in onSaleProducts)
        {
            _dbContext.WpTermRelationships.Add(new WpTermRelationship
            {
                ObjectId = product.Id,
                TermTaxonomyId = termCategoryId,
                TermOrder = 0
            });
        }

        var notMoreOnSaleProductIds = currentlyInPromoCategoryIds
            .Except(onSaleProducts.Select(x => x.Id))
            .ToList();

        foreach (var productId in notMoreOnSaleProductIds)
        {
            var relationship = currentlyInPromoCategory.FirstOrDefault(x => x.ObjectId == productId);
            _dbContext.WpTermRelationships.Remove(relationship);
        }
    }
}
