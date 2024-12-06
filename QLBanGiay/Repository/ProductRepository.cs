using Microsoft.EntityFrameworkCore;
using QLBanGiay.Models.Models;
using QLBanGiay.Repository.IRepository;

namespace QLBanGiay.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly QlShopBanGiayContext _context;

		public ProductRepository(QlShopBanGiayContext context)
		{
			_context = context;
		}

		public async Task<List<Product>> GetProductsAsync(int page, int pageSize)
		{
			return await _context.Products
				.Include(p => p.Category)
				.ThenInclude(c => c.Parentcategory)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<int> GetTotalProductsAsync()
		{
			return await _context.Products.CountAsync();
		}

		public async Task<Product> GetProductByIdAsync(int id)
		{
			return await _context.Products
				.Include(p => p.Category) 
				.ThenInclude(c => c.Parentcategory) 
				.Include(p => p.ProductSizes) 
				.FirstOrDefaultAsync(p => p.Productid == id); 
		}
	}
}
