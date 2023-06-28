using FluentResults;
using MarleneCollectionXmlTool.Domain.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Queries.GetProductAttributes;

public class GetProductAttributesRequestHandler : IRequestHandler<GetProductAttributesRequest, Result<GetProductAttributesResponse>>
{
    private readonly IConfiguration _configuration;
    private XmlDocument _xmlDoc;
    private readonly HttpClient _httpClient;

    public GetProductAttributesRequestHandler(IConfiguration configuration)
    {
        _configuration = configuration;
        _xmlDoc = new XmlDocument();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_configuration.GetValue<string>("BaseUrl"))
        };
    }

    public async Task<Result<GetProductAttributesResponse>> Handle(GetProductAttributesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var xmlUrl = _configuration.GetValue<string>("WoocommerceXmlUrl");
            var response = await _httpClient.GetAsync(xmlUrl, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            var result = new GetProductAttributesResponse();

            var colorAttributes = AttributeValuesXmlFileHelper.GetAttriubuteValuesFromXmlFile(responseContent, "kolor");
            result.Attributes.Add(colorAttributes);

            var lenghtAttributes = AttributeValuesXmlFileHelper.GetAttriubuteValuesFromXmlFile(responseContent, "dlugosc");
            result.Attributes.Add(lenghtAttributes);

            var sizeAttributes = AttributeValuesXmlFileHelper.GetAttriubuteValuesFromXmlFile(responseContent, "rozmiar");
            result.Attributes.Add(sizeAttributes);

            var patternAttributes = AttributeValuesXmlFileHelper.GetAttriubuteValuesFromXmlFile(responseContent, "wzor");
            result.Attributes.Add(patternAttributes);

            var shapeAttributes = AttributeValuesXmlFileHelper.GetAttriubuteValuesFromXmlFile(responseContent, "fason");
            result.Attributes.Add(shapeAttributes);

            return Result.Ok(result);
        }
        catch
        {
            return Result.Fail("Error");
        }
    }
}
