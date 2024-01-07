using MarleneCollectionXmlTool.Domain.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace MarleneCollectionXmlTool.AzureFunctions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults();

        builder.ConfigureServices(services =>
        {
            var isDevelopment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development";
            
            if (isDevelopment)
            {
                var configuration = new ConfigurationBuilder().Build();
                configuration = GetAzureFunctionConfiguration("local.settings.json");
                SetEnvironmentVariable(configuration, "DB_NAME");
                SetEnvironmentVariable(configuration, "DB_USER");
                SetEnvironmentVariable(configuration, "DB_PASSWORD");
                SetEnvironmentVariable(configuration, "DB_HOST");
                SetEnvironmentVariable(configuration, "DB_PORT");
                SetEnvironmentVariable(configuration, "BaseClientUrl");
                SetEnvironmentVariable(configuration, "BaseUrl");
                SetEnvironmentVariable(configuration, "WoocommerceXmlUrl");
                SetEnvironmentVariable(configuration, "NestedVariantsXmlUrl");
                SetEnvironmentVariable(configuration, "PriceMarginFactor");
                SetEnvironmentVariable(configuration, "PriceMarginStatic");
            }

            services.AddDomainServices();
        });

        var host = builder.Build();
        host.Run();
    }

    private static void SetEnvironmentVariable(IConfiguration configs, string envName)
    {
        Environment.SetEnvironmentVariable(envName, configs.GetValue<string>($"Values:{envName}"));
    }

    private static IConfigurationRoot GetAzureFunctionConfiguration(string name)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile(name, true, true);
        var config = builder.Build();

        return config;
    }
}
