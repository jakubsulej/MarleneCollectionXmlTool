using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpBlcFilter
{
    public uint Id { get; set; }

    public string Name { get; set; }

    public string Params { get; set; }
}
