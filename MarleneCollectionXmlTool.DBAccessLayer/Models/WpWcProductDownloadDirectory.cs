using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcProductDownloadDirectory
{
    public ulong UrlId { get; set; }

    public string Url { get; set; }

    public bool Enabled { get; set; }
}
