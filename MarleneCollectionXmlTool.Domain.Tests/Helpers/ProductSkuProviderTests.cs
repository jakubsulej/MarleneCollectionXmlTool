using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Utils;
using System.Xml;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Helpers;

public class ProductSkuProviderTests
{
    [Theory]
    [InlineData("", "5908214230822")]
    [InlineData(" ", "5908214230822")]
    [InlineData("1328-S/M", "5908214230822")]
    public void GetVariantProductSku_ShouldReturnEanInPlaceOfSku_GivenCorrectEanInXmlFile(string variantProductId, string variantProductEan)
    {
        //Arrange
        var document = new XmlDocument();
        document.LoadXml($@"
            <wariant>
                <{HurtIvonXmlConstans.Id}>{variantProductId}</{HurtIvonXmlConstans.Id}>
                <{HurtIvonXmlConstans.Ean}>{variantProductEan}</{HurtIvonXmlConstans.Ean}>
                <{HurtIvonXmlConstans.Rozmiar}>S/M</{HurtIvonXmlConstans.Rozmiar}>
                <{HurtIvonXmlConstans.DostepnaIlosc}>5</{HurtIvonXmlConstans.DostepnaIlosc}>
            </wariant>
        ");

        var variantChildNodes = document.ChildNodes[0]!.ChildNodes;

        //Act
        var result = ProductSkuProvider.GetVariantProductSku(variantChildNodes, variantProductEan);

        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(variantProductEan, result);
    }

    [Theory]
    [InlineData("1328-S/M", "")]
    [InlineData("1328-S/M", " ")]
    [InlineData("1328-S/M", null)]
    public void GetVariantProductSku_ShouldReturnIdInPlaceOfSku_GivenCorrectIdsAndMissingEansInXmlFile(string variantProductId, string variantProductEan)
    {
        //Arrange
        var document = new XmlDocument();
        document.LoadXml($@"
            <wariant>
                <{HurtIvonXmlConstans.Id}>{variantProductId}</{HurtIvonXmlConstans.Id}>
                <{HurtIvonXmlConstans.Ean}>{variantProductEan}</{HurtIvonXmlConstans.Ean}>
                <{HurtIvonXmlConstans.Rozmiar}>S/M</{HurtIvonXmlConstans.Rozmiar}>
                <{HurtIvonXmlConstans.DostepnaIlosc}>5</{HurtIvonXmlConstans.DostepnaIlosc}>
            </wariant>
        ");

        var variantChildNodes = document.ChildNodes[0]!.ChildNodes;

        //Act
        var result = ProductSkuProvider.GetVariantProductSku(variantChildNodes, variantProductEan);

        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(variantProductId, result);
    }
}
