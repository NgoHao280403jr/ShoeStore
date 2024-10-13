using System;
using System.Collections.Generic;

namespace QLBanGiay.Models;

public partial class Invoice
{
    public long Invoiceid { get; set; }

    public long Employeeid { get; set; }

    public string? Phonenumber { get; set; }

    public string Paymentmethod { get; set; } = null!;

    public DateTime Issuedate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<Invoicedetail> Invoicedetails { get; set; } = new List<Invoicedetail>();
}
