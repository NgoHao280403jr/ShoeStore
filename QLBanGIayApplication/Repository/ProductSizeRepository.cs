using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace QLBanGiay_Application.Repository
{
    public class ProductSizeRepository : IProductSizeRepository
    {
        private readonly QlShopBanGiayContext _context;
        public ProductSizeRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }
        public ProductSize GetProductSizesByProductIdAndSize(int productId, string size)
        {
            return _context.ProductSizes
                           .FirstOrDefault(ps => ps.ProductId == productId && ps.Size == size);
        }

        public IEnumerable<ProductSize> GetAllProductSizes()
        {
            return _context.ProductSizes.Include(ps => ps.Product).ToList();
        }

        public ProductSize GetProductSizeById(long productSizeId)
        {
            return _context.ProductSizes.Include(ps => ps.Product)
                                        .FirstOrDefault(ps => ps.ProductSizeId == productSizeId);
        }

        public void AddProductSize(ProductSize productSize)
        {
            _context.ProductSizes.Add(productSize);
            _context.SaveChanges();
        }

        public void UpdateProductSize(ProductSize productSize)
        {
            _context.ProductSizes.Update(productSize);
            _context.SaveChanges();
        }

        public void DeleteProductSize(long productSizeId)
        {
            var productSize = _context.ProductSizes.Find(productSizeId);
            if (productSize != null)
            {
                _context.ProductSizes.Remove(productSize);
                _context.SaveChanges();
            }
        }

        public IEnumerable<ProductSize> GetProductSizeByCategory(long productId)
        {
            return _context.ProductSizes.Where(ps => ps.ProductId == productId).ToList();
        }
    }
}
