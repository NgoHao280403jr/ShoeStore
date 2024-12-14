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

		public async Task<List<Product>> GetProductsAsync(
			int page,
			int pageSize,
			string sortBy,
			string sortOrder,
			long? parentCategoryId = null,
			long? categoryId = null,
			decimal? priceMin = null,
			decimal? priceMax = null,
            string searchTerm="")
		{
			var query = _context.Products
				.Include(p => p.Category)
				.ThenInclude(c => c.Parentcategory)
				.AsQueryable();

			// Lọc theo danh mục cha
			if (parentCategoryId.HasValue)
			{
				query = query.Where(p => p.Parentcategoryid == parentCategoryId.Value);
			}

			// Lọc theo danh mục con
			if (categoryId.HasValue)
			{
				query = query.Where(p => p.Categoryid == categoryId.Value);
			}

			// Lọc theo giá
			if (priceMin.HasValue)
			{
				query = query.Where(p => p.Price >= priceMin.Value);
			}

			if (priceMax.HasValue)
			{
				query = query.Where(p => p.Price <= priceMax.Value);
			}
            if (!string.IsNullOrEmpty(searchTerm))
            {
				query = query.Where(p => p.Productname.ToLower().Contains(searchTerm.ToLower()));

			}

			// Áp dụng sắp xếp
			query = sortBy.ToLower() switch
			{
				"price" => sortOrder.ToLower() == "desc"
					? query.OrderByDescending(p => p.Price)
					: query.OrderBy(p => p.Price),
				_ => query.OrderBy(p => p.Productname) // Mặc định sắp xếp theo ProductName
			};

			// Phân trang
			return await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<int> GetTotalProductsAsync(
	         long? parentCategoryId = null,
	         long? categoryId = null,
			 decimal? priceMin = null,
			decimal? priceMax = null,
			string searchTerm = "")
		{
			var query = _context.Products.AsQueryable();

			// Lọc theo danh mục cha
			if (parentCategoryId.HasValue)
			{
				query = query.Where(p => p.Parentcategoryid == parentCategoryId.Value);
			}

			// Lọc theo danh mục con
			if (categoryId.HasValue)
			{
				query = query.Where(p => p.Categoryid == categoryId.Value);
			}
			if (priceMin.HasValue)
			{
				query = query.Where(p => p.Price >= priceMin.Value);
			}

			if (priceMax.HasValue)
			{
				query = query.Where(p => p.Price <= priceMax.Value);
			}
			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query.Where(p => p.Productname.ToLower().Contains(searchTerm.ToLower()));

			}
			return await query.CountAsync();  // Trả về tổng số sản phẩm sau khi áp dụng các bộ lọc
		}

		public async Task<Product> GetProductByIdAsync(int id)
		{
			return await _context.Products
				.Include(p => p.Category) 
				.ThenInclude(c => c.Parentcategory) 
				.Include(p => p.ProductSizes) 
				.FirstOrDefaultAsync(p => p.Productid == id); 
		}
        public async Task<List<Product>> SearchProductsAsync(string searchTerm, int page, int pageSize)
        {
            var query = _context.Products
                .Where(p => EF.Functions.Like(p.Productname.ToLower(), $"%{searchTerm.ToLower()}%") ||
                            (p.Category != null && EF.Functions.Like(p.Category.Categoryname.ToLower(), $"%{searchTerm.ToLower()}%")) ||
                            (p.Category != null && p.Category.Parentcategory != null && EF.Functions.Like(p.Category.Parentcategory.Parentcategoryname.ToLower(), $"%{searchTerm.ToLower()}%")))
                .Include(p => p.Category)
                .ThenInclude(c => c.Parentcategory)
                .AsQueryable();

            return await query
                .OrderBy(p => p.Productname)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetSearchTotalCountAsync(string searchTerm)
        {
            return await _context.Products
                .CountAsync(p => p.Productname.Contains(searchTerm) ||
                                 (p.Category != null && p.Category.Categoryname.Contains(searchTerm)) ||
                                 (p.Category != null && p.Category.Parentcategory != null && p.Category.Parentcategory.Parentcategoryname.Contains(searchTerm)));
        }

    }
}
