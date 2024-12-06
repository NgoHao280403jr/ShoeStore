using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class Orderdetail
{
	public long Orderdetailid { get; set; }

	public long Orderid { get; set; }

	public long Productid { get; set; }

	public string Size { get; set; } = null!;

	public int? Quantity { get; set; }

	public double? Unitprice { get; set; }

	public double? Subtotal { get; set; }

	public virtual Order Order { get; set; } = null!;

	public virtual Product Product { get; set; } = null!;
}
