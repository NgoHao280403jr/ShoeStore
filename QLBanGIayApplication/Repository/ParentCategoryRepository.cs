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
                var childCategories = _context.Productcategories.Where(c => c.Parentcategoryid == parentCategoryId).ToList();
                foreach (var childCategory in childCategories)
                {
                    var relatedProducts = _context.Products.Where(p => p.Categoryid == childCategory.Categoryid).ToList();
                    foreach (var product in relatedProducts)
                    {
                        _context.Products.Remove(product);
                    }

                    _context.Productcategories.Remove(childCategory);
                }
                var productsInParentCategory = _context.Products.Where(p => p.Parentcategoryid == parentCategoryId).ToList();
                foreach (var product in productsInParentCategory)
                {
                    _context.Products.Remove(product); 
                }

                _context.Parentproductcategories.Remove(category); 
                _context.SaveChanges(); 
            }
            else
            {
                throw new Exception("Danh mục cha không tồn tại.");
            }
        }

    }
}
