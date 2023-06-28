namespace MarleneCollectionXmlTool.Domain.Helpers;

public static class StringExtenions
{
    public static string FirstLetterToUpper(this string str)
    {
        if (str == null)
            return null;

        if (str.Length > 1)
            return char.ToUpper(str[0]) + str[1..].ToLower();

        return str.ToUpper();
    }
}
