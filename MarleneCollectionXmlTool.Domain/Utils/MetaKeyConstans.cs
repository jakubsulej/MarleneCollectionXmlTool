namespace MarleneCollectionXmlTool.Domain.Utils;

public struct MetaKeyConstans
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
    public const string Downloadable = "_downloadable";
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
    public const string AttributePaRozmiar = "attribute_pa_rozmiar";
    public const string AttributeKolor = "attribute_kolor";
    public const string RegularPrice = "_regular_price";
    public const string SalePrice = "_sale_price";
    public const string ThumbnailId = "_thumbnail_id";
    public const string UniqId = "uniqid";
    public const string ImportUid = "import_uid";
    public const string ImportStartedAt = "import_started_at";

    public static readonly string[] AcceptableMetaKeys =
    {
        VariantDescription, 
        TotalSales, 
        TaxStatus, 
        TaxClass, 
        ManageStock, 
        Backorders, 
        SoldIndividually, 
        Virtual, 
        Downloadable,
        DownloadLimit,
        DownloadExpiry, 
        Stock, 
        StockStatus,
        WcAverageRating,
        WcReviewCount, 
        AttributeKolor, 
        AttributePaRozmiar,
        UniqId,
        ProductVersion,
        ImportUid,
        ImportStartedAt, 
        Sku, 
        RegularPrice, 
        Price,
        SalePrice
    };
}
