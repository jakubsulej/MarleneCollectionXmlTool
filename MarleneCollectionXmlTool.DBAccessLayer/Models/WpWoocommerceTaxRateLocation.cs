using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceTaxRateLocation
{
    public ulong LocationId { get; set; }

    public string LocationCode { get; set; }

    public ulong TaxRateId { get; set; }

    public string LocationType { get; set; }
}
