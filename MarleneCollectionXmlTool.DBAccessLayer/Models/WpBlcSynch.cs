using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpBlcSynch
{
    public uint ContainerId { get; set; }

    public string ContainerType { get; set; }

    public byte Synched { get; set; }

    public DateTime LastSynch { get; set; }
}
