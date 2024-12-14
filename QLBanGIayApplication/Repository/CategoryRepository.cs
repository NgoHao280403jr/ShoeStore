using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly QlShopBanGiayContext _context;

        public CategoryRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }

        public IEnumerable<Productcategory> GetAllCategories()
        {
            return _context.Productcategories.ToList(); 
        }

        public List<Productcategory> GetCategoriesByParentCategoryId(long parentCategoryId)
        {
            return _context.Productcategories
                           .Where(c => c.Parentcategoryid == parentCategoryId)
                           .ToList();
        }


        public Productcategory GetCategoryById(long categoryId)
        {
            return _context.Productcategories.FirstOrDefault(c => c.Categoryid == categoryId);
        }

        public void AddCategory(Productcategory category)
        {
            _context.Productcategories.Add(category); 
            _context.SaveChanges(); 
        }

        public void UpdateCategory(Productcategory category)
        {
            var existingCategory = GetCategoryById(category.Categoryid);
            if (existingCategory != null)
            {
                existingCategory.Categoryname = category.Categoryname;
                existingCategory.Parentcategoryid = category.Parentcategoryid;
                _context.SaveChanges(); 
            }
        }

        public void DeleteCategory(long categoryId)
        {
            var category = GetCategoryById(categoryId);
            if (category != null)
            {
                _context.Productcategories.Remove(category); 
                _context.SaveChanges(); 
            }
        }
    }
}
