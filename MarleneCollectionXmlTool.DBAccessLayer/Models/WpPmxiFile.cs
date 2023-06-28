using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpPmxiFile
{
    public ulong Id { get; set; }

    public ulong ImportId { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public DateTime RegisteredOn { get; set; }
}
