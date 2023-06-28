using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceShippingZone
{
    public ulong ZoneId { get; set; }

    public string ZoneName { get; set; }

    public ulong ZoneOrder { get; set; }
}
