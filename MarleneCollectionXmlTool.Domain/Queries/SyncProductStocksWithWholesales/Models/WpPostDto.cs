namespace MarleneCollectionXmlTool.Domain.Queries.SyncProductStocksWithWholesales.Models;

public class WpPostDto
{
    public WpPostDto() { }

    public WpPostDto(string attributeKolor, string attributeRozmiar, string attributeFason, string attributeDlugosc, string attributeWzor)
    {
        AttributeKolor = attributeKolor;
        AttributeRozmiar = attributeRozmiar;
        AttributeFason = attributeFason;
        AttributeDlugosc = attributeDlugosc;
        AttributeWzor = attributeWzor;
    }

    public string PostContent { get; set; }
    public string PostTitle { get; set; }
    public string PostExcerpt { get; set; }
    public string Sku { get; set; }
    public string StockStatus { get; set; }
    public string Stock { get; set; }
    public int StockInt => int.Parse(Stock);
    public string AttributeKolor { get; set; }
    public string AttributeRozmiar { get; set; }
    public string AttributeFason { get; set; }
    public string AttributeDlugosc { get; set; }
    public string AttributeWzor { get; set; }
    public List<string> ImageUrls { get; set; }
    public string RegularPrice { get; set; }
}
