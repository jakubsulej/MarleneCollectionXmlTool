using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Utils;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Helpers;

public class ProductPromoPriceProviderTests
{
    [Fact]
    public void CurrentProductIsNotOnSale_AddNewPromoPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = 24.5m;
        var currentPrice = 49m;
        var currentPromoPrice = (decimal?)null;
        var expectedResult = new ProductPriceDto(catalogPrice, promoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(1, 0).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CurrentProductPriceIsLowerThanInCatalog_UpdateCatalogPriceOnly()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = 24.5m;
        var currentPrice = 39m;
        var currentPromoPrice = (decimal?)null;
        var expectedResult = new ProductPriceDto(catalogPrice, promoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(1, 0).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CurrentProductPricesAreSameAsInCatalog_DoNotUpdateAnyPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = (decimal?)null;
        var currentPrice = 49m;
        var currentPromoPrice = (decimal?)null;
        var expectedResult = new ProductPriceDto(catalogPrice, promoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(1, 0).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CurrentProductIsOnSaleButInCatalogItIsNot_NoMorePromoPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = (decimal?)null;
        var currentPrice = 49m;
        var currentPromoPrice = 24.5m;
        var expectedResult = new ProductPriceDto(catalogPrice, promoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(1, 0).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CurrentProductPriceIsLowerThanCatalogPrice_UpdateOnlyRegularPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = (decimal?)null;
        var currentPrice = 39m;
        var currentPromoPrice = (decimal?)null;
        var expectedResult = new ProductPriceDto(catalogPrice, promoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(1, 0).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CurrentPromoPriceIsLowerThanInCatalog_IncreaseOnlyPromoPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = 30m;
        var currentPrice = 49m;
        var currentPromoPrice = 24.5m;
        var expectedResult = new ProductPriceDto(catalogPrice, promoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(1, 0).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CurrentPromoPriceIsHigherThanInCatalog_DecreaseOnlyPromoPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = 24.5m;
        var currentPrice = 49m;
        var currentPromoPrice = 30m;
        var expectedResult = new ProductPriceDto(catalogPrice, promoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(1, 0).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CurrentProductPriceIsHigherThanInCatalog_DecreaseOnlyProductPrice()
    {
        //Arrange
        var catalogPrice = 199m;
        var promoPrice = (decimal?)null;
        var currentPrice = 350m;
        var currentPromoPrice = (decimal?)null;
        var expectedResult = new ProductPriceDto(catalogPrice, promoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(1, 0).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1.2, 14)]
    [InlineData(1, 22)]
    public void GetNewProductPrice_ShouldAddStaticMarginToCurrentPrice(decimal factorMargin, decimal staticMargin)
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = 24.5m;
        var currentPrice = 49m;
        var currentPromoPrice = 30m;

        var expectedCatalogPrice = (catalogPrice * factorMargin) + staticMargin;
        var expectedPromoPrice = (promoPrice * factorMargin) + staticMargin;
        var expectedResult = new ProductPriceDto(expectedCatalogPrice, expectedPromoPrice);

        //Act
        var result = new ProductPromoPriceValueProvider(factorMargin, staticMargin).GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }
}
