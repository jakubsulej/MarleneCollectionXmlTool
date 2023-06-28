using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpBerocketTermmetum
{
    public long MetaId { get; set; }

    public long BerocketTermId { get; set; }

    public string MetaKey { get; set; }

    public string MetaValue { get; set; }
}
