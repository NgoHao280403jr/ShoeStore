using System;
using System.Collections.Generic;

namespace QLBanGiay.Models;

public partial class Productcategory
{
    public long Categoryid { get; set; }

    public string? Categoryname { get; set; }

    public long? Parentcategoryid { get; set; }

    public virtual Parentproductcategory? Parentcategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
