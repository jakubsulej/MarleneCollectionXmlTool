using FakeItEasy;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales;
using MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;
using MarleneCollectionXmlTool.Domain.Services;
using MarleneCollectionXmlTool.Domain.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Queries.SyncProductStocksWithWolesales;

public class SyncProductStocksWithWolesalerRequestHandlerTests
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ICacheProvider _cacheProvider;
    private readonly IProductMetaService _propdctMetaService;
    private readonly SyncProductStocksWithWholesalerRequestHandler _sut;
    private readonly IGetXmlDocumentFromWholesalerService _wholesalerService;
    private readonly IProductAttributeService _productAttributeHelper;

    public SyncProductStocksWithWolesalerRequestHandlerTests()
    {
        _wholesalerService = A.Fake<IGetXmlDocumentFromWholesalerService>();
        _productAttributeHelper = A.Fake<IProductAttributeService>();
        A.CallTo(() => _productAttributeHelper.CreateProductAttributesString(A<WpPostDto>._, A<List<WpPostDto>>._)).Returns(string.Empty);
        _dbContext = FakeDbContextFactory.CreateMockDbContext<WoocommerceDbContext>();
        _configuration = A.Fake<IConfiguration>();
        _cacheProvider = A.Fake<ICacheProvider>();
        _propdctMetaService = A.Fake<IProductMetaService>();
        _sut = new SyncProductStocksWithWholesalerRequestHandler(_wholesalerService, _productAttributeHelper, _propdctMetaService, _cacheProvider, _configuration, _dbContext);
    }

    /// <summary>D20-ZIELON.xml</summary>
    [Fact] 
    public async Task SyncProductStocksWithWholesaler_DoNotUpdatedDataOtherThanStock()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var originalWpPosts = MockDataHelper.GetFakeProductWithVariations();
        _dbContext.SeedRange(originalWpPosts);

        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile(expectedParentSku);
        A.CallTo(() => _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(A<CancellationToken>._)).Returns(xmlDocument);

        //Act
        var response = await _sut.Handle(new SyncProductStocksWithWholesalerRequest(), cancellationToken);

        //Assert
        var wpPosts = await _dbContext.WpPosts.ToListAsync();
        var wpMetas = await _dbContext.WpPostmeta.ToListAsync();
        var parentPost = wpPosts?.FirstOrDefault(x => x.PostType == "product");
        var variantPosts = wpPosts?.Where(x => x.PostParent == parentPost?.Id)?.ToList();

        var skus = wpMetas?
            .Where(x => x.MetaKey == "_sku")?
            .Select(x => new { x.PostId, x.MetaValue })?
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))?
            .ToDictionary(x => x.PostId, x => x.MetaValue);

        var parentStockStatus = wpMetas?
            .Where(x => x.PostId == parentPost?.Id)?
            .Where(x => x.MetaKey == "_stock_status")
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))
            .FirstOrDefault()?
            .MetaValue;

        Assert.True(response.IsSuccess);
        Assert.NotNull(parentPost);
        Assert.Equal(5, variantPosts?.Count);
        Assert.Equal(expectedParentSku, skus?.FirstOrDefault(x => x.Key == parentPost?.Id).Value);
        Assert.Equal("instock", parentStockStatus);

        var variantDetails = GetVariantDetails(wpMetas, parentPost, variantPosts, skus);
        AssertVariantDetails("dummy post title", "dummy post name", 38, "instock", "XS/S", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 3, "instock", "M/L", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 11, "instock", "XL/XXL", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 2, "instock", "3XL/4XL", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 8, "instock", "5XL/6XL", 123, "5908214231812", variantDetails);
    }

    /// <summary>D20-ZIELON.xml</summary>
    [Fact]
    public async Task SyncProductStocksWithWholesaler_AddMissingVariantsThatInheritsTitlesAndPricesFromParent()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var originalWpPosts = MockDataHelper.GetFakeProductWithVariations();
        originalWpPosts.Remove(originalWpPosts.First(x => x.Id == 6));
        originalWpPosts.Remove(originalWpPosts.First(x => x.Id == 5));
        _dbContext.SeedRange(originalWpPosts);

        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile(expectedParentSku);
        A.CallTo(() => _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(A<CancellationToken>._)).Returns(xmlDocument);

        //Act
        var response = await _sut.Handle(new SyncProductStocksWithWholesalerRequest(), cancellationToken);

        //Assert
        var wpPosts = await _dbContext.WpPosts.ToListAsync();
        var wpMetas = await _dbContext.WpPostmeta.ToListAsync();
        var parentPost = wpPosts?.FirstOrDefault(x => x.PostType == "product");
        var variantPosts = wpPosts?.Where(x => x.PostParent == parentPost?.Id)?.ToList();

        var skus = wpMetas?
            .Where(x => x.MetaKey == "_sku")?
            .Select(x => new { x.PostId, x.MetaValue })?
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))?
            .ToDictionary(x => x.PostId, x => x.MetaValue);

        var parentStockStatus = wpMetas?
            .Where(x => x.PostId == parentPost?.Id)?
            .Where(x => x.MetaKey == "_stock_status")
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))
            .FirstOrDefault()?
            .MetaValue;

        Assert.True(response.IsSuccess);
        Assert.NotNull(parentPost);
        Assert.Equal(5, variantPosts?.Count);
        Assert.Equal(expectedParentSku, skus?.FirstOrDefault(x => x.Key == parentPost?.Id).Value);
        Assert.Equal("instock", parentStockStatus);

        var variantDetails = GetVariantDetails(wpMetas, parentPost, variantPosts, skus);
        AssertVariantDetails("dummy post title", "dummy post name", 38, "instock", "XS/S", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 3, "instock", "M/L", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 11, "instock", "XL/XXL", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title - 3XL/4XL", "dummy-post-title-3xl-4xl", 2, "instock", "3XL/4XL", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title - 5XL/6XL", "dummy-post-title-5xl-6xl", 8, "instock", "5XL/6XL", 123, "5908214231812", variantDetails);
    }

    /// <summary>D20-ZIELON.xml</summary>
    [Fact]
    public async Task AddMissingProductWithVariantions_AddedProductWithAllVariants()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile(expectedParentSku);
        A.CallTo(() => _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(A<CancellationToken>._)).Returns(xmlDocument);

        //Act
        var response = await _sut.Handle(new SyncProductStocksWithWholesalerRequest(), cancellationToken);

        //Assert
        var wpPosts = await _dbContext.WpPosts.ToListAsync();
        var wpMetas = await _dbContext.WpPostmeta.ToListAsync();
        var parentPost = wpPosts?.FirstOrDefault(x => x.PostType == "product");
        var variantPosts = wpPosts?.Where(x => x.PostParent == parentPost?.Id)?.ToList();

        var skus = wpMetas?
            .Where(x => x.MetaKey == "_sku")?
            .Select(x => new { x.PostId, x.MetaValue })?
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))?
            .ToDictionary(x => x.PostId, x => x.MetaValue);

        var parentStockStatus = wpMetas?
            .Where(x => x.PostId == parentPost?.Id)?
            .Where(x => x.MetaKey == "_stock_status")
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))
            .FirstOrDefault()?
            .MetaValue;

        Assert.True(response.IsSuccess);
        Assert.NotNull(parentPost);
        Assert.Equal(5, variantPosts?.Count);
        Assert.Equal(expectedParentSku, skus?.FirstOrDefault(x => x.Key == parentPost?.Id).Value);
        Assert.Equal("instock", parentStockStatus);

        var variantDetails = GetVariantDetails(wpMetas, parentPost, variantPosts, skus);
        AssertVariantDetails("Komplet dresowy LOUIS - XS/S", "komplet-dresowy-louis-xs-s", 38, "instock", "XS/S", 199, "5908214227099", variantDetails);
        AssertVariantDetails("Komplet dresowy LOUIS - M/L", "komplet-dresowy-louis-m-l", 3, "instock", "M/L", 199, "5908214227082", variantDetails);
        AssertVariantDetails("Komplet dresowy LOUIS - XL/XXL", "komplet-dresowy-louis-xl-xxl", 11, "instock", "XL/XXL", 199, "5908214231799", variantDetails);
        AssertVariantDetails("Komplet dresowy LOUIS - 3XL/4XL", "komplet-dresowy-louis-3xl-4xl", 2, "instock", "3XL/4XL", 199, "590821423180", variantDetails);
        AssertVariantDetails("Komplet dresowy LOUIS - 5XL/6XL", "komplet-dresowy-louis-5xl-6xl", 8, "instock", "5XL/6XL", 199, "5908214231812", variantDetails);
    }

    /// <summary>D20-ZIELON-LessVariants.xml</summary>
    [Fact]
    public async Task WholesalerHasFewerVariantsThanSystem_MissingVariantsInSystemAreSetToOutOfStock()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var originalWpPosts = MockDataHelper.GetFakeProductWithVariations();
        _dbContext.SeedRange(originalWpPosts);

        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile("D20-ZIELON-LessVariants");
        A.CallTo(() => _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(A<CancellationToken>._)).Returns(xmlDocument);

        //Act
        var response = await _sut.Handle(new SyncProductStocksWithWholesalerRequest(), cancellationToken);

        //Assert
        var wpPosts = await _dbContext.WpPosts.ToListAsync();
        var wpMetas = await _dbContext.WpPostmeta.ToListAsync();
        var parentPost = wpPosts?.FirstOrDefault(x => x.PostType == "product");
        var variantPosts = wpPosts?.Where(x => x.PostParent == parentPost?.Id)?.ToList();

        var skus = wpMetas?
            .Where(x => x.MetaKey == "_sku")?
            .Select(x => new { x.PostId, x.MetaValue })?
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))?
            .ToDictionary(x => x.PostId, x => x.MetaValue);

        var parentStockStatus = wpMetas?
            .Where(x => x.PostId == parentPost?.Id)?
            .Where(x => x.MetaKey == "_stock_status")
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))
            .FirstOrDefault()?
            .MetaValue;

        Assert.True(response.IsSuccess);
        Assert.NotNull(parentPost);
        Assert.Equal(5, variantPosts?.Count);
        Assert.Equal(expectedParentSku, skus?.FirstOrDefault(x => x.Key == parentPost?.Id).Value);
        Assert.Equal("instock", parentStockStatus);

        var variantDetails = GetVariantDetails(wpMetas, parentPost, variantPosts, skus);
        AssertVariantDetails("dummy post title", "dummy post name", 38, "instock", "XS/S", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "M/L", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "XL/XXL", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "3XL/4XL", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "5XL/6XL", 123, "5908214231812", variantDetails);
    }

    /// <summary>D8-BORDO.xml</summary>
    [Fact]
    public async Task WholesalerHasNoProduct_MissingProductWithAllVariantsInSystemAreSetToOutOfStock()
    {
        //Arrange
        var cancellationToken = new CancellationToken();

        var originalWpPosts = MockDataHelper.GetFakeProductWithVariations();
        _dbContext.SeedRange(originalWpPosts);

        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile("D8-BORDO");
        A.CallTo(() => _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(A<CancellationToken>._)).Returns(xmlDocument);

        //Act
        var response = await _sut.Handle(new SyncProductStocksWithWholesalerRequest(), cancellationToken);

        //Assert
        var wpPosts = await _dbContext.WpPosts.ToListAsync();
        var wpMetas = await _dbContext.WpPostmeta.ToListAsync();
        var parentPostId = wpMetas.Where(x => x.MetaKey == "_sku").Where(x => x.MetaValue == "D20-ZIELON").First().PostId;
        var parentPost = wpPosts.Where(x => x.PostType == "product").Where(x => x.Id == parentPostId).First();

        var variantPosts = wpPosts?
            .Where(x => x.PostParent == parentPost?.Id)?
            .ToList();

        var skus = wpMetas?
            .Where(x => x.MetaKey == "_sku")?
            .Select(x => new { x.PostId, x.MetaValue })?
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))?
            .Where(x => variantPosts!.Select(y => y.Id).Contains(x.PostId))
            .ToDictionary(x => x.PostId, x => x.MetaValue);

        var parentStockStatus = wpMetas?
            .Where(x => x.PostId == parentPost?.Id)?
            .Where(x => x.MetaKey == "_stock_status")
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))
            .FirstOrDefault()?
            .MetaValue;

        Assert.True(response.IsSuccess);
        Assert.Equal("outofstock", parentStockStatus);

        var variantDetails = GetVariantDetails(wpMetas, parentPost, variantPosts, skus);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "XS/S", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "M/L", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "XL/XXL", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "3XL/4XL", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "5XL/6XL", 123, "5908214231812", variantDetails);
    }

    private static void AssertVariantDetails(
        string postTitle, string postName, int stock, string stockStatus, string attributeRozmiar, decimal price, string variantSku,
        Dictionary<string, (string postTitle, string postName, int stock, string stockStatus, string attributeRozmiar, decimal price)> variantDetails)
    {
        Assert.Equal(postTitle, variantDetails[variantSku].postTitle);
        Assert.Equal(postName, variantDetails[variantSku].postName);
        Assert.Equal(stock, variantDetails[variantSku].stock);
        Assert.Equal(stockStatus, variantDetails[variantSku].stockStatus);
        Assert.Equal(attributeRozmiar, variantDetails[variantSku].attributeRozmiar);
        Assert.Equal(price, variantDetails[variantSku].price);
    }

    private static Dictionary<string, (string postTitle, string postName, int stock, string stockStatus, string attributeRozmiar, decimal price)> GetVariantDetails(List<WpPostmetum>? wpMetas, WpPost? parentPost, List<WpPost>? variantPosts, Dictionary<ulong, string>? skus)
    {
        var variantDetails = new Dictionary<string, (string postTitle, string postName, int stock, string stockStatus, string attributeRozmiar, decimal price)>();

        foreach (var sku in skus!.Where(x => x.Key != parentPost!.Id))
        {
            var variantPost = variantPosts?.First(x => x.Id == sku.Key);
            var postTitle = variantPost!.PostTitle;
            var postName = variantPost!.PostName;
            var variantMeta = wpMetas?.Where(x => x.PostId == sku.Key).ToList();
            var stock = int.Parse(variantMeta!.FirstOrDefault(x => x.MetaKey == "_stock")!.MetaValue);
            var variantStockStatus = variantMeta!.FirstOrDefault(x => x.MetaKey == "_stock_status")!.MetaValue;
            var attributeRozmiar = variantMeta!.FirstOrDefault(x => x.MetaKey == "attribute_rozmiar")!.MetaValue;
            var variantPrice = decimal.Parse(variantMeta!.FirstOrDefault(x => x.MetaKey == "_price")!.MetaValue);
            variantDetails.Add(sku.Value, new(postTitle, postName, stock, variantStockStatus, attributeRozmiar, variantPrice));
        }

        return variantDetails;
    }
}
