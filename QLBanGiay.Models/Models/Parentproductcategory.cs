using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class Parentproductcategory
{
    public long Parentcategoryid { get; set; }

    public string? Parentcategoryname { get; set; }

    public virtual ICollection<Productcategory> Productcategories { get; set; } = new List<Productcategory>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
