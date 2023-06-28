using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceOrderItem
{
    public ulong OrderItemId { get; set; }

    public string OrderItemName { get; set; }

    public string OrderItemType { get; set; }

    public ulong OrderId { get; set; }
}
