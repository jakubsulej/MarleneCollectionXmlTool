using MarleneCollectionXmlTool.Domain.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MarleneCollectionXmlTool;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var configuration = host.Services.GetRequiredService<IConfiguration>();
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureSharedSettingsAndUserSecrets();
}