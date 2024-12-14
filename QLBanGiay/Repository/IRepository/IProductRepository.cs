using QLBanGiay.Models.Models;
namespace QLBanGiay.Repository.IRepository
{
	public interface IProductRepository
	{
        Task<List<Product>> GetProductsAsync(int page, int pageSize, string sortBy, string sortOrder, long? parentCategoryId, long? categoryId, decimal? priceMin, decimal? priceMax, string searchTerm);

		Task<int> GetTotalProductsAsync(long? parentCategoryId = null, long? categoryId = null, decimal? priceMin=null, decimal? priceMax = null, string searchTerm="");
		Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> SearchProductsAsync(string searchTerm, int page, int pageSize);
        Task<int> GetSearchTotalCountAsync(string searchTerm);

    }
}
