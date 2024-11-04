using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay_Application.Repository.IRepository;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public Product GetProductById(long productId)
        {
            return _productRepository.GetProductById(productId);
        }

        public void AddProduct(Product product)
        {
            _productRepository.AddProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.UpdateProduct(product);
        }

        public void DeleteProduct(long productId)
        {
            _productRepository.DeleteProduct(productId);
        }

        public IEnumerable<Product> GetProductsByCategory(long categoryId)
        {
            return _productRepository.GetProductsByCategory(categoryId);
        }

        public IEnumerable<Product> GetProductsByParentCategory(long parentCategoryId) // Lấy sản phẩm theo danh mục cha
        {
            return _productRepository.GetProductsByParentCategory(parentCategoryId);
        }
    }
}
