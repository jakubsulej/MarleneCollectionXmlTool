using MarleneCollectionXmlTool.DBAccessLayer;
using MarleneCollectionXmlTool.DBAccessLayer.Cache;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using MarleneCollectionXmlTool.Domain.Services.ClientSevices;
using MarleneCollectionXmlTool.Domain.Services.ProductUpdaters;
using MarleneCollectionXmlTool.Domain.Utils;
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

        var baseUrl = configuration.GetValue<string>(ConfigurationKeyConstans.HurtIvonBaseUrl);
        services.AddScoped<IGetXmlDocumentFromWholesalerService>(sp => new GetXmlDocumentFromWholesalerService(new Uri(baseUrl), configuration));

        var priceMarginFactor = configuration.GetValue<decimal>(ConfigurationKeyConstans.PriceMarginFactor, 1);
        var priceMarginStatic = configuration.GetValue<decimal>(ConfigurationKeyConstans.PriceMarginStatic, 0);
        services.AddSingleton<IProductPromoPriceValueProvider>(sp => new ProductPromoPriceValueProvider(priceMarginFactor, priceMarginStatic));

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

        var baseUrl = Environment.GetEnvironmentVariable(ConfigurationKeyConstans.HurtIvonBaseUrl);
        services.AddScoped<IGetXmlDocumentFromWholesalerService>(sp => new GetXmlDocumentFromWholesalerService(new Uri(baseUrl)));

        var priceMarginFactor = EnvironmentVariableValueProvider.GetEnvironmentVariableOrDefault(ConfigurationKeyConstans.PriceMarginFactor, 1);
        var priceMarginStatic = EnvironmentVariableValueProvider.GetEnvironmentVariableOrDefault(ConfigurationKeyConstans.PriceMarginStatic, 0);
        services.AddSingleton<IProductPromoPriceValueProvider>(sp => new ProductPromoPriceValueProvider(priceMarginFactor, priceMarginStatic));

        services.AddCommonServices();

        return services;
    }

    private static void AddCommonServices(this IServiceCollection services)
    {
        //MediatR services
        var thisAssemblyMarker = typeof(ServiceCollectionExtensions).Assembly;
        services.AddMediatR(thisAssemblyMarker);

        //Business services
        services.AddSingleton<IConfigurationArrayProvider, ConfigurationArrayProvider>();
        services.AddScoped<IProductAttributeService, ProductAttributeService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IWoocommerceRestApiService, WoocommerceRestApiService>();
        services.AddScoped<IProductPriceService, ProductPriceService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<ISyncWoocommerceProductsWithWholesalerService, SyncWoocommerceProductsWithWholesalerService>();
        services.AddScoped<IProductStockStatusService, ProductStockStatusService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<IProductDeleteService, ProductDeleteService>();

        //Cache services
        services.AddScoped<ICacheProvider, CacheProvider>();
        services.AddScoped<ICacheDataRepository, CacheDataRepository>();
        services.AddScoped<IMemoryCache, MemoryCache>();
    }

    private static string GetMySqlConnectionString(this IConfiguration configuration)
    {
        var mySqlBuilder = new MySqlConnectionStringBuilder
        {
            Server = configuration.GetValue<string>(ConfigurationKeyConstans.DB_HOST),
            Port = configuration.GetValue<uint>(ConfigurationKeyConstans.DB_PORT),
            UserID = configuration.GetValue<string>(ConfigurationKeyConstans.DB_USER),
            Password = configuration.GetValue<string>(ConfigurationKeyConstans.DB_PASSWORD),
            Database = configuration.GetValue<string>(ConfigurationKeyConstans.DB_NAME),
            AllowZeroDateTime = true,
            ConvertZeroDateTime = true,
        };

        return mySqlBuilder.ConnectionString;
    }

    private static string GetMySqlConnectionStringFromEnvironment()
    {
        var mySqlBuilder = new MySqlConnectionStringBuilder
        {
            Server = Environment.GetEnvironmentVariable(ConfigurationKeyConstans.DB_HOST),
            Port = uint.Parse(Environment.GetEnvironmentVariable(ConfigurationKeyConstans.DB_PORT)),
            UserID = Environment.GetEnvironmentVariable(ConfigurationKeyConstans.DB_USER),
            Password = Environment.GetEnvironmentVariable(ConfigurationKeyConstans.DB_PASSWORD),
            Database = Environment.GetEnvironmentVariable(ConfigurationKeyConstans.DB_NAME),
            AllowZeroDateTime = true,
            ConvertZeroDateTime = true,
        };

        return mySqlBuilder.ConnectionString;
    }
}
