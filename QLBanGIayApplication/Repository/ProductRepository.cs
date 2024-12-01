using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly QlShopBanGiayContext _context;

        public ProductRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(long productId)
        {
            return _context.Products.FirstOrDefault(p => p.Productid == productId);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = GetProductById(product.Productid);
            if (existingProduct != null)
            {
                existingProduct.Productname = product.Productname;
                existingProduct.Categoryid = product.Categoryid;
                existingProduct.Parentcategoryid = product.Parentcategoryid;
                existingProduct.Image = product.Image;
                existingProduct.Price = product.Price;
                existingProduct.Discount = product.Discount;
                existingProduct.Ratingcount = product.Ratingcount;
                existingProduct.Productdescription = product.Productdescription;
                existingProduct.Isactive = product.Isactive;
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(long productId)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Product> GetProductsByCategory(long categoryId)
        {
            return _context.Products.Where(p => p.Categoryid == categoryId).ToList();
        }

        public IEnumerable<Product> GetProductsByParentCategory(long parentCategoryId)
        {
            return _context.Products.Where(p => p.Parentcategoryid == parentCategoryId).ToList();
        }
    }
}
