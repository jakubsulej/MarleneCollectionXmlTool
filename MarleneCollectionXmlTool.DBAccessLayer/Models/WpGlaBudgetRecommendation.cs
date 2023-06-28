using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpGlaBudgetRecommendation
{
    public long Id { get; set; }

    public string Currency { get; set; }

    public string Country { get; set; }

    public int DailyBudgetLow { get; set; }

    public int DailyBudgetHigh { get; set; }
}
