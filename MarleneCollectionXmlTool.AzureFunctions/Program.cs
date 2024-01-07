using MarleneCollectionXmlTool.Domain.DependencyInjection;
using MarleneCollectionXmlTool.Domain.Utils;
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
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.DB_NAME);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.DB_USER);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.DB_PASSWORD);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.DB_HOST);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.DB_PORT);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.BaseClientUrl);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.BaseUrl);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.WoocommerceXmlUrl);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.NestedVariantsXmlUrl);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.PriceMarginFactor);
                SetEnvironmentVariable(configuration, ConfigurationKeyConstans.PriceMarginStatic);
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
