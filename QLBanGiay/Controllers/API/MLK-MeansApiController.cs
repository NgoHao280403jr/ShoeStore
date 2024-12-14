using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Models.Models;
using Accord.MachineLearning;
using Microsoft.EntityFrameworkCore;
namespace QLBanGiay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MLK_MeansApiController : ControllerBase
    {
        private readonly QlShopBanGiayContext _context;

        public MLK_MeansApiController(QlShopBanGiayContext context)
        {
            _context = context;
        }

        [HttpPost("cluster")]
        public IActionResult ClusterProducts(int numberOfClusters = 3)
        {
            var products = _context.Products
                .Include(p => p.Category) // Load ProductCategory
                .Include(p => p.Category.Parentcategory) // Load ParentProductCategory
                .Where(p => p.Isactive)
                .Select(p => new
                {
                    p.Productid,
                    p.Price,
                    p.Discount,
                    ParentCategoryId = p.Parentcategoryid ?? 0, // Sản phẩm không có danh mục cha
                    CategoryId = p.Categoryid ?? 0 // Sản phẩm không có danh mục con
                })
                .ToList();

            if (!products.Any())
                return BadRequest("Không có sản phẩm nào để phân cụm.");

            // Chuẩn bị dữ liệu cho K-Means
            double[][] data = products
                .Select(p => new double[]
                {
                    (double)p.Price,
                    (double)p.Discount,
                    (double)p.ParentCategoryId,
                    (double)p.CategoryId
                })
                .ToArray();

            // Áp dụng K-Means
            KMeans kmeans = new KMeans(numberOfClusters);
            int[] clusters = kmeans.Learn(data).Decide(data);

            // Tạo kết quả trả về
            var result = products.Select((p, i) => new
            {
                ProductId = p.Productid,
                ClusterId = clusters[i],
                p.Price,
                p.Discount,
                p.ParentCategoryId,
                p.CategoryId
            }).ToList();

            return Ok(result);
        }

		[HttpGet("recommend/{productId}")]
		public IActionResult GetRecommendations(long productId, int numberOfClusters = 5)
		{
			var products = _context.Products
				.Include(p => p.Category)
				.Include(p => p.Category.Parentcategory)
				.Where(p => p.Isactive)
				.Select(p => new
				{
					p.Productid,
					p.Productname,
					p.Image,
					p.Price,
					p.Discount,
					ParentCategoryId = p.Parentcategoryid ?? 0,
					CategoryId = p.Categoryid ?? 0
				})
				.ToList();

			if (!products.Any())
				return BadRequest("Không có sản phẩm nào để gợi ý.");

			// Chuẩn bị dữ liệu cho K-Means
			double[][] data = products
				.Select(p => new double[]
				{
			(double)p.Price,
			(double)p.Discount,
			(double)p.ParentCategoryId,
			(double)p.CategoryId
				})
				.ToArray();

			// Áp dụng K-Means
			KMeans kmeans = new KMeans(numberOfClusters);
			int[] clusters = kmeans.Learn(data).Decide(data);

			// Tìm sản phẩm cần gợi ý
			var productIndex = products.FindIndex(p => p.Productid == productId);
			if (productIndex == -1)
				return NotFound("Sản phẩm không tồn tại.");

			int targetCluster = clusters[productIndex];

			// Lấy các sản phẩm trong cùng cụm, trừ sản phẩm hiện tại
			var recommendations = products
				.Where((p, i) => clusters[i] == targetCluster && p.Productid != productId)
				.Take(4) // Chỉ lấy 4 sản phẩm
				.ToList();

			return Ok(recommendations);
		}

	}
}
