using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpTmTaskmetum
{
    public long MetaId { get; set; }

    public long TaskId { get; set; }

    public string MetaKey { get; set; }

    public string MetaValue { get; set; }
}
