using System.Text.Json;

namespace MarleneCollectionXmlTool.Customizations.Services;

public interface ICustomAttributeValueMappingService
{
    string GetCustomAttributeMappingValue(string attributeKey, string attributeValue);
}

public class CustomAttributeValueMappingService : ICustomAttributeValueMappingService
{
    private readonly IDictionary<string, Dictionary<string, string>> _dictionary;

    public CustomAttributeValueMappingService()
    {
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)
            .Parent.Parent.Parent.FullName;

        var json = File.ReadAllText(
            Path.Combine(projectDirectory,
            "MarleneCollectionXmlTool.Customizations",
            "attributesCustomValueMapping.json"))
            ?? throw new ArgumentNullException();

        _dictionary = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
    }

    public string GetCustomAttributeMappingValue(string attributeKey, string attributeValue)
    {
        if (string.IsNullOrEmpty(attributeKey) || string.IsNullOrEmpty(attributeValue))
            return attributeValue;

        _dictionary.TryGetValue(attributeKey, out var attribute);

        if (attribute == null)
            return attributeValue;

        attribute.TryGetValue(attributeValue, out var value);
        return value ?? attributeValue;
    }
}
