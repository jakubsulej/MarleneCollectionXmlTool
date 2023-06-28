using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpGlaShippingRate
{
    public long Id { get; set; }

    public string Country { get; set; }

    public string Currency { get; set; }

    public double Rate { get; set; }

    public string Options { get; set; }
}
