using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Helpers;

public class ProductPromoPriceProviderTests
{
    private readonly IProductPromoPriceValueProvider _sut;

    public ProductPromoPriceProviderTests()
    {
        _sut = new ProductPromoPriceValueProvider();
    }

    [Fact]
    public void CurrentProductIsNotOnSale_AddNewPromoPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = 24.5m;
        var currentPrice = 49m;
        var currentPromoPrice = (decimal?)null;
        var expectedResult = (catalogPrice, promoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

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
        var expectedResult = (catalogPrice, promoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

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
        var expectedResult = (catalogPrice, promoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

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
        var expectedResult = (catalogPrice, promoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

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
        var expectedResult = (catalogPrice, promoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

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
        var expectedResult = (catalogPrice, promoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

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
        var expectedResult = (catalogPrice, promoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }
}
