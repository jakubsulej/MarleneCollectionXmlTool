using FluentResults;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MarleneCollectionXmlTool.Domain.Services;

public interface IWoocommerceRestApiService
{
    Task<Result> UpdateProduct(ulong productId, string json);
}

public class WoocommerceRestApiService : IWoocommerceRestApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _consumerKey;
    private readonly string _consumerSecret;

    public WoocommerceRestApiService(IConfiguration configuration)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(configuration.GetValue<string>("BaseClientUrl")) };
        _consumerKey = configuration.GetValue<string>("WoocommerceConsumerKey");
        _consumerSecret = configuration.GetValue<string>("WoocommerceConsumerSecret");
    }

    public async Task<Result> UpdateProduct(ulong productId, string json)
    {
        var endpoint = $"/wp-json/wc/v3/products/{productId}";
        var response = await SendRequest(HttpMethod.Put, endpoint, json);

        if (response.IsSuccess)
            return Result.Ok();

        return Result.Fail("error");
    }

    private async Task<Result<HttpStatusCode>> SendRequest(HttpMethod method, string endpoint, string data = null)
    {
        try
        {
            var request = new HttpRequestMessage(method, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{_consumerKey}:{_consumerSecret}")));

            if (data != null)
            {
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return Result.Ok(response.StatusCode);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
