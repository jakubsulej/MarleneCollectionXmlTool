using Microsoft.Extensions.Configuration;

namespace MarleneCollectionXmlTool.Domain.Helpers.Providers;

public interface IConfigurationArrayProvider
{
    List<string> GetCategoriesToSkip();
    List<string> GetNotUpdatableSkus();
}

public class ConfigurationArrayProvider : IConfigurationArrayProvider
{
    private readonly IConfiguration _configuration;
    private const string CategoriesToSkip = "CategoriesToSkip";
    private const string NotUpdatableSkus = "NotUpdatableSkus";

    public ConfigurationArrayProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<string> GetCategoriesToSkip() => GetAllConfigurationValuesAsList(CategoriesToSkip);
    public List<string> GetNotUpdatableSkus() => GetAllConfigurationValuesAsList(NotUpdatableSkus);

    public List<string> GetAllConfigurationValuesAsList(string configurationName)
    {
        var categories = new List<string>();
        var index = 0;

        while (true)
        {
            var category = _configuration.GetValue<string>($"{configurationName}:{index}");
            if (string.IsNullOrEmpty(category))
                break;

            categories.Add(category);
            index++;
        }

        return categories;
    }
}
