﻿using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpActionschedulerAction
{
    public ulong ActionId { get; set; }

    public string Hook { get; set; }

    public string Status { get; set; }

    public DateTime? ScheduledDateGmt { get; set; }

    public DateTime? ScheduledDateLocal { get; set; }

    public string Args { get; set; }

    public string Schedule { get; set; }

    public ulong GroupId { get; set; }

    public int Attempts { get; set; }

    public DateTime? LastAttemptGmt { get; set; }

    public DateTime? LastAttemptLocal { get; set; }

    public ulong ClaimId { get; set; }

    public string ExtendedArgs { get; set; }
}
