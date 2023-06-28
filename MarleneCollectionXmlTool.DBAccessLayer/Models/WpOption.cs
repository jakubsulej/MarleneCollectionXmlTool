using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpOption
{
    public ulong OptionId { get; set; }

    public string OptionName { get; set; }

    public string OptionValue { get; set; }

    public string Autoload { get; set; }
}
