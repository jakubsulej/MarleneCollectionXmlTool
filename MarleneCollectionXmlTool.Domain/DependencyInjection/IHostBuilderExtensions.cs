using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace MarleneCollectionXmlTool.Domain.DependencyInjection;

public static class IHostBuilderExtensions
{
    public static IHostBuilder ConfigureSharedSettingsAndUserSecrets(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENTIRONMENT") ?? "Development";

            config.ConfigureSharedSettings();

            var userSecretsId = Assembly.GetEntryAssembly()
                ?.GetCustomAttribute<UserSecretsIdAttribute>()
                ?.UserSecretsId;

            if (string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase) && userSecretsId != null)
            {
                config.AddUserSecrets(userSecretsId);
            }
        });
    }

    private static IConfigurationBuilder ConfigureSharedSettings(this IConfigurationBuilder builder)
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENTIRONMENT") ?? "Development";

        builder
            .AddJsonFile(Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.FullName, "sharedSettings.json"), optional: true)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings/{environmentName}.json", optional: true)
            .AddEnvironmentVariables();

        return builder;
    }
}
