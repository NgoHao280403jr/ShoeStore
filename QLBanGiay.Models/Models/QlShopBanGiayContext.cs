﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLBanGiay.Models.Models;

public partial class QlShopBanGiayContext : DbContext
{
    public QlShopBanGiayContext()
    {
    }

    public QlShopBanGiayContext(DbContextOptions<QlShopBanGiayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Invoicedetail> Invoicedetails { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }
    public virtual DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

    public virtual DbSet<Parentproductcategory> Parentproductcategories { get; set; }

    public virtual DbSet<Product> Products { get; set; }
	public virtual DbSet<ProductSize> ProductSizes { get; set; }

	public virtual DbSet<Productcategory> Productcategories { get; set; }

    public virtual DbSet<Productreview> Productreviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=QL_ShopBanGiay;User ID=postgres;Password=123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.ProductSizeId).HasName("productsize_pkey");

            entity.ToTable("productsize");

            entity.Property(e => e.ProductSizeId).HasColumnName("productsizeid");
            entity.Property(e => e.ProductId).HasColumnName("productid");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .HasColumnName("size");
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .HasDefaultValue(0);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductSizes) 
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_productsize_product");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.HasIndex(e => e.Email, "customer_email_key").IsUnique();

            entity.HasIndex(e => e.Userid, "uni_userid_customer").IsUnique();

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasDefaultValueSql("'Not specified'::character varying")
                .HasColumnName("address");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Customername)
                .HasMaxLength(255)
                .HasColumnName("customername");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("phonenumber");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customer_user");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Employeeid).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.HasIndex(e => e.Email, "employee_email_key").IsUnique();

            entity.HasIndex(e => e.Userid, "uni_userid_employee").IsUnique();

            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasDefaultValueSql("'Not specified'::character varying")
                .HasColumnName("address");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Employeename)
                .HasMaxLength(255)
                .HasColumnName("employeename");
            entity.Property(e => e.Gender)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("phonenumber");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_user");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Invoiceid).HasName("invoice_pkey");

            entity.ToTable("invoice");

            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Issuedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issuedate");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(30)
                .HasColumnName("paymentmethod");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(10)
                .HasColumnName("phonenumber");

            entity.HasOne(d => d.Employee).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoice_employee");
        });

        modelBuilder.Entity<Invoicedetail>(entity =>
        {
            entity.HasKey(e => new { e.Invoiceid, e.Productid }).HasName("pk_invoicedetail");

            entity.ToTable("invoicedetail");

            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Unitprice).HasColumnName("unitprice");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Invoicedetails)
                .HasForeignKey(d => d.Invoiceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoicedetail_invoice");

            entity.HasOne(d => d.Product).WithMany(p => p.Invoicedetails)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoicedetail_product");
        });

		modelBuilder.Entity<Order>(entity =>
		{
			entity.HasKey(e => e.Orderid).HasName("orders_pkey");

			entity.ToTable("orders");

			entity.Property(e => e.Orderid).HasColumnName("orderid");
			entity.Property(e => e.Customerid).HasColumnName("customerid");
			entity.Property(e => e.Deliveryaddress)
				.HasMaxLength(255)
				.HasColumnName("deliveryaddress");
			entity.Property(e => e.Expecteddeliverytime).HasColumnName("expecteddeliverytime");
			entity.Property(e => e.Iscart)
				.HasDefaultValue(true)
				.HasColumnName("iscart");
			entity.Property(e => e.Orderstatus)
				.HasMaxLength(30)
				.HasDefaultValueSql("'Cart'::character varying")
				.HasColumnName("orderstatus");
			entity.Property(e => e.Ordertime).HasColumnName("ordertime");
			entity.Property(e => e.Paymentmethod)
				.HasMaxLength(30)
				.HasColumnName("paymentmethod");
			entity.Property(e => e.Paymentstatus)
				.HasMaxLength(30)
				.HasDefaultValueSql("'Not Paid'::character varying")
				.HasColumnName("paymentstatus");
			entity.Property(e => e.Phonenumber)
				.HasMaxLength(10)
				.HasColumnName("phonenumber");
            entity.Property(e => e.Customername)
                .HasMaxLength(255)
                .HasColumnName("customername");
			entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
				.HasForeignKey(d => d.Customerid)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_order_customer");
		});

		modelBuilder.Entity<Orderdetail>(entity =>
		{
			entity.HasKey(e => e.Orderdetailid).HasName("orderdetail_pkey");

			entity.ToTable("orderdetail");

			entity.Property(e => e.Orderdetailid).HasColumnName("orderdetailid");
			entity.Property(e => e.Orderid).HasColumnName("orderid");
			entity.Property(e => e.Productid).HasColumnName("productid");
			entity.Property(e => e.Quantity).HasColumnName("quantity");
			entity.Property(e => e.Size)
				.HasMaxLength(50)
				.HasColumnName("size");
			entity.Property(e => e.Subtotal)
				.HasComputedColumnSql("((quantity)::double precision * unitprice)", true)
				.HasColumnName("subtotal");
			entity.Property(e => e.Unitprice).HasColumnName("unitprice");

			entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
				.HasForeignKey(d => d.Orderid)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_orderdetail_order");

			entity.HasOne(d => d.Product).WithMany(p => p.Orderdetails)
				.HasForeignKey(d => d.Productid)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_orderdetail_product");
		});

		modelBuilder.Entity<Parentproductcategory>(entity =>
        {
            entity.HasKey(e => e.Parentcategoryid).HasName("parentproductcategory_pkey");

            entity.ToTable("parentproductcategory");

            entity.HasIndex(e => e.Parentcategoryname, "parentproductcategory_parentcategoryname_key").IsUnique();

            entity.Property(e => e.Parentcategoryid).HasColumnName("parentcategoryid");
            entity.Property(e => e.Parentcategoryname)
                .HasMaxLength(255)
                .HasColumnName("parentcategoryname");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("product_pkey");

            entity.ToTable("product");

            entity.HasIndex(e => e.Productname, "product_productname_key").IsUnique();

            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Parentcategoryid).HasColumnName("parentcategoryid");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Productdescription).HasColumnName("productdescription");
            entity.Property(e => e.Productname)
                .HasMaxLength(255)
                .HasColumnName("productname");
            entity.Property(e => e.Ratingcount).HasColumnName("ratingcount");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("product_categoryid_fkey");

            entity.HasOne(d => d.Parentcategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.Parentcategoryid)
                .HasConstraintName("product_parentcategoryid_fkey");
        });
		modelBuilder.Entity<ProductSize>(entity =>
		{
			entity.HasKey(e => e.ProductSizeId).HasName("productsize_pkey");

			entity.ToTable("productsize");

			entity.Property(e => e.ProductSizeId).HasColumnName("productsizeid");
			entity.Property(e => e.ProductId).HasColumnName("productid");
			entity.Property(e => e.Quantity).HasColumnName("quantity");
			entity.Property(e => e.Size)
				.HasMaxLength(50)
				.HasColumnName("size");

			entity.HasOne(d => d.Product).WithMany(p => p.ProductSizes)
				.HasForeignKey(d => d.ProductId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("productsize_productid_fkey");
		});
		modelBuilder.Entity<Productcategory>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("productcategory_pkey");

            entity.ToTable("productcategory");

            entity.HasIndex(e => e.Categoryname, "productcategory_categoryname_key").IsUnique();

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(255)
                .HasColumnName("categoryname");
            entity.Property(e => e.Parentcategoryid).HasColumnName("parentcategoryid");

            entity.HasOne(d => d.Parentcategory).WithMany(p => p.Productcategories)
                .HasForeignKey(d => d.Parentcategoryid)
                .HasConstraintName("fk_productcategory_parent");
        });

        modelBuilder.Entity<Productreview>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("productreview_pkey");

            entity.ToTable("productreview");

            entity.Property(e => e.Reviewid).HasColumnName("reviewid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Customer).WithMany(p => p.Productreviews)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productreview_customer");

            entity.HasOne(d => d.Product).WithMany(p => p.Productreviews)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productreview_product");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.Rolename, "role_rolename_key").IsUnique();

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(255)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isbanned).HasColumnName("isbanned");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role");
        });
        modelBuilder.Entity<PurchaseInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("purchaseinvoice_pkey");

            entity.ToTable("purchaseinvoice");

            entity.Property(e => e.InvoiceId)
                .HasColumnName("invoiceid")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ProductId)
                .HasColumnName("productid")
                .IsRequired();
            entity.Property(e => e.ProductSizeId)
                .HasColumnName("productsizeid");

            entity.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(e => e.UnitPrice)
                .HasColumnName("unitprice")
                .IsRequired()
                .HasDefaultValue(0.0);

            entity.Property(e => e.TotalPrice)
                .HasColumnName("totalprice")
                .HasComputedColumnSql("[quantity] * [unitprice]");

            entity.Property(e => e.ImportDate)
                .HasColumnName("importdate");

            entity.Property(e => e.EmployeeId)
                .HasColumnName("employeeid")
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany(p => p.PurchaseInvoices)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_purchaseinvoice_product");

            entity.HasOne(e => e.Employee)
                .WithMany(emp => emp.PurchaseInvoices)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_purchaseinvoice_employee");
            entity.HasOne(e => e.ProductSize)
                .WithMany(ps => ps.PurchaseInvoices)
                .HasForeignKey(e => e.ProductSizeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_purchaseinvoice_productsize");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
