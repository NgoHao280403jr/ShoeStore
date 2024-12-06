using QLBanGiay.Models.Models;

namespace QLBanGiay.Repository.IRepository
{
	public interface IProductSizeRepository
	{
		Task<ProductSize?> GetProductSizeAsync(long productId, string size);
		Task UpdateProductSizeAsync(ProductSize productSize);
	}
}
