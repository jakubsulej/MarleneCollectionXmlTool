using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommercePaymentToken
{
    public ulong TokenId { get; set; }

    public string GatewayId { get; set; }

    public string Token { get; set; }

    public ulong UserId { get; set; }

    public string Type { get; set; }

    public bool IsDefault { get; set; }
}
