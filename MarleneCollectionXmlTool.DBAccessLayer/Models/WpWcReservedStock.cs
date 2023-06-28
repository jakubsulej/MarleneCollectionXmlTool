using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcReservedStock
{
    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public double StockQuantity { get; set; }

    public DateTime Timestamp { get; set; }

    public DateTime Expires { get; set; }
}
