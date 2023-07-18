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

    [Fact]
    public async Task Test()
    {
        //Arrange
        var xmlDocument = XmlTestHelper.GetXmlDocumentFromStaticFile("D20-ZIELON-Promo");
        var xmlProducts = xmlDocument.GetElementsByTagName(HurtIvonXmlConstrains.Produkt);

        var originalWpPostsWithMetas = MockDataHelper.GetFakeProductWithVariations();
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

        var priceValues = await _dbContext.WpPostmeta.Where(x => x.MetaKey == MetaKeyConstrains.Price).ToDictionaryAsync(x => x.PostId, x => decimal.Parse(x.MetaValue));
        var regularPriceValues = await _dbContext.WpPostmeta.Where(x => x.MetaKey == MetaKeyConstrains.RegularPrice).ToDictionaryAsync(x => x.PostId, x => decimal.Parse(x.MetaValue));
        var salePriceValues = await _dbContext.WpPostmeta.Where(x => x.MetaKey == MetaKeyConstrains.SalePrice).ToDictionaryAsync(x => x.PostId, x => decimal.Parse(x.MetaValue));


    }
}
