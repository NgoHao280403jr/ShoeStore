using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();           
        Product GetProductById(long productId);          
        void AddProduct(Product product);                
        void UpdateProduct(Product product);            
        void DeleteProduct(long productId);              
        IEnumerable<Product> GetProductsByCategory(long categoryId);
        IEnumerable<Product> GetProductsByParentCategory(long parentCategoryId);
    }
}
