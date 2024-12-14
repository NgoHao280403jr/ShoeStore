using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Services;

namespace QLBanGiay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
		private readonly ProductService _productService;

		public ProductApiController(ProductService productService)
		{
			_productService = productService;
		}

        // GET: api/ProductApi
        [HttpGet]
        public async Task<IActionResult> GetProducts(
            int page = 1,
            int pageSize = 12,
            string sortBy = "",
            string sortOrder = "",
            long? parentCategoryId = null,
            long? categoryId = null)
        {
            var result = await _productService.GetProductsAsync(page, pageSize, sortBy, sortOrder, parentCategoryId, categoryId);
            return Ok(result);
        }

        // GET: api/ProductApi/{id}
        [HttpGet("{id}")]
		public async Task<IActionResult> GetProductById(int id)
		{
			var result = await _productService.GetProductByIdAsync(id);
			if (result == null)
			{
				return NotFound();
			}

			return Ok(result);
		}
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string searchTerm, int page = 1, int pageSize = 10)
        {
            var result = await _productService.SearchProductsAsync(searchTerm, page, pageSize);
            return Ok(result);
        }

    }
}
