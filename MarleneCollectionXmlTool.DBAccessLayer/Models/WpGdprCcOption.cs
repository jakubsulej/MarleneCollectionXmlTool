using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpGdprCcOption
{
    public int Id { get; set; }

    public string OptionKey { get; set; }

    public string OptionValue { get; set; }

    public int? SiteId { get; set; }

    public string Extras { get; set; }
}
