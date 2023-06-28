namespace MarleneCollectionXmlTool.Domain.Queries.GetProductAttributes;

public class GetProductAttributesResponse
{
    public List<Attribute> Attributes { get; set; } = new List<Attribute>();

    public class Attribute
    {
        public string AttributeName { get; set; }
        public List<string> AttributeValues { get; set; } = new List<string>();
    }
}
