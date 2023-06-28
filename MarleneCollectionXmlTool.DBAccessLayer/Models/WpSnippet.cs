using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpSnippet
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Code { get; set; }

    public string Tags { get; set; }

    public string Scope { get; set; }

    public short Priority { get; set; }

    public bool Active { get; set; }

    public DateTime Modified { get; set; }
}
