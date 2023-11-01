namespace MarleneCollectionXmlTool.Domain.Queries.GetAllProductsWithoutSkus;

public class GetAllProductsWithoutSkusResponse
{
    //public string ProductWithoutSkuRow { get; set; }
    public List<ParentProductDto> ProductsWithoutSkus { get; set; }
}

public record ParentProductDto
{
    public string Id { get; set; }
    public string ProductName { get; set; }
    public string CatalogeCode { get; set; }
    public List<VariantProductDto> VariantProducts { get; set; } = new List<VariantProductDto>();

    public record VariantProductDto
    {
        public string Id { get; set; }
        public string Sku { get; set; }
        public string Size { get; set; }
    }
}
