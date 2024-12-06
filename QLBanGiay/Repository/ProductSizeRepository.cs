using Microsoft.EntityFrameworkCore;
using QLBanGiay.Models.Models;
using QLBanGiay.Repository.IRepository;

namespace QLBanGiay.Repository
{
	public class ProductSizeRepository : IProductSizeRepository
	{
		private readonly QlShopBanGiayContext _context;

		public ProductSizeRepository(QlShopBanGiayContext context)
		{
			_context = context;
		}
		public async Task<ProductSize?> GetProductSizeAsync(long productId, string size)
		{
			return await _context.ProductSizes
				.FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.Size == size);
		}

		public async Task UpdateProductSizeAsync(ProductSize productSize)
		{
			_context.ProductSizes.Update(productSize);
			await _context.SaveChangesAsync();
		}
	}
}
