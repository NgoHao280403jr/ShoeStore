using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class Orderdetail
{
    public long Orderid { get; set; }

    public long Productid { get; set; }

    public int? Quantity { get; set; }

    public double? Unitprice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
