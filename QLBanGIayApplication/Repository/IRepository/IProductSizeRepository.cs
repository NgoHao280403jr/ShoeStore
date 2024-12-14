using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IProductSizeRepository
    {
        IEnumerable<ProductSize> GetAllProductSizes();
        ProductSize GetProductSizeById(long productSizeId);
        void AddProductSize(ProductSize productSize);
        void UpdateProductSize(ProductSize productSize);
        void DeleteProductSize(long productId);
        ProductSize GetProductSizesByProductIdAndSize(int productId, string size);
        IEnumerable<ProductSize> GetProductSizeByCategory(long productId);
    }
}
