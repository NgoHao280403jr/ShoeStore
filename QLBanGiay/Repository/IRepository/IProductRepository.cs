using QLBanGiay.Models.Models;
namespace QLBanGiay.Repository.IRepository
{
	public interface IProductRepository
	{
        Task<List<Product>> GetProductsAsync(int page, int pageSize, string sortBy, string sortOrder, long? parentCategoryId, long? categoryId);

        Task<int> GetTotalProductsAsync();
		Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> SearchProductsAsync(string searchTerm, int page, int pageSize);
        Task<int> GetSearchTotalCountAsync(string searchTerm);

    }
}
