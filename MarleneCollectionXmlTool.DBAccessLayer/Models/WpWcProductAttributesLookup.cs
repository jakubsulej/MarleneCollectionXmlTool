using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcProductAttributesLookup
{
    public long ProductId { get; set; }

    public long ProductOrParentId { get; set; }

    public string Taxonomy { get; set; }

    public long TermId { get; set; }

    public bool IsVariationAttribute { get; set; }

    public bool InStock { get; set; }
}
