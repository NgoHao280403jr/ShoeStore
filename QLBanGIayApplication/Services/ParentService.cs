using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Services
{
    public class ParentService
    {
        private readonly IParentCategory _parentCategoryRepository;

        public ParentService(IParentCategory parentCategoryRepository)
        {
            _parentCategoryRepository = parentCategoryRepository;
        }

        // Lấy tất cả danh mục cha
        public IEnumerable<Parentproductcategory> GetAllParentCategories()
        {
            return _parentCategoryRepository.GetAllParentCategories();
        }

        // Lấy danh mục cha theo ID
        public Parentproductcategory GetParentCategoryById(long parentCategoryId)
        {
            return _parentCategoryRepository.GetParentCategoryById(parentCategoryId);
        }

        // Thêm danh mục cha mới
        public void AddParentCategory(Parentproductcategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _parentCategoryRepository.AddParentCategory(category);
        }

        // Cập nhật danh mục cha
        public void UpdateParentCategory(Parentproductcategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _parentCategoryRepository.UpdateParentCategory(category);
        }

        // Xóa danh mục cha
        public void DeleteParentCategory(long parentCategoryId)
        {
            _parentCategoryRepository.DeleteParentCategory(parentCategoryId);
        }
    }
}
