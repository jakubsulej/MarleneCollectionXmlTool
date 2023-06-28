using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWoocommerceTaxRate
{
    public ulong TaxRateId { get; set; }

    public string TaxRateCountry { get; set; }

    public string TaxRateState { get; set; }

    public string TaxRate { get; set; }

    public string TaxRateName { get; set; }

    public ulong TaxRatePriority { get; set; }

    public int TaxRateCompound { get; set; }

    public int TaxRateShipping { get; set; }

    public ulong TaxRateOrder { get; set; }

    public string TaxRateClass { get; set; }
}
