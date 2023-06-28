using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcDownloadLog
{
    public ulong DownloadLogId { get; set; }

    public DateTime Timestamp { get; set; }

    public ulong PermissionId { get; set; }

    public ulong? UserId { get; set; }

    public string UserIpAddress { get; set; }
}
