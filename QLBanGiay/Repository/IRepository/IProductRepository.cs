using QLBanGiay.Models.Models;
namespace QLBanGiay.Repository.IRepository
{
	public interface IProductRepository
	{
		Task<List<Product>> GetProductsAsync(int page, int pageSize);
		Task<int> GetTotalProductsAsync();
		Task<Product> GetProductByIdAsync(int id);
	}
}
