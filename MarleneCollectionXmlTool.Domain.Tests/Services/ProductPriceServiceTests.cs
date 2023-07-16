using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.Domain.Services;
using MarleneCollectionXmlTool.Domain.Tests.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Services;

public class ProductPriceServiceTests
{
    private readonly WoocommerceDbContext _dbContext;
    private readonly ProductPriceService _sut;

    public ProductPriceServiceTests()
    {
        _dbContext = FakeDbContextFactory.CreateMockDbContext<WoocommerceDbContext>();
        _sut = new ProductPriceService(_dbContext);
    }

    [Fact]
    public void GetNewProductPrice_AddNewPromoPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = 24.5m;
        var currentPrice = 49m;
        var currentPromoPrice = (decimal?)null;
        var expectedCatalogPrice = 49m;
        var expectedPromoPrice = 24.5m;

        var expectedResult = (expectedCatalogPrice, expectedPromoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNewProductPrice_UpdatePromoPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = 24.5m;
        var currentPrice = 39m;
        var currentPromoPrice = (decimal?)null;
        var expectedCatalogPrice = 49m;
        var expectedPromoPrice = 24.5m;

        var expectedResult = (expectedCatalogPrice, expectedPromoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNewProductPrice_DoNotUpdateAnyPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = (decimal?)null;
        var currentPrice = 49m;
        var currentPromoPrice = (decimal?)null;
        var expectedCatalogPrice = 49m;
        var expectedPromoPrice = (decimal?)null;

        var expectedResult = (expectedCatalogPrice, expectedPromoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNewProductPrice_NoMorePromoPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = (decimal?)null;
        var currentPrice = 49m;
        var currentPromoPrice = 24.5m;
        var expectedCatalogPrice = 49m;
        var expectedPromoPrice = (decimal?)null;

        var expectedResult = (expectedCatalogPrice, expectedPromoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNewProductPrice_UpdateRegularPrice()
    {
        //Arrange
        var catalogPrice = 49m;
        var promoPrice = (decimal?)null;
        var currentPrice = 39m;
        var currentPromoPrice = (decimal?)null;
        var expectedCatalogPrice = 49m;
        var expectedPromoPrice = (decimal?)null;

        var expectedResult = (expectedCatalogPrice, expectedPromoPrice);

        //Act
        var result = _sut.GetNewProductPrice(catalogPrice, promoPrice, currentPrice, currentPromoPrice);

        //Assert
        Assert.Equal(expectedResult, result);
    }
}
