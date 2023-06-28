using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceLog
{
    public ulong LogId { get; set; }

    public DateTime Timestamp { get; set; }

    public short Level { get; set; }

    public string Source { get; set; }

    public string Message { get; set; }

    public string Context { get; set; }
}
