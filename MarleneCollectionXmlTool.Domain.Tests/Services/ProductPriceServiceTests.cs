using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Models;
using MarleneCollectionXmlTool.Domain.Helpers;
using MarleneCollectionXmlTool.Domain.Services;
using MarleneCollectionXmlTool.Domain.Tests.Utils;
using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Services;

public class ProductPriceServiceTests
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly ProductPriceService _sut;

    public ProductPriceServiceTests()
    {
        _dbContext = FakeDbContextFactory.CreateMockDbContext<WoocommerceDbContext>();
        _sut = new ProductPriceService(_dbContext, new ProductPromoPriceValueProvider());
    }

    [Theory]
    [InlineData("120.00", "120.00", null)]
    [InlineData("84.50", "199.00", "84.50")]
    public async Task WholesalerProductInOnSale_SyncProductPricesToMatchWholesaler(string currentPrice, string currentRegularPrice, string currentSalesPrice)
    {
        //Arrange
        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile("D20-ZIELON-Promo");
        var xmlProducts = xmlDocument.GetElementsByTagName(HurtIvonXmlConstrains.Produkt);
        
        var originalWpPostsWithMetas = CreateMockDatabaseObjects(currentPrice, currentRegularPrice, currentSalesPrice);
        var parentProducts = originalWpPostsWithMetas.Where(x => x.WpPost.PostType == WpPostConstrains.Product).Select(x => x.WpPost).ToList();
        var variantProducts = originalWpPostsWithMetas.Where(x => x.WpPost.PostType == WpPostConstrains.ProductVariation).Select(x => x.WpPost).ToList();
        var productMetaDetails = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        var metaLookups = originalWpPostsWithMetas.Select(x => x.ProductMetaLookup).ToList();

        await _dbContext.AddRangeAsync(parentProducts);
        await _dbContext.AddRangeAsync(variantProducts);
        await _dbContext.AddRangeAsync(productMetaDetails);
        await _dbContext.AddRangeAsync(metaLookups);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = await _sut.UpdateProductPrices(parentProducts, variantProducts, productMetaDetails, xmlProducts);
        await _dbContext.SaveChangesAsync();

        //Assert
        Assert.Equal(originalWpPostsWithMetas.Count, result);

        var priceValues = await _dbContext.WpPostmeta
            .Where(x => variantProducts.Select(x => x.Id).Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstrains.Price)
            .ToDictionaryAsync(x => x.PostId, x => decimal.Parse(x.MetaValue));

        var regularPriceValues = await _dbContext.WpPostmeta
            .Where(x => variantProducts.Select(x => x.Id).Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstrains.RegularPrice)
            .ToDictionaryAsync(x => x.PostId, x => decimal.Parse(x.MetaValue));

        var salePriceValues = await _dbContext.WpPostmeta
            .Where(x => variantProducts.Select(x => x.Id).Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstrains.SalePrice)
            .ToDictionaryAsync(x => x.PostId, x => decimal.Parse(x.MetaValue));

        Assert.True(priceValues.All(x => x.Value == 84.50m));
        Assert.True(salePriceValues.All(x => x.Value == 84.50m));
        Assert.True(regularPriceValues.All(x => x.Value == 199.00m));

        var minPriceLookup = await _dbContext.WpWcProductMetaLookups
            .Where(x => variantProducts.Select(x => (long)x.Id).Contains(x.ProductId))
            .ToDictionaryAsync(x => x.ProductId, x => x.MinPrice);

        var maxPriceLookup = await _dbContext.WpWcProductMetaLookups
            .Where(x => variantProducts.Select(x => (long)x.Id).Contains(x.ProductId))
            .ToDictionaryAsync(x => x.ProductId, x => x.MaxPrice);

        Assert.True(minPriceLookup.All(x => x.Value == 84.50m));
        Assert.True(maxPriceLookup.All(x => x.Value == 199.00m));
    }

    [Theory]
    [InlineData("84.50", "199.00", "84.50")]
    public async Task WholesalerProductInNoMoreOnSale_SyncProductPricesToMatchWholesaler(string currentPrice, string currentRegularPrice, string currentSalesPrice)
    {
        //Arrange
        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile("D20-ZIELON");
        var xmlProducts = xmlDocument.GetElementsByTagName(HurtIvonXmlConstrains.Produkt);

        var originalWpPostsWithMetas = CreateMockDatabaseObjects(currentPrice, currentRegularPrice, currentSalesPrice);
        var parentProducts = originalWpPostsWithMetas.Where(x => x.WpPost.PostType == WpPostConstrains.Product).Select(x => x.WpPost).ToList();
        var variantProducts = originalWpPostsWithMetas.Where(x => x.WpPost.PostType == WpPostConstrains.ProductVariation).Select(x => x.WpPost).ToList();
        var productMetaDetails = originalWpPostsWithMetas.SelectMany(x => x.WpPostmetum).ToList();
        var metaLookups = originalWpPostsWithMetas.Select(x => x.ProductMetaLookup).ToList();

        await _dbContext.AddRangeAsync(parentProducts);
        await _dbContext.AddRangeAsync(variantProducts);
        await _dbContext.AddRangeAsync(productMetaDetails);
        await _dbContext.AddRangeAsync(metaLookups);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = await _sut.UpdateProductPrices(parentProducts, variantProducts, productMetaDetails, xmlProducts);
        await _dbContext.SaveChangesAsync();

        //Assert
        Assert.Equal(originalWpPostsWithMetas.Count, result);

        var priceValues = await _dbContext.WpPostmeta
            .Where(x => variantProducts.Select(x => x.Id).Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstrains.Price)
            .ToDictionaryAsync(x => x.PostId, x => decimal.Parse(x.MetaValue));

        var regularPriceValues = await _dbContext.WpPostmeta
            .Where(x => variantProducts.Select(x => x.Id).Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstrains.RegularPrice)
            .ToDictionaryAsync(x => x.PostId, x => decimal.Parse(x.MetaValue));

        var salePriceValues = await _dbContext.WpPostmeta
            .Where(x => variantProducts.Select(x => x.Id).Contains(x.PostId))
            .Where(x => x.MetaKey == MetaKeyConstrains.SalePrice)
            .ToDictionaryAsync(x => x.PostId, x => string.IsNullOrEmpty(x.MetaValue) ? (decimal?)null : decimal.Parse(x.MetaValue!));

        Assert.True(priceValues.All(x => x.Value == 199.00m));
        Assert.False(salePriceValues.All(x => x.Value == null));
        Assert.True(regularPriceValues.All(x => x.Value == 199.00m));

        var minPriceLookup = await _dbContext.WpWcProductMetaLookups
            .Where(x => variantProducts.Select(x => (long)x.Id).Contains(x.ProductId))
            .ToDictionaryAsync(x => x.ProductId, x => x.MinPrice);

        var maxPriceLookup = await _dbContext.WpWcProductMetaLookups
            .Where(x => variantProducts.Select(x => (long)x.Id).Contains(x.ProductId))
            .ToDictionaryAsync(x => x.ProductId, x => x.MaxPrice);

        Assert.True(minPriceLookup.All(x => x.Value == 199.00m));
        Assert.True(maxPriceLookup.All(x => x.Value == 199.00m));
    }

    private static List<MockDataHelper.WpPostWithMeta> CreateMockDatabaseObjects(string price, string regularPrice, string salesPrice)
    {
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
