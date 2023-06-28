using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcOrderCouponLookup
{
    public ulong OrderId { get; set; }

    public long CouponId { get; set; }

    public DateTime DateCreated { get; set; }

    public double DiscountAmount { get; set; }
}
