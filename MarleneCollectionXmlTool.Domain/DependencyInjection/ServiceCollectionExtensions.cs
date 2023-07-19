using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Services.ClientSevices;
using MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace MarleneCollectionXmlTool.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetMySqlConnectionString();

        services.AddDbContext<WoocommerceDbContext>(options =>
        {
            options.UseMySQL(connectionString);
        });

        var baseUrl = configuration.GetValue<string>("HurtIvonBaseUrl");
        services.AddScoped<IGetXmlDocumentFromWholesalerService>(sp => new GetXmlDocumentFromWholesalerService(new Uri(baseUrl), configuration));

        services.AddCommonServices();

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        var connectionString = GetMySqlConnectionStringFromEnvironment();

        services.AddDbContext<WoocommerceDbContext>(options =>
        {
            options.UseMySQL(connectionString);
        });

        var baseUrl = Environment.GetEnvironmentVariable("HurtIvonBaseUrl");
        services.AddScoped<IGetXmlDocumentFromWholesalerService>(sp => new GetXmlDocumentFromWholesalerService(new Uri(baseUrl)));
        
        services.AddCommonServices();

        return services;
    }

    private static void AddCommonServices(this IServiceCollection services)
    {
        var thisAssemblyMarker = typeof(ServiceCollectionExtensions).Assembly;
        services.AddMediatR(thisAssemblyMarker);

        //Business services
        services.AddScoped<IProductAttributeService, ProductAttributeService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IWoocommerceRestApiService, WoocommerceRestApiService>();
        services.AddSingleton<IConfigurationArrayProvider, ConfigurationArrayProvider>();
        services.AddScoped<IUpdateProductPriceService, UpdateProductPriceService>();
        services.AddSingleton<IProductPromoPriceValueProvider, ProductPromoPriceValueProvider>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();

        //Cache services
        services.AddScoped<ICacheProvider, CacheProvider>();
        services.AddScoped<ICacheDataRepository, CacheDataRepository>();
        services.AddScoped<IMemoryCache, MemoryCache>();
    }

    private static string GetMySqlConnectionString(this IConfiguration configuration)
    {
        var mySqlBuilder = new MySqlConnectionStringBuilder
        {
            Server = configuration.GetValue<string>("DB_HOST"),
            Port = configuration.GetValue<uint>("DB_PORT"),
            UserID = configuration.GetValue<string>("DB_USER"),
            Password = configuration.GetValue<string>("DB_PASSWORD"),
            Database = configuration.GetValue<string>("DB_NAME"),
            AllowZeroDateTime = true,
            ConvertZeroDateTime = true,
        };

        return mySqlBuilder.ConnectionString;
    }

    private static string GetMySqlConnectionStringFromEnvironment()
    {
        var mySqlBuilder = new MySqlConnectionStringBuilder
        {
            Server = Environment.GetEnvironmentVariable("DB_HOST"),
            Port = uint.Parse(Environment.GetEnvironmentVariable("DB_PORT")),
            UserID = Environment.GetEnvironmentVariable("DB_USER"),
            Password = Environment.GetEnvironmentVariable("DB_PASSWORD"),
            Database = Environment.GetEnvironmentVariable("DB_NAME"),
            AllowZeroDateTime = true,
            ConvertZeroDateTime = true,
        };

        return mySqlBuilder.ConnectionString;
    }
}
