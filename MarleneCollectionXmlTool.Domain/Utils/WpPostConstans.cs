namespace MarleneCollectionXmlTool.Domain.Utils;

public struct WpPostConstans
{
    public const string Draft = "draft";
    public const string Publish = "publish";
    public const string Open = "open";
    public const string Closed = "closed";
    public const string Product = "product";
    public const string ProductVariation = "product_variation";

    public static string GetPostName(string postTitle, string attribute) => 
        $"{postTitle} {attribute}".Replace(" ", "-").Replace("/", "-").ToLower();

    public static string GetPostTitle(string postTitle, string attribute) => 
        $"{postTitle} - {attribute}";

    public static string GetPostExcerpt(string attributeName, string attributeValue) => 
        $"{attributeName}: {attributeValue}";

    public static string GetWpParentPostGuid(string baseClientUrl, ulong postId) => 
        $"{baseClientUrl}/?post_type=product&p={postId}";

    public static string GetWpVariantPostGuid(string baseClientUrl, ulong postId) => 
        $"{baseClientUrl}/?post_type=product_variation&p={postId}";
}
