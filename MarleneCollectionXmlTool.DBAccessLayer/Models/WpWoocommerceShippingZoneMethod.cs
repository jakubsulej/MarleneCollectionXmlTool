using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceShippingZoneMethod
{
    public ulong ZoneId { get; set; }

    public ulong InstanceId { get; set; }

    public string MethodId { get; set; }

    public ulong MethodOrder { get; set; }

    public bool? IsEnabled { get; set; }
}
