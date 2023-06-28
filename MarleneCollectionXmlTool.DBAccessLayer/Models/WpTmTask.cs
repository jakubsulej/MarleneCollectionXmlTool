using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpTmTask
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public string Type { get; set; }

    public string ClassIdentifier { get; set; }

    public int? Attempts { get; set; }

    public string Description { get; set; }

    public DateTime TimeCreated { get; set; }

    public long? LastLockedAt { get; set; }

    public string Status { get; set; }
}
