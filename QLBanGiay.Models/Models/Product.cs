using System;
using System.Collections.Generic;

namespace QLBanGiay.Models.Models;

public partial class Product
{
    public long Productid { get; set; }

    public string Productname { get; set; } = null!;

    public long? Categoryid { get; set; }

    public long? Parentcategoryid { get; set; }

    public string Image { get; set; } = null!;

    public long? Price { get; set; }

    public int? Discount { get; set; }

    public int? Ratingcount { get; set; }

    public string? Productdescription { get; set; }

    public bool Isactive { get; set; }

    public int? Quantity { get; set; }

    public virtual Productcategory? Category { get; set; }

    public virtual ICollection<Invoicedetail> Invoicedetails { get; set; } = new List<Invoicedetail>();

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual Parentproductcategory? Parentcategory { get; set; }

    public virtual ICollection<Productreview> Productreviews { get; set; } = new List<Productreview>();
}
