using FakeItEasy;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;
using MarleneCollectionXmlTool.Domain.Tests.Utils;
using MarleneCollectionXmlTool.Domain.Utils;
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
    public async Task AddProductsToPromoCategory_AddAllToPromoAndRemoveAllThatAreNotPromo()
    {
        //Arrange
        var databaseWpPostsWithMetas = CreateMockDatabaseObjects();
        var databaseParentProducts = databaseWpPostsWithMetas.Where(x => x.WpPost.PostType == WpPostConstans.Product).Select(x => x.WpPost).ToList();
        var databaseVariantProducts = databaseWpPostsWithMetas.Where(x => x.WpPost.PostType == WpPostConstans.ProductVariation).Select(x => x.WpPost).ToList();
        var databaseProductMetaDetails = databaseWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();      
        var termRelationships = databaseWpPostsWithMetas.Select(postWithMeta => new WpTermRelationship(postWithMeta.WpPost.Id, _wpTerm.TermId));        

        await _dbContext.AddRangeAsync(databaseParentProducts);
        await _dbContext.AddRangeAsync(databaseVariantProducts);
        await _dbContext.AddRangeAsync(databaseProductMetaDetails);
        await _dbContext.AddRangeAsync(termRelationships);
        await _dbContext.SaveChangesAsync();

        var wpPostWithMetas = CreateMockDatabaseObjects();
        var parentProducts = wpPostWithMetas.Where(x => x.WpPost.PostType == WpPostConstans.Product).Select(x => x.WpPost).ToList();
        var variantProducts = wpPostWithMetas.Where(x => x.WpPost.PostType == WpPostConstans.ProductVariation).Select(x => x.WpPost).ToList();
        var productMetaDetails = wpPostWithMetas.SelectMany(x => x.WpPostmetum).ToList();

        //Act
        await _sut.AddProductsToPromoCategory(parentProducts, variantProducts, productMetaDetails);

        //Assert
    }

    private static List<MockDataHelper.WpPostWithMeta> CreateMockDatabaseObjects()
    {
        var price = "123";
        var regularPrice = "123";
        var salesPrice = "123";

        var variantTree = new Dictionary<string, List<MockDataHelper.FakeProductVariableValues>>
        {
            {
                "D20-ZIELON",
                new List<MockDataHelper.FakeProductVariableValues>
                {
                    new MockDataHelper.FakeProductVariableValues(Sku: "5908214227099", AttributeRozmiar: "xs-s", Stock: "3", Price: price, RegularPrice: regularPrice, SalesPrice: salesPrice, StockStatus: "instock"),
                    new MockDataHelper.FakeProductVariableValues(Sku: "5908214227082", AttributeRozmiar: "m-l", Stock: "5", Price: price, RegularPrice: regularPrice, SalesPrice: salesPrice, StockStatus: "instock"),
                    new MockDataHelper.FakeProductVariableValues(Sku: "5908214231799", AttributeRozmiar: "xl-xxl", Stock: "11", Price: price, RegularPrice: regularPrice, SalesPrice: salesPrice, StockStatus: "instock"),
                    new MockDataHelper.FakeProductVariableValues(Sku: "590821423180", AttributeRozmiar: "3xl-4xl", Stock: "0", Price: price , RegularPrice: regularPrice, SalesPrice: salesPrice, StockStatus: "outofstock"),
                    new MockDataHelper.FakeProductVariableValues(Sku: "5908214231812", AttributeRozmiar: "5xl-6xl", Stock: "0", Price: price, RegularPrice: regularPrice, SalesPrice: salesPrice, StockStatus: "outofstock"),
                }
            },
        };

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations(variantTree);
        return originalWpPostsWithMetas;
    }
}
