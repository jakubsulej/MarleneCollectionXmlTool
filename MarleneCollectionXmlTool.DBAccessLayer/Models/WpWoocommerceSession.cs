using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceSession
{
    public ulong SessionId { get; set; }

    public string SessionKey { get; set; }

    public string SessionValue { get; set; }

    public ulong SessionExpiry { get; set; }
}
