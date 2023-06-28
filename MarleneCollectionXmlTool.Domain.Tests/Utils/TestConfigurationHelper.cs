using Microsoft.Extensions.Configuration;

namespace MarleneCollectionXmlTool.Domain.Tests.Utils;

internal static class TestConfigurationHelper
{
    public static IConfiguration InitConfiguration()
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENTIRONMENT") ?? "Development";

        return new ConfigurationBuilder()
           .AddJsonFile(Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.FullName, "sharedSettings.json"), optional: true)
           .AddJsonFile("appsettings.json", optional: true)
           .AddJsonFile($"appsettings/{environmentName}.json", optional: true)
           .AddEnvironmentVariables()
           .Build();
    }
}
