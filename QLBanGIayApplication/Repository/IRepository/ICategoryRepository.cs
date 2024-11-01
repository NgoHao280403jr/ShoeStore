using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface ICategoryRepository
    {
        IEnumerable<Productcategory> GetAllCategories();
        Productcategory GetCategoryById(long categoryId);
        void AddCategory(Productcategory category);
        void UpdateCategory(Productcategory category);
        void DeleteCategory(long categoryId);
    }
}
