using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;

public interface IProductCategoryService
{
    Task RemoveProductCategory(WpPost wpPost, string termSlug);
    Task UpdateProductCategory(WpPost parentPosts, string termSlug);
}

public class ProductCategoryService : IProductCategoryService
{
    private readonly ICacheProvider _cacheProvider;
    private readonly WoocommerceDbContext _dbContext;

    public ProductCategoryService(ICacheProvider cacheProvider, WoocommerceDbContext dbContext)
    {
        _cacheProvider = cacheProvider;
        _dbContext = dbContext;
    }

    public async Task UpdateProductCategory(WpPost wpPost, string termSlug)
    {
        var wpTerms = _cacheProvider.GetAllWpTerms();
        var termCategoryId = wpTerms.FirstOrDefault(x => x.Slug == termSlug.ToLower()).TermId;

        await _dbContext.AddAsync(new WpTermRelationship
        {
            ObjectId = wpPost.Id,
            TermTaxonomyId = termCategoryId,
            TermOrder = 0
        });
    }

    public async Task RemoveProductCategory(WpPost wpPost, string termSlug)
    {
        var wpTerms = _cacheProvider.GetAllWpTerms();
        var termCategoryId = wpTerms.FirstOrDefault(x => x.Slug == termSlug.ToLower()).TermId;

        var relationship = await _dbContext.WpTermRelationships
            .Where(x => x.ObjectId == wpPost.Id)
            .Where(x => x.TermTaxonomyId == termCategoryId)
            .FirstOrDefaultAsync();

        _dbContext.Remove(relationship);
    }
}
