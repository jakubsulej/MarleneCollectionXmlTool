using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWssLog
{
    public ulong Id { get; set; }

    public ulong? ProductId { get; set; }

    public string Type { get; set; }

    public string Message { get; set; }

    public string Data { get; set; }

    public short? HasError { get; set; }

    public DateTime CreatedAt { get; set; }
}
