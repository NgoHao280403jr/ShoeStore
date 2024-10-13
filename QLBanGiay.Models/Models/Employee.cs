using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class Employee
{
    public long Employeeid { get; set; }

    public long Userid { get; set; }

    public string Employeename { get; set; } = null!;

    public DateOnly? Birthdate { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User User { get; set; } = null!;
}
