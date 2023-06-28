using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpGlaAttributeMappingRule
{
    public long Id { get; set; }

    public string Attribute { get; set; }

    public string Source { get; set; }

    public string CategoryConditionType { get; set; }

    public string Categories { get; set; }
}
