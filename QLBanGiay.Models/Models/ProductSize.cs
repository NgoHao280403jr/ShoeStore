using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class ProductSize
{
    public long ProductSizeId { get; set; }

    public long? ProductId { get; set; } 

    public string? Size { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }
    public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }

}
