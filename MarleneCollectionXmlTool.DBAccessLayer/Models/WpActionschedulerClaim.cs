using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpActionschedulerClaim
{
    public ulong ClaimId { get; set; }

    public DateTime? DateCreatedGmt { get; set; }
}
