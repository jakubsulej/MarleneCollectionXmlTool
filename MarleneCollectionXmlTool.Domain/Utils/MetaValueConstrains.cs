namespace MarleneCollectionXmlTool.Domain.Utils;

public struct MetaValueConstrains
{
    public const string OutOfStock = "outofstock";
    public const string InStock = "instock";
    public const string No = "no";
    public const string Yes = "yes";
    public const string ProductVersion = "7.7.0";
    public const string Parent = "parent";
    public const string Taxable = "taxable";

    public static string GetPriceFormat(string price) => (decimal.Parse(price)).ToString("0");
}
