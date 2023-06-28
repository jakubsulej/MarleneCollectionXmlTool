using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceOrderItemmetum
{
    public ulong MetaId { get; set; }

    public ulong OrderItemId { get; set; }

    public string MetaKey { get; set; }

    public string MetaValue { get; set; }
}
