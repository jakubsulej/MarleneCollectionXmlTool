using MarleneCollectionXmlTool.Domain.Services;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Services;

public class CustomAttributeValueMappingServiceTests
{
    private readonly ICustomAttributeValueMappingService _sut;

    public CustomAttributeValueMappingServiceTests()
    {
        _sut = new CustomAttributeValueMappingService();
    }

    [Theory]
    [InlineData("rozmiar", "U", "Uniwersalny")]
    [InlineData("kolor", "ŚMIETANKOWY", "Ecru")]
    public void GetCustomAttributeMappingValue_SuccessfulTest(string attributeKey, string attributeValue, string expected)
    {
        //Act
        var result = _sut.GetCustomAttributeMappingValue(attributeKey, attributeValue);

        //Assert
        Assert.Equal(expected, result);
    }
}
