using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpBlcLink
{
    public uint LinkId { get; set; }

    public string Url { get; set; }

    public DateTime FirstFailure { get; set; }

    public DateTime LastCheck { get; set; }

    public DateTime LastSuccess { get; set; }

    public DateTime LastCheckAttempt { get; set; }

    public uint CheckCount { get; set; }

    public string FinalUrl { get; set; }

    public ushort RedirectCount { get; set; }

    public string Log { get; set; }

    public short HttpCode { get; set; }

    public string StatusCode { get; set; }

    public string StatusText { get; set; }

    public float RequestDuration { get; set; }

    public byte Timeout { get; set; }

    public byte Broken { get; set; }

    public byte Warning { get; set; }

    public bool? MayRecheck { get; set; }

    public bool BeingChecked { get; set; }

    public string ResultHash { get; set; }

    public bool FalsePositive { get; set; }

    public bool Dismissed { get; set; }
}
