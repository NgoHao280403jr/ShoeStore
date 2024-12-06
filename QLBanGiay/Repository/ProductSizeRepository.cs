using QLBanGiay.Repository.IRepository;
using QLBanGiay.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace QLBanGiay.Repository
{
	public class ProductSizeRepository: IProductSizeRepositoy
	{
		private readonly QlShopBanGiayContext _context;

		public ProductSizeRepository(QlShopBanGiayContext context)
		{
			_context = context;
		}
		public async Task<List<ProductSize>> GetAvailableSizesAsync(long productId)
		{
			return await _context.ProductSizes
			.Where(ps => ps.ProductId == productId && ps.Quantity > 0)
			.ToListAsync();
		}
	}
}
