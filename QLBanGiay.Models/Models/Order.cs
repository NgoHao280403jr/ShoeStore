using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class Order
{
    public long Orderid { get; set; }

    public long Customerid { get; set; }

    public string Phonenumber { get; set; } = null!;

    public string Deliveryaddress { get; set; } = null!;

    public string Paymentmethod { get; set; } = null!;

    public DateTime Ordertime { get; set; }

    public DateTime Expecteddeliverytime { get; set; }

    public string? Orderstatus { get; set; }

    public string? Paymentstatus { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
