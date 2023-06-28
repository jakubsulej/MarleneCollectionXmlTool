using FluentResults;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Queries.GetAllProductConfigurations;

public class GetAllProductConfigurationsRequestHandler : IRequestHandler<GetAllProductConfigurationsRequest, Result<GetAllProductConfigurationsResponse>>
{
    private readonly IConfiguration _configuration;
    private XmlDocument _xmlDoc;
    private readonly HttpClient _httpClient;

    public GetAllProductConfigurationsRequestHandler(IConfiguration configuration)
    {
        _configuration = configuration;
        _xmlDoc = new XmlDocument();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_configuration.GetValue<string>("BaseUrl"))
        };
    }

    public async Task<Result<GetAllProductConfigurationsResponse>> Handle(GetAllProductConfigurationsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var xmlUrl = _configuration.GetValue<string>("WoocommerceXmlUrl");
            var response = await _httpClient.GetAsync(xmlUrl, cancellationToken);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseContent);

            var products = xmlDoc.GetElementsByTagName("produkt");

            var result = new GetAllProductConfigurationsResponse();

            foreach (XmlNode product in products)
            {
                string catalogCode = null;
                string colorVariant = null;
                string sizeVariant = null;
                string ean = null;
                string catalogPrice = null;
                var imageUrls = new List<string>();

                foreach (XmlNode child in product.ChildNodes)
                {
                    if (child.Name == "nazwa")
                        catalogCode = child.InnerText;

                    if (child.Name == "kolor")
                        colorVariant = child.InnerText;

                    if (child.Name == "rozmiar")
                        sizeVariant = child.InnerText;

                    if (child.Name == "ean")
                        ean = child.InnerText;

                    if (child.Name == "cena_katalogowa")
                        catalogPrice = child.InnerText;

                    if (child.Name == "zdjecia")
                    {
                        foreach (XmlNode photo in child.ChildNodes)
                        {
                            var photoUrl = photo.InnerText;
                            imageUrls.Add(photoUrl);
                        }
                    }
                }

                if (catalogCode == null)
                    continue;

                var variations = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(sizeVariant))
                    variations.Add("rozmiar", sizeVariant);

                if (!string.IsNullOrEmpty(colorVariant))
                    variations.Add("kolor", colorVariant);

                var configuration = new GetAllProductConfigurationsResponse.ProductConfiguration
                {
                    SKU = ean,
                    ImageUrls = imageUrls,
                    Price = catalogPrice,
                    ProductVariations = variations
                };

                if (result.AllProductConfigurations.ContainsKey(catalogCode) == false)
                {
                    var configurations = new List<GetAllProductConfigurationsResponse.ProductConfiguration> { configuration };

                    result.AllProductConfigurations.Add(catalogCode, configurations);
                }
                else
                {
                    var configurations = result.AllProductConfigurations[catalogCode];

                    //foreach (var existingVariations in configurations.Select(x => x.ProductVariations))
                    //{
                    //    foreach (var existingVariation in existingVariations)
                    //    {
                    //        var count = existingVariations.Where(x => x.Key == existingVariation.Key)?.Count();

                    //        if (existingVariations.All(x => x.Key == existingVariation.Key && x.Value == existingVariation.Value) && count > 1)
                    //            existingVariations.Remove(existingVariation.Key);
                    //    }
                    //}

                    configurations.Add(configuration);
                }
            }

            return Result.Ok(result);
        }
        catch
        {
            return Result.Fail("Error");
        }
    }
}
