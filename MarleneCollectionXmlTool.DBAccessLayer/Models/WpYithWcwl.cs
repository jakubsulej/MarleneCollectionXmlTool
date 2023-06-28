using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpYithWcwl
{
    public long Id { get; set; }

    public long ProdId { get; set; }

    public int Quantity { get; set; }

    public long? UserId { get; set; }

    public long? WishlistId { get; set; }

    public int? Position { get; set; }

    public decimal? OriginalPrice { get; set; }

    public string OriginalCurrency { get; set; }

    public DateTime Dateadded { get; set; }

    public sbyte OnSale { get; set; }
}
