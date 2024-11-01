using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLBanGiay.Models.Models;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Productcategory> GetAllCategories() // Sử dụng Productcategory
        {
            return _categoryRepository.GetAllCategories();
        }

        public Productcategory GetCategoryById(long categoryId) // Sử dụng Productcategory
        {
            return _categoryRepository.GetCategoryById(categoryId);
        }

        public void AddCategory(Productcategory category) // Sử dụng Productcategory
        {
            _categoryRepository.AddCategory(category);
        }

        public void UpdateCategory(Productcategory category) // Sử dụng Productcategory
        {
            _categoryRepository.UpdateCategory(category);
        }

        public void DeleteCategory(long categoryId)
        {
            _categoryRepository.DeleteCategory(categoryId);
        }
    }
}
