using MarleneCollectionXmlTool.Domain.Queries.GetProductAttributes;
using System.Text.RegularExpressions;

namespace MarleneCollectionXmlTool.Domain.Helpers;

public class AttributeValuesXmlFileHelper
{
    public static GetProductAttributesResponse.Attribute GetAttriubuteValuesFromXmlFile(string responseContent, string attributeName)
    {
        string startTag = $"<{attributeName}>";
        string endTag = $"</{attributeName}>";

        string pattern = Regex.Escape(startTag) + "(.*?)" + Regex.Escape(endTag);
        MatchCollection matches = Regex.Matches(responseContent, pattern);

        var result = new GetProductAttributesResponse.Attribute
        {
            AttributeName = attributeName
        };

        foreach (Match match in matches)
        {
            string extractedText = match.Groups[1].Value.ToLower().Replace("<![cdata[ ", string.Empty).Replace(" ]]>", string.Empty);

            if (result.AttributeValues.Contains(extractedText))
                continue;

            result.AttributeValues.Add(extractedText);
        }

        return result;
    }
}
