using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpGlaShippingTime
{
    public long Id { get; set; }

    public string Country { get; set; }

    public long Time { get; set; }
}
