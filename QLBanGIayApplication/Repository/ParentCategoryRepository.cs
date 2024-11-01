using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository
{
    public class ParentCategoryRepository : IParentCategory
    {
        private readonly QlShopBanGiayContext _context;

        public ParentCategoryRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }

        public IEnumerable<Parentproductcategory> GetAllParentCategories()
        {
            return _context.Parentproductcategories.ToList(); // Lấy tất cả danh mục cha
        }

        public Parentproductcategory GetParentCategoryById(long parentCategoryId)
        {
            return _context.Parentproductcategories.FirstOrDefault(c => c.Parentcategoryid == parentCategoryId); // Lấy danh mục cha theo ID
        }

        public void AddParentCategory(Parentproductcategory category)
        {
            _context.Parentproductcategories.Add(category); // Thêm danh mục cha
            _context.SaveChanges(); // Lưu thay đổi
        }

        public void UpdateParentCategory(Parentproductcategory category)
        {
            var existingCategory = GetParentCategoryById(category.Parentcategoryid);
            if (existingCategory != null)
            {
                existingCategory.Parentcategoryname = category.Parentcategoryname;
                _context.SaveChanges(); // Lưu thay đổi
            }
        }

        public void DeleteParentCategory(long parentCategoryId)
        {
            var category = GetParentCategoryById(parentCategoryId);
            if (category != null)
            {
                _context.Parentproductcategories.Remove(category); // Xóa danh mục cha
                _context.SaveChanges(); // Lưu thay đổi
            }
        }
    }
}
