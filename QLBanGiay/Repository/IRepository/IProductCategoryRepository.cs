using QLBanGiay.DTO;

namespace QLBanGiay.Repository.IRepository
{
    public interface IProductCategoryRepository
    {
        Task<List<ParentCategoryWithChildrenDto>> GetParentCategoriesWithChildrenAsync();
    }
}
