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
            return _context.Productcategories.ToList(); // Truy vấn tất cả danh mục từ cơ sở dữ liệu
        }

        public Productcategory GetCategoryById(long categoryId)
        {
            return _context.Productcategories.FirstOrDefault(c => c.Categoryid == categoryId); // Tìm danh mục theo ID
        }

        public void AddCategory(Productcategory category)
        {
            _context.Productcategories.Add(category); // Thêm danh mục vào cơ sở dữ liệu
            _context.SaveChanges(); // Lưu thay đổi
        }

        public void UpdateCategory(Productcategory category)
        {
            var existingCategory = GetCategoryById(category.Categoryid);
            if (existingCategory != null)
            {
                existingCategory.Categoryname = category.Categoryname;
                existingCategory.Parentcategoryid = category.Parentcategoryid;
                _context.SaveChanges(); // Lưu thay đổi
            }
        }

        public void DeleteCategory(long categoryId)
        {
            var category = GetCategoryById(categoryId);
            if (category != null)
            {
                _context.Productcategories.Remove(category); // Xóa danh mục khỏi cơ sở dữ liệu
                _context.SaveChanges(); // Lưu thay đổi
            }
        }
    }
}
