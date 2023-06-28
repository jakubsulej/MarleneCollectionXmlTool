using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpPmxiHash
{
    public byte[] Hash { get; set; }

    public ulong PostId { get; set; }

    public ushort ImportId { get; set; }

    public string PostType { get; set; }
}
