namespace MarleneCollectionXmlTool.Domain.Queries.GetAllProductConfigurations;

public class GetAllProductConfigurationsResponse
{
    public Dictionary<string, List<ProductConfiguration>> AllProductConfigurations { get; set; } = new Dictionary<string, List<ProductConfiguration>>();

    public class ProductConfiguration
    {
        public string SKU { get; set; }
        public Dictionary<string, string> ProductVariations { get; set; }
        public string Price { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
