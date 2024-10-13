using System;
using System.Collections.Generic;

namespace QLBanGiay.Models;

public partial class Invoicedetail
{
    public long Invoiceid { get; set; }

    public long Productid { get; set; }

    public int? Quantity { get; set; }

    public double? Unitprice { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
