using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcTaxRateClass
{
    public ulong TaxRateClassId { get; set; }

    public string Name { get; set; }

    public string Slug { get; set; }
}
