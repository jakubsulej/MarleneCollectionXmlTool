using MarleneCollectionXmlTool.Domain.Utils;
using Microsoft.Extensions.Configuration;
using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Services.ClientSevices;

public interface IGetXmlDocumentFromWholesalerService
{
    Task<XmlDocument> GetXmlDocumentForWoocommerceXmlUrl(CancellationToken cancellationToken);
    Task<XmlDocument> GetXmlDocumentNestedVariantsXmlUrl(CancellationToken cancellationToken);
}

public class GetXmlDocumentFromWholesalerService : IGetXmlDocumentFromWholesalerService
{
    private readonly string _woocommerceXmlUrl;
    private readonly string _nestedVariantsXmlUrl;
    private readonly HttpClient _httpClient;

    public GetXmlDocumentFromWholesalerService(Uri baseAddress, IConfiguration configuration)
    {
        _woocommerceXmlUrl = configuration.GetValue<string>(ConfigurationKeyConstans.WoocommerceXmlUrl);
        _nestedVariantsXmlUrl = configuration.GetValue<string>(ConfigurationKeyConstans.NestedVariantsXmlUrl);
        _httpClient = new HttpClient { BaseAddress = baseAddress };
    }

    public GetXmlDocumentFromWholesalerService(Uri baseAddress)
    {
        _woocommerceXmlUrl = Environment.GetEnvironmentVariable(ConfigurationKeyConstans.WoocommerceXmlUrl);
        _nestedVariantsXmlUrl = Environment.GetEnvironmentVariable(ConfigurationKeyConstans.NestedVariantsXmlUrl);
        _httpClient = new HttpClient { BaseAddress = baseAddress };
    }

    public async Task<XmlDocument> GetXmlDocumentForWoocommerceXmlUrl(CancellationToken cancellationToken)
    {
        var xmlUrl = _woocommerceXmlUrl;
        var xmlDoc = await GetXmlDocumentFromXmlUrl(xmlUrl, cancellationToken);
        return xmlDoc;
    }

    public async Task<XmlDocument> GetXmlDocumentNestedVariantsXmlUrl(CancellationToken cancellationToken)
    {
        var xmlUrl = _nestedVariantsXmlUrl;
        var xmlDoc = await GetXmlDocumentFromXmlUrl(xmlUrl, cancellationToken);
        return xmlDoc;
    }

    private async Task<XmlDocument> GetXmlDocumentFromXmlUrl(string xmlUrl, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(xmlUrl, cancellationToken);

        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(responseContent);

        return xmlDoc;
    }
}
