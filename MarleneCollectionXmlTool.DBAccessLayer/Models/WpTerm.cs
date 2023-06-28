using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpTerm
{
    public ulong TermId { get; set; }

    public string Name { get; set; }

    public string Slug { get; set; }

    public long TermGroup { get; set; }
}
