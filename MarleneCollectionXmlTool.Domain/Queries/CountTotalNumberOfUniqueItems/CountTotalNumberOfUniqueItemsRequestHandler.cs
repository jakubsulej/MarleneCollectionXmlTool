using FluentResults;
using MarleneCollectionXmlTool.Domain.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace MarleneCollectionXmlTool.Domain.Queries.CountTotalNumberOfUniqueItems;

public class CountTotalNumberOfUniqueItemsRequestHandler : IRequestHandler<CountTotalNumberOfUniqueItemsRequest, Result<CountTotalNumberOfUniqueItemsResponse>>
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public CountTotalNumberOfUniqueItemsRequestHandler(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_configuration.GetValue<string>("BaseUrl"))
        };
    }

    public async Task<Result<CountTotalNumberOfUniqueItemsResponse>> Handle(CountTotalNumberOfUniqueItemsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var xmlUrl = _configuration.GetValue<string>("WoocommerceXmlUrl");
            var response = await _httpClient.GetAsync(xmlUrl, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            var categoriesRaw = AttributeValuesXmlFileHelper.GetAttriubuteValuesFromXmlFile(responseContent, "category");
            var categories = categoriesRaw.AttributeValues.GroupBy(x => x).SelectMany(x => x).ToList();

            var colorAttributes = AttributeValuesXmlFileHelper.GetAttriubuteValuesFromXmlFile(responseContent, "id");
            var numberOfItems = colorAttributes.AttributeValues.GroupBy(x => x).Count();

            var result = new CountTotalNumberOfUniqueItemsResponse(numberOfItems);
            return Result.Ok(result);
        }
        catch
        {
            return Result.Fail("Error");
        }
    }
}
