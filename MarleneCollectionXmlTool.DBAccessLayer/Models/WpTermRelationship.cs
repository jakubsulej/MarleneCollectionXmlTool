namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpTermRelationship
{
    public WpTermRelationship() { }

    public WpTermRelationship(ulong objectId, ulong termTaxonmyId)
    {
        ObjectId = objectId;
        TermTaxonomyId = termTaxonmyId;
    }

    public ulong ObjectId { get; set; }
    public ulong TermTaxonomyId { get; set; }
    public int TermOrder { get; set; }
}
