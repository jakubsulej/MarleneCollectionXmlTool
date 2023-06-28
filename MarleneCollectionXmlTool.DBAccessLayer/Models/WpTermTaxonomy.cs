namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpTermTaxonomy
{
    public WpTermTaxonomy() { }

    public WpTermTaxonomy(string taxonomy)
    {
        Taxonomy = taxonomy;
    }

    public ulong TermTaxonomyId { get; set; }

    public ulong TermId { get; set; }

    public string Taxonomy { get; set; }

    public string Description { get; set; }

    public ulong Parent { get; set; }

    public long Count { get; set; }
}
