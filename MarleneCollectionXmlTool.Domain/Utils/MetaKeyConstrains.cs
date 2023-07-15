namespace MarleneCollectionXmlTool.Domain.Utils;

public struct MetaKeyConstrains
{
    public const string Stock = "_stock";
    public const string StockStatus = "_stock_status";
    public const string Sku = "_sku";
    public const string Price = "_price";
    public const string TotalSales = "total_sales";
    public const string TaxStatus = "_tax_status";
    public const string TaxClass = "_tax_class";
    public const string ManageStock = "_manage_stock";
    public const string Backorders = "_backorders";
    public const string SoldIndividually = "_sold_individually";
    public const string Virtual = "_virtual";
    public const string DownloadLimit = "_download_limit";
    public const string DownloadExpiry = "_download_expiry";
    public const string WcAverageRating = "_wc_average_rating";
    public const string WcReviewCount = "_wc_review_count";
    public const string ProductAttributes = "_product_attributes";
    public const string ProductVersion = "_product_version";
    public const string HasParent = "has_parent";
    public const string Resync = "resync";
    public const string WooseaExcludeProduct = "_woosea_exclude_product";
    public const string VariantDescription = "_variation_description";
    public const string Downloadable = "_downloadable";
    public const string AttributePaRozmiar = "attribute_pa_rozmiar";
    public const string RegularPrice = "_regular_price";
    public const string ThumbnailId = "_thumbnail_id";

    public static readonly string[] AcceptableMetaKeys =
    {
        "_variation_description", 
        "total_sales", 
        "_tax_status", 
        "_tax_class", 
        "_manage_stock", 
        "_backorders", 
        "_sold_individually", 
        "_virtual", 
        "_downloadable",
        "_download_limit", 
        "_download_expiry", 
        "_stock", 
        "_stock_status", 
        "_wc_average_rating", 
        "_wc_review_count", 
        "attribute_kolor", 
        "attribute_rozmiar",
        "uniqid", 
        "_product_version", 
        "import_uid", 
        "import_started_at", 
        "_sku", 
        "_regular_price", 
        "_price"
    };
}
