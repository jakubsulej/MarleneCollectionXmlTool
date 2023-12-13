using FakeItEasy;
using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Commands.SyncProductStocksWithWholesales;
using MarleneCollectionXmlTool.Domain.Commands.SyncProductStocksWithWholesales.Models;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Services.ClientSevices;
using MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;
using MarleneCollectionXmlTool.Domain.Tests.Utils;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Queries.SyncProductStocksWithWolesales;

public class SyncProductStocksWithWolesalerRequestHandlerTests
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly SyncProductStocksWithWholesalerRequestHandler _sut;
    private readonly IGetXmlDocumentFromWholesalerService _wholesalerService;

    public SyncProductStocksWithWolesalerRequestHandlerTests()
    {
        _wholesalerService = A.Fake<IGetXmlDocumentFromWholesalerService>();
        _dbContext = FakeDbContextFactory.CreateMockDbContext<WoocommerceDbContext>();
        var productAttributeHelper = A.Fake<IProductAttributeService>();
        A.CallTo(() => productAttributeHelper.CreateProductAttributesString(A<WpPostDto>._, A<List<WpPostDto>>._)).Returns(string.Empty);      
        var configuration = A.Fake<IConfiguration>();
        var productPriceService = A.Fake<IProductPriceService>();
        var configurationArrayProvider = new ConfigurationArrayProvider(configuration);
        var syncProductWithWholesalerService = new SyncWoocommerceProductsWithWholesalerService(configuration, productAttributeHelper, configurationArrayProvider, _dbContext);
        var productStockStatusService = new ProductStockStatusService(configurationArrayProvider);
        var productDeleteService = new ProductDeleteService(_dbContext);
        var cacheProvider = A.Fake<ICacheProvider>();
        A.CallTo(() => cacheProvider.GetAllWpTerms()).Returns(new List<WpTerm> { new WpTerm { TermId = 1, Name = WpTermSlugConstans.Promocje, Slug = WpTermSlugConstans.Promocje } });
        var productCategoryService = new ProductCategoryService(cacheProvider, _dbContext);   
        
        _sut = new SyncProductStocksWithWholesalerRequestHandler(
            syncProductWithWholesalerService, 
            _wholesalerService, 
            productStockStatusService, 
            productPriceService, 
            productCategoryService,
            productDeleteService, 
            _dbContext);
    }

    /// <summary>D20-ZIELON.xml</summary>
    [Fact] 
    public async Task SyncProductStocksWithWholesaler_DoNotUpdatedDataOtherThanStock()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations();
        var originalWpPosts = originalWpPostsWithMetas.Select(x => x.WpPost).ToList();
        var originalWpMetas = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        _dbContext.SeedRange(originalWpPosts);
        _dbContext.SeedRange(originalWpMetas);

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
        AssertVariantDetails("dummy post title", "dummy post name", 38, "instock", "xs-s", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 3, "instock", "m-l", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 11, "instock", "xl-xxl", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 2, "instock", "3xl-4xl", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 8, "instock", "5xl-6xl", 123, "5908214231812", variantDetails);
    }

    /// <summary>D20-ZIELON.xml</summary>
    [Fact]
    public async Task SyncProductStocksWithWholesaler_AddMissingVariantsThatInheritsTitlesAndPricesFromParent()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations();
        originalWpPostsWithMetas.Remove(originalWpPostsWithMetas.First(x => x.WpPost.Id == 6));
        originalWpPostsWithMetas.Remove(originalWpPostsWithMetas.First(x => x.WpPost.Id == 5));

        var originalWpPosts = originalWpPostsWithMetas.Select(x => x.WpPost).ToList();
        var originalWpMetas = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        _dbContext.SeedRange(originalWpPosts);
        _dbContext.SeedRange(originalWpMetas);

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
        AssertVariantDetails("dummy post title", "dummy post name", 38, "instock", "xs-s", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 3, "instock", "m-l", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 11, "instock", "xl-xxl", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title - 3XL/4XL", "dummy-post-title-3xl-4xl", 2, "instock", "3xl-4xl", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title - 5XL/6XL", "dummy-post-title-5xl-6xl", 8, "instock", "5xl-6xl", 123, "5908214231812", variantDetails);
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
        AssertVariantDetails("Komplet dresowy LOUIS - XS/S", "komplet-dresowy-louis-xs-s", 38, "instock", "xs-s", 199, "5908214227099", variantDetails);
        AssertVariantDetails("Komplet dresowy LOUIS - M/L", "komplet-dresowy-louis-m-l", 3, "instock", "m-l", 199, "5908214227082", variantDetails);
        AssertVariantDetails("Komplet dresowy LOUIS - XL/XXL", "komplet-dresowy-louis-xl-xxl", 11, "instock", "xl-xxl", 199, "5908214231799", variantDetails);
        AssertVariantDetails("Komplet dresowy LOUIS - 3XL/4XL", "komplet-dresowy-louis-3xl-4xl", 2, "instock", "3xl-4xl", 199, "590821423180", variantDetails);
        AssertVariantDetails("Komplet dresowy LOUIS - 5XL/6XL", "komplet-dresowy-louis-5xl-6xl", 8, "instock", "5xl-6xl", 199, "5908214231812", variantDetails);
    }

    /// <summary>D20-ZIELON-LessVariants.xml</summary>
    [Fact]
    public async Task WholesalerHasFewerVariantsThanSystem_MissingVariantsInSystemAreSetToOutOfStock()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations();
        var originalWpPosts = originalWpPostsWithMetas.Select(x => x.WpPost).ToList();
        var originalWpMetas = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        _dbContext.SeedRange(originalWpPosts);
        _dbContext.SeedRange(originalWpMetas);

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
        AssertVariantDetails("dummy post title", "dummy post name", 38, "instock", "xs-s", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "m-l", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "xl-xxl", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "3xl-4xl", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "5xl-6xl", 123, "5908214231812", variantDetails);
    }

    /// <summary>D8-BORDO.xml</summary>
    [Fact]
    public async Task WholesalerHasNoProduct_MissingProductWithAllVariantsInSystemAreSetToOutOfStock()
    {
        //Arrange
        var cancellationToken = new CancellationToken();

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations();
        var originalWpPosts = originalWpPostsWithMetas.Select(x => x.WpPost).ToList();
        var originalWpMetas = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        _dbContext.SeedRange(originalWpPosts);
        _dbContext.SeedRange(originalWpMetas);

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
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "xs-s", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "m-l", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "xl-xxl", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "3xl-4xl", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 0, "outofstock", "5xl-6xl", 123, "5908214231812", variantDetails);
    }

    /// <summary>B53-CZARNY.xml</summary>
    [Fact]
    public async Task WholesalerHasProductWithoutVariantSkuField_MissingAllVariantsAreAddedWithVariantIdAsSku()
    {
        //Arrange
        var expectedParentSku = "B53-CZARNY";
        var cancellationToken = new CancellationToken();

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations();
        var originalWpPosts = originalWpPostsWithMetas.Select(x => x.WpPost).ToList();
        var originalWpMetas = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        _dbContext.SeedRange(originalWpPosts);
        _dbContext.SeedRange(originalWpMetas);

        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile(expectedParentSku);
        A.CallTo(() => _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(A<CancellationToken>._)).Returns(xmlDocument);

        //Act
        var response = await _sut.Handle(new SyncProductStocksWithWholesalerRequest(), cancellationToken);

        //Assert
        var wpPosts = await _dbContext.WpPosts.ToListAsync();
        var wpMetas = await _dbContext.WpPostmeta.ToListAsync();
        var parentPostId = wpMetas.Where(x => x.MetaKey == MetaKeyConstans.Sku).Where(x => x.MetaValue == expectedParentSku).First().PostId;
        var parentPost = wpPosts.Where(x => x.PostType == "product").Where(x => x.Id == parentPostId).First();

        var variantPosts = wpPosts?
            .Where(x => x.PostParent == parentPost?.Id)?
            .ToList();

        var skus = wpMetas?
            .Where(x => x.MetaKey == MetaKeyConstans.Sku)?
            .Select(x => new { x.PostId, x.MetaValue })?
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))?
            .Where(x => variantPosts!.Select(y => y.Id).Contains(x.PostId))
            .ToDictionary(x => x.PostId, x => x.MetaValue);

        var parentStockStatus = wpMetas?
            .Where(x => x.PostId == parentPost?.Id)?
            .Where(x => x.MetaKey == MetaKeyConstans.StockStatus)
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))
            .FirstOrDefault()?
            .MetaValue;

        Assert.True(response.IsSuccess);
        Assert.Equal("instock", parentStockStatus);

        var variantDetails = GetVariantDetails(wpMetas, parentPost, variantPosts, skus);
        AssertVariantDetails("Czarna koszula Emilie ze złotymi guzikami - S/M", "czarna-koszula-emilie-ze-złotymi-guzikami-s-m", 5, "instock", "s-m", 199, "1328-S/M", variantDetails);
        AssertVariantDetails("Czarna koszula Emilie ze złotymi guzikami - L/XL", "czarna-koszula-emilie-ze-złotymi-guzikami-l-xl", 5, "instock", "l-xl", 199, "1328-L/XL", variantDetails);
        AssertVariantDetails("Czarna koszula Emilie ze złotymi guzikami - 2XL/3XL", "czarna-koszula-emilie-ze-złotymi-guzikami-2xl-3xl", 5, "instock", "2xl-3xl", 199, "1328-2XL/3XL", variantDetails);
        AssertVariantDetails("Czarna koszula Emilie ze złotymi guzikami - 4XL/5XL", "czarna-koszula-emilie-ze-złotymi-guzikami-4xl-5xl", 5, "instock", "4xl-5xl", 199, "1328-4XL/5XL", variantDetails);
    }

    /// <summary>D20-ZIELON-NoEan.xml</summary>
    [Fact]
    public async Task WholesalerHasProductWithoutVariantSkuField_AllVariantsAreSuccessfulyUpdatedUsingIdsAsEans_GivenWholesalerProductWithNoEan()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var variantTree = new Dictionary<string, List<MockDataHelper.FakeProductVariableValues>>
        {
            {
                expectedParentSku,
                new List<MockDataHelper.FakeProductVariableValues>
                {
                    new MockDataHelper.FakeProductVariableValues("1008-XS/S", "xs-s", "12", Price: "199", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("1008-M/L", "m-l", "12", Price: "199", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("1008-XL/XXL", "xl-xxl", "12", Price: "199", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("1008-3XL/4XL", "3xl-4xl", "12", Price: "199" , RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("1008-5XL/6XL", "5xl-6xl", "0", Price: "199", RegularPrice: "123", SalesPrice: null, "outofstock"),
                }
            }
        };

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations(variantTree);
        var originalWpPosts = originalWpPostsWithMetas.Select(x => x.WpPost).ToList();
        var originalWpMetas = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        _dbContext.SeedRange(originalWpPosts);
        _dbContext.SeedRange(originalWpMetas);

        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile("D20-ZIELON-NoEan");
        A.CallTo(() => _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(A<CancellationToken>._)).Returns(xmlDocument);

        //Act
        var response = await _sut.Handle(new SyncProductStocksWithWholesalerRequest(), cancellationToken);

        //Assert
        var wpPosts = await _dbContext.WpPosts.ToListAsync();
        var wpMetas = await _dbContext.WpPostmeta.ToListAsync();
        var parentPostId = wpMetas.Where(x => x.MetaKey == MetaKeyConstans.Sku).Where(x => x.MetaValue == expectedParentSku).First().PostId;
        var parentPost = wpPosts.Where(x => x.PostType == "product").Where(x => x.Id == parentPostId).First();

        var variantPosts = wpPosts?
            .Where(x => x.PostParent == parentPost?.Id)?
            .ToList();

        var skus = wpMetas?
            .Where(x => x.MetaKey == MetaKeyConstans.Sku)?
            .Select(x => new { x.PostId, x.MetaValue })?
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))?
            .Where(x => variantPosts!.Select(y => y.Id).Contains(x.PostId))
            .ToDictionary(x => x.PostId, x => x.MetaValue);

        var parentStockStatus = wpMetas?
            .Where(x => x.PostId == parentPost?.Id)?
            .Where(x => x.MetaKey == MetaKeyConstans.StockStatus)
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))
            .FirstOrDefault()?
            .MetaValue;

        Assert.True(response.IsSuccess);
        Assert.Equal("instock", parentStockStatus);

        var variantDetails = GetVariantDetails(wpMetas, parentPost, variantPosts, skus);
        AssertVariantDetails("dummy post title", "dummy post name", 38, "instock", "xs-s", 199, "1008-XS/S", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 3, "instock", "m-l", 199, "1008-M/L", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 11, "instock", "xl-xxl", 199, "1008-XL/XXL", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 2, "instock", "3xl-4xl", 199, "1008-3XL/4XL", variantDetails);
        AssertVariantDetails("dummy post title", "dummy post name", 8, "instock", "5xl-6xl", 199, "1008-5XL/6XL", variantDetails);
    }

    /// <summary>D20-ZIELON.xml</summary>
    [Fact]
    public async Task 
        WholesalerHasProductWithoutVariantSkuField_AllVariantsAreSuccessfulyUpdatedUsingIdsAsEans_GivenWholesalerProductWithExistingEan()
    {
        //Arrange
        var expectedParentSku = "D20-ZIELON";
        var cancellationToken = new CancellationToken();

        var variantTree = new Dictionary<string, List<MockDataHelper.FakeProductVariableValues>>
        {
            {
                expectedParentSku,
                new List<MockDataHelper.FakeProductVariableValues>
                {
                    new MockDataHelper.FakeProductVariableValues("1008-XS/S", "xs-s", "12", Price: "199", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("1008-M/L", "m-l", "12", Price: "199", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("1008-XL/XXL", "xl-xxl", "12", Price: "199", RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("1008-3XL/4XL", "3xl-4xl", "12", Price: "199" , RegularPrice: "123", SalesPrice: null, "instock"),
                    new MockDataHelper.FakeProductVariableValues("1008-5XL/6XL", "5xl-6xl", "0", Price: "199", RegularPrice: "123", SalesPrice: null, "outofstock"),
                }
            }
        };

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations(variantTree);
        var originalWpPosts = originalWpPostsWithMetas.Select(x => x.WpPost).ToList();
        var originalWpMetas = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        _dbContext.SeedRange(originalWpPosts);
        _dbContext.SeedRange(originalWpMetas);

        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile("D20-ZIELON");
        A.CallTo(() => _wholesalerService.GetXmlDocumentNestedVariantsXmlUrl(A<CancellationToken>._)).Returns(xmlDocument);

        //Act
        var response = await _sut.Handle(new SyncProductStocksWithWholesalerRequest(), cancellationToken);

        //Assert
        var wpPosts = await _dbContext.WpPosts.ToListAsync();
        var wpMetas = await _dbContext.WpPostmeta.ToListAsync();
        var parentPostId = wpMetas.Where(x => x.MetaKey == MetaKeyConstans.Sku).Where(x => x.MetaValue == expectedParentSku).First().PostId;
        var parentPost = wpPosts.Where(x => x.PostType == "product").Where(x => x.Id == parentPostId).First();

        var variantPosts = wpPosts?
            .Where(x => x.PostParent == parentPost?.Id)?
            .ToList();

        var skus = wpMetas?
            .Where(x => x.MetaKey == MetaKeyConstans.Sku)?
            .Select(x => new { x.PostId, x.MetaValue })?
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))?
            .Where(x => variantPosts!.Select(y => y.Id).Contains(x.PostId))
            .ToDictionary(x => x.PostId, x => x.MetaValue);

        var parentStockStatus = wpMetas?
            .Where(x => x.PostId == parentPost?.Id)?
            .Where(x => x.MetaKey == MetaKeyConstans.StockStatus)
            .Where(x => !string.IsNullOrEmpty(x.MetaValue))
            .FirstOrDefault()?
            .MetaValue;

        Assert.True(response.IsSuccess);
        Assert.Equal("instock", parentStockStatus);
        Assert.Equal(variantTree.Values.Sum(x => x.Count), variantPosts!.Count);

        var variantDetails = GetVariantDetails(wpMetas, parentPost, variantPosts, skus);
        AssertVariantDetails("dummy post title - XS/S", "dummy-post-title-xs-s", 38, "instock", "xs-s", 123, "5908214227099", variantDetails);
        AssertVariantDetails("dummy post title - M/L", "dummy-post-title-m-l", 3, "instock", "m-l", 123, "5908214227082", variantDetails);
        AssertVariantDetails("dummy post title - XL/XXL", "dummy-post-title-xl-xxl", 11, "instock", "xl-xxl", 123, "5908214231799", variantDetails);
        AssertVariantDetails("dummy post title - 3XL/4XL", "dummy-post-title-3xl-4xl", 2, "instock", "3xl-4xl", 123, "590821423180", variantDetails);
        AssertVariantDetails("dummy post title - 5XL/6XL", "dummy-post-title-5xl-6xl", 8, "instock", "5xl-6xl", 123, "5908214231812", variantDetails);
    }

    private static void AssertVariantDetails(
        string postTitle, string postName, int stock, string stockStatus, string attributeRozmiar, decimal price, string variantSku,
        VariantDetailsAssertion variantDetails)
    {
        Assert.Equal(postTitle, variantDetails.VariantDetails[variantSku].PostTitle);
        Assert.Equal(postName, variantDetails.VariantDetails[variantSku].PostName);
        Assert.Equal(stock, variantDetails.VariantDetails[variantSku].Stock);
        Assert.Equal(stockStatus, variantDetails.VariantDetails[variantSku].StockStatus);
        Assert.Equal(attributeRozmiar, variantDetails.VariantDetails[variantSku].AttributeRozmiar);
        Assert.Equal(price, variantDetails.VariantDetails[variantSku].Price);
    }

    private static VariantDetailsAssertion GetVariantDetails(
        List<WpPostmetum>? wpMetas, WpPost? parentPost, List<WpPost>? variantPosts, Dictionary<ulong, string>? skus)
    {
        var variantDetails = new VariantDetailsAssertion();

        foreach (var sku in skus!.Where(x => x.Key != parentPost!.Id))
        {
            var variantPost = variantPosts?.First(x => x.Id == sku.Key);
            var postTitle = variantPost!.PostTitle;
            var postName = variantPost!.PostName;
            var variantMeta = wpMetas?.Where(x => x.PostId == sku.Key).ToList();
            var stock = int.Parse(variantMeta!.FirstOrDefault(x => x.MetaKey == MetaKeyConstans.Stock)!.MetaValue);
            var variantStockStatus = variantMeta!.FirstOrDefault(x => x.MetaKey == MetaKeyConstans.StockStatus)!.MetaValue;
            var attributeRozmiar = variantMeta!.FirstOrDefault(x => x.MetaKey == MetaKeyConstans.AttributePaRozmiar)!.MetaValue;
            var variantPrice = decimal.Parse(variantMeta!.FirstOrDefault(x => x.MetaKey == MetaKeyConstans.Price)!.MetaValue);
            variantDetails.VariantDetails.Add(sku.Value, new(postTitle, postName, stock, variantStockStatus, attributeRozmiar, variantPrice));
        }

        return variantDetails;
    }
}

internal record VariantDetailsAssertion
{
    public Dictionary<string, VariantDetailsParameters> VariantDetails { get; set; } = new Dictionary<string, VariantDetailsParameters>();
    internal record VariantDetailsParameters(string PostTitle, string PostName, int Stock, string StockStatus, string AttributeRozmiar, decimal Price);
}
