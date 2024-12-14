using QLBanGiay.DTO;
using QLBanGiay.Repository.IRepository;

namespace QLBanGiay.Services
{
    public class ProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<List<ParentCategoryWithChildrenDto>> GetParentCategoriesWithChildrenAsync()
        {
            return await _productCategoryRepository.GetParentCategoriesWithChildrenAsync();
        }
    }
}
