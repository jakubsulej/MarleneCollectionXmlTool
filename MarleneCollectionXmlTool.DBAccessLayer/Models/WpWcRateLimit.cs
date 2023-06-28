using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcRateLimit
{
    public ulong RateLimitId { get; set; }

    public string RateLimitKey { get; set; }

    public ulong RateLimitExpiry { get; set; }

    public short RateLimitRemaining { get; set; }
}
