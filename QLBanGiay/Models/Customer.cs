using System;
using System.Collections.Generic;

namespace QLBanGiay.Models;

public partial class Customer
{
    public long Customerid { get; set; }

    public long Userid { get; set; }

    public string Customername { get; set; } = null!;

    public DateOnly? Birthdate { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Productreview> Productreviews { get; set; } = new List<Productreview>();

    public virtual User User { get; set; } = null!;
}
