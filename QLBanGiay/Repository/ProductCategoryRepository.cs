using Microsoft.EntityFrameworkCore;
using QLBanGiay.DTO;
using QLBanGiay.Models.Models;
using QLBanGiay.Repository.IRepository;

namespace QLBanGiay.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly QlShopBanGiayContext _context;

        public ProductCategoryRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }

        public async Task<List<ParentCategoryWithChildrenDto>> GetParentCategoriesWithChildrenAsync()
        {
            return await _context.Parentproductcategories
                .Select(parent => new ParentCategoryWithChildrenDto
                {
                    ParentId = parent.Parentcategoryid,
                    ParentName = parent.Parentcategoryname,
                    Categories = _context.Productcategories
                        .Where(child => child.Parentcategoryid == parent.Parentcategoryid)
                        .Select(child => new CategoryDto
                        {
                            CategoryId = child.Categoryid,
                            CategoryName = child.Categoryname
                        }).ToList()
                }).ToListAsync();
        }
    }
}
