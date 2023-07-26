using FakeItEasy;
using FluentAssertions;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;
using MarleneCollectionXmlTool.Domain.Tests.Utils;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Services;

public class ProductCategoryServiceTests
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly ICacheProvider _cacheProvider;
    private readonly WpTerm _wpTerm;
    private readonly IProductCategoryService _sut;

    public ProductCategoryServiceTests()
    {
        _dbContext = FakeDbContextFactory.CreateMockDbContext<WoocommerceDbContext>();
        _cacheProvider = A.Fake<ICacheProvider>();
        _wpTerm = new WpTerm { TermId = 1, Name = WpTermSlugConstans.Promocje, Slug = WpTermSlugConstans.Promocje };
        A.CallTo(() => _cacheProvider.GetAllWpTerms()).Returns(new List<WpTerm> { _wpTerm });
        _sut = new ProductCategoryService(_cacheProvider, _dbContext);
    }

    [Fact]
    public async Task DatabaseProductAreOnSaleNewProductAreNotOnSale_RemoveProductsThatAreNotOnSale()
    {
        //Arrange
        var wpPostsWithMetasMock = CreateMockDatabaseObjects();
        var noSaleParentPostId = (ulong)wpPostsWithMetasMock.Select(x => x.ProductMetaLookup).FirstOrDefault(x => x.Sku == "D20-ZIELON")!.ProductId;
        var saleParentPostId = (ulong)wpPostsWithMetasMock.Select(x => x.ProductMetaLookup).FirstOrDefault(x => x.Sku == "D8-BORDO")!.ProductId;

        var databaseParentProducts = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostType == WpPostConstans.Product)
            .Where(x => x.WpPost.Id == noSaleParentPostId)
            .Select(x => x.WpPost)
            .ToList();

        var databaseVariantProducts = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostType == WpPostConstans.ProductVariation)
            .Where(x => x.WpPost.PostParent == noSaleParentPostId)
            .Select(x => x.WpPost)
            .ToList();

        var databaseProductMetaDetails = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostParent == noSaleParentPostId || x.WpPost.Id == noSaleParentPostId)
            .SelectMany(x => x.WpPostmetum)
            .ToList();

        var existingTermRelationships = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostParent == noSaleParentPostId || x.WpPost.Id == noSaleParentPostId)
            .Select(postWithMeta => new WpTermRelationship(postWithMeta.WpPost.Id, _wpTerm.TermId));        

        await _dbContext.AddRangeAsync(databaseParentProducts);
        await _dbContext.AddRangeAsync(databaseVariantProducts);
        await _dbContext.AddRangeAsync(databaseProductMetaDetails);
        await _dbContext.AddRangeAsync(existingTermRelationships);
        await _dbContext.SaveChangesAsync();

        var parentProducts = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostType == WpPostConstans.Product)
            .Where(x => x.WpPost.Id == saleParentPostId)
            .Select(x => x.WpPost)
            .ToList();

        var variantProducts = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostType == WpPostConstans.ProductVariation)
            .Where(x => x.WpPost.PostParent == saleParentPostId)
            .Select(x => x.WpPost)
            .ToList();

        var productMetaDetails = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostParent == saleParentPostId || x.WpPost.Id == saleParentPostId)
            .SelectMany(x => x.WpPostmetum)
            .ToList();

        //Act
        await _sut.AddProductsToPromoCategory(parentProducts, variantProducts, productMetaDetails);
        await _dbContext.SaveChangesAsync();

        //Assert
        var wpTermRelationships = await _dbContext.WpTermRelationships.ToListAsync();
        
        var onSaleProductIds = variantProducts.Select(x => x.Id).ToList();
        onSaleProductIds.AddRange(parentProducts.Select(x => x.Id));

        var notOnSaleProductIds = databaseParentProducts.Select(x => x.Id).ToList();
        notOnSaleProductIds.AddRange(databaseVariantProducts.Select(x => x.Id));

        Assert.Equal(onSaleProductIds.Count, wpTermRelationships.Count);
        onSaleProductIds.Should().BeEquivalentTo(wpTermRelationships.Select(x => x.ObjectId));
        notOnSaleProductIds.Should().NotBeEquivalentTo(wpTermRelationships.Select(wpTermRel => wpTermRel.ObjectId));
    }

    [Fact]
    public async Task DatabaseProductAreOnSaleAndNewOnesAreNot_DatabaseProductsShouldBeRemovedFromSale()
    {
        //Arrange
        var wpPostsWithMetasMock = CreateMockDatabaseObjects();
        var noSaleParentPostId = (ulong)wpPostsWithMetasMock.Select(x => x.ProductMetaLookup).FirstOrDefault(x => x.Sku == "D20-ZIELON")!.ProductId;
        var saleParentPostId = (ulong)wpPostsWithMetasMock.Select(x => x.ProductMetaLookup).FirstOrDefault(x => x.Sku == "D8-BORDO")!.ProductId;

        var databaseParentProducts = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostType == WpPostConstans.Product)
            .Where(x => x.WpPost.Id == saleParentPostId)
            .Select(x => x.WpPost)
            .ToList();

        var databaseVariantProducts = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostType == WpPostConstans.ProductVariation)
            .Where(x => x.WpPost.PostParent == saleParentPostId)
            .Select(x => x.WpPost)
            .ToList();

        var databaseProductMetaDetails = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostParent == saleParentPostId || x.WpPost.Id == saleParentPostId)
            .SelectMany(x => x.WpPostmetum)
            .ToList();

        var existingTermRelationships = wpPostsWithMetasMock
            .Where(x => x.WpPost.PostParent == saleParentPostId || x.WpPost.Id == saleParentPostId)
            .Select(postWithMeta => new WpTermRelationship(postWithMeta.WpPost.Id, _wpTerm.TermId));

        await _dbContext.AddRangeAsync(databaseParentProducts);
        await _dbContext.AddRangeAsync(databaseVariantProducts);
        await _dbContext.AddRangeAsync(databaseProductMetaDetails);
        await _dbContext.AddRangeAsync(existingTermRelationships);
        await _dbContext.SaveChangesAsync();

        var parentProducts = new List<WpPost>();
        var variantProducts = new List<WpPost>();
        var productMetaDetails = new List<WpPostmetum>();

        //Act
        await _sut.AddProductsToPromoCategory(parentProducts, variantProducts, productMetaDetails);
        await _dbContext.SaveChangesAsync();

        //Assert
        var wpTermRelationships = await _dbContext.WpTermRelationships.ToListAsync();

        var onSaleProductIds = variantProducts.Select(x => x.Id).ToList();
        onSaleProductIds.AddRange(parentProducts.Select(x => x.Id));

        var notOnSaleProductIds = databaseParentProducts.Select(x => x.Id).ToList();
        notOnSaleProductIds.AddRange(databaseVariantProducts.Select(x => x.Id));

        Assert.Equal(onSaleProductIds.Count, wpTermRelationships.Count);
        onSaleProductIds.Should().BeEquivalentTo(wpTermRelationships.Select(x => x.ObjectId));
        notOnSaleProductIds.Should().NotBeEquivalentTo(wpTermRelationships.Select(wpTermRel => wpTermRel.ObjectId));
    }

    private static List<MockDataHelper.WpPostWithMeta> CreateMockDatabaseObjects()
    {
        var variantTree = new Dictionary<string, List<MockDataHelper.FakeProductVariableValues>>
        {
            {
                "D20-ZIELON",
                new List<MockDataHelper.FakeProductVariableValues>
                {
                    new MockDataHelper.FakeProductVariableValues("5908214227099", "xs-s", "3", Price: "123", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("5908214227082", "m-l", "5", Price: "123", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("5908214231799", "xl-xxl", "11", Price: "123", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("590821423180", "3xl-4xl", "0", Price: "123" , RegularPrice: "123", SalesPrice: null, "outofstock"),
                    new MockDataHelper.FakeProductVariableValues("5908214231812", "5xl-6xl", "0", Price: "123", RegularPrice: "123", SalesPrice: null, "outofstock"),
                }
            },
            {
                "D8-BORDO",
                new List<MockDataHelper.FakeProductVariableValues>
                {
                    new MockDataHelper.FakeProductVariableValues("5908214227303", "xs-s", "3", Price: "123", RegularPrice: "123", SalesPrice: "123", "instock"),
                    new MockDataHelper.FakeProductVariableValues("5908214227297", "m-l", "5", Price: "123", RegularPrice: "123", SalesPrice: "123", "instock"),
                    new MockDataHelper.FakeProductVariableValues("5908214231942", "xl-xxl", "11", Price: "123", RegularPrice: "123", SalesPrice: "123", "instock"),
                    new MockDataHelper.FakeProductVariableValues("590821423195", "3xl-4xl", "0", Price: "123" , RegularPrice: "123", SalesPrice: "123", "outofstock"),
                    new MockDataHelper.FakeProductVariableValues("5908214231966", "5xl-6xl", "0", Price: "123", RegularPrice: "123", SalesPrice: "123", "outofstock"),
                }
            }
        };

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations(variantTree);
        return originalWpPostsWithMetas;
    }
}
