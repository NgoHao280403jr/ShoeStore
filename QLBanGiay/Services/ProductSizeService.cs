using QLBanGiay.Models.Models;
using QLBanGiay.Repository;
using QLBanGiay.Repository.IRepository;
namespace QLBanGiay.Services
{
	public class ProductSizeService
	{
		private readonly IProductSizeRepositoy _productSizeRepository;

		public ProductSizeService(IProductSizeRepositoy productSizeRepository)
		{
			_productSizeRepository = productSizeRepository;
		}

		public async Task<List<ProductSize>> GetAvailableSizesAsync(long productId)
		{
			return await _productSizeRepository.GetAvailableSizesAsync(productId);
		}
	}
}
