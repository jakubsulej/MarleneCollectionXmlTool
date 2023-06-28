using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommercePaymentTokenmetum
{
    public ulong MetaId { get; set; }

    public ulong PaymentTokenId { get; set; }

    public string MetaKey { get; set; }

    public string MetaValue { get; set; }
}
