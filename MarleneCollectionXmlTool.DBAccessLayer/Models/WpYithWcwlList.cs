using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpYithWcwlList
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string SessionId { get; set; }

    public string WishlistSlug { get; set; }

    public string WishlistName { get; set; }

    public string WishlistToken { get; set; }

    public bool WishlistPrivacy { get; set; }

    public bool IsDefault { get; set; }

    public DateTime Dateadded { get; set; }

    public DateTime? Expiration { get; set; }
}
