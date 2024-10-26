using System;
using System.Collections.Generic;

namespace QLBanGiay.Models;

public partial class Productreview
{
    public long Reviewid { get; set; }

    public long Productid { get; set; }

    public long Customerid { get; set; }

    public int? Rating { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
