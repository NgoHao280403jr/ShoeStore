using QLBanGiay.Models.Models;
namespace QLBanGiay.Repository.IRepository
{
	public interface IProductSizeRepositoy
	{
		Task<List<ProductSize>> GetAvailableSizesAsync(long productId);
	}
}
