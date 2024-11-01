using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IParentCategory
    {
        IEnumerable<Parentproductcategory> GetAllParentCategories(); // Lấy tất cả danh mục cha
        Parentproductcategory GetParentCategoryById(long parentCategoryId); // Lấy danh mục cha theo ID
        void AddParentCategory(Parentproductcategory category); // Thêm danh mục cha
        void UpdateParentCategory(Parentproductcategory category); // Cập nhật danh mục cha
        void DeleteParentCategory(long parentCategoryId); // Xóa danh mục cha
    }
}
