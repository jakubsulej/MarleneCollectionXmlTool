using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpGlaMerchantIssue
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Issue { get; set; }

    public string Code { get; set; }

    public string Severity { get; set; }

    public string Product { get; set; }

    public string Action { get; set; }

    public string ActionUrl { get; set; }

    public string ApplicableCountries { get; set; }

    public string Source { get; set; }

    public string Type { get; set; }

    public DateTime CreatedAt { get; set; }
}
