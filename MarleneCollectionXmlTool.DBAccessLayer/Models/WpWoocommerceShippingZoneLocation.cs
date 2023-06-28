using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceShippingZoneLocation
{
    public ulong LocationId { get; set; }

    public ulong ZoneId { get; set; }

    public string LocationCode { get; set; }

    public string LocationType { get; set; }
}
