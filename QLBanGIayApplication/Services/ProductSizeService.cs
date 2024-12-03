using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;

namespace QLBanGiay_Application.Services
{
    public class ProductSizeService
    {
        private readonly IProductSizeRepository _productSizeRepository;

        public ProductSizeService(IProductSizeRepository productSizeRepository)
        {
            _productSizeRepository = productSizeRepository;
        }

        public IEnumerable<ProductSize> GetAllProductSizes()
        {
            return _productSizeRepository.GetAllProductSizes();
        }

        public ProductSize GetProductSizeById(long productSizeId)
        {
            return _productSizeRepository.GetProductSizeById(productSizeId);
        }

        public void AddProductSize(ProductSize productSize)
        {
            _productSizeRepository.AddProductSize(productSize);
        }

        public void UpdateProductSize(ProductSize productSize)
        {
            _productSizeRepository.UpdateProductSize(productSize);
        }

        public void DeleteProductSize(long productSizeId)
        {
            _productSizeRepository.DeleteProductSize(productSizeId);
        }

        public IEnumerable<ProductSize> GetProductSizeByCategory(long productId)
        {
            return _productSizeRepository.GetProductSizeByCategory(productId);
        }
    }
}
