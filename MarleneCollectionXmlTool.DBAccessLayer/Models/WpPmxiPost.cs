using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpPmxiPost
{
    public ulong Id { get; set; }

    public ulong PostId { get; set; }

    public ulong ImportId { get; set; }

    public string UniqueKey { get; set; }

    public string ProductKey { get; set; }

    public long Iteration { get; set; }

    public bool Specified { get; set; }
}
