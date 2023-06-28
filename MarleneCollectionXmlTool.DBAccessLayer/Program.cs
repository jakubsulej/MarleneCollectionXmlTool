using Microsoft.Extensions.Configuration;

namespace MarleneCollectionXmlTool.DBAccessLayer;

public static class Program
{
    public static void Main(string[] args)
    {
        string projectDirectory = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName}\\BookAService\\Server";

        var builder = new ConfigurationBuilder()
            .SetBasePath(projectDirectory)
            .AddJsonFile("appsettings.json", optional: false);

        builder.Build();
    }
}
