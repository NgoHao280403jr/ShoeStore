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
		public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 12)
		{
			var result = await _productService.GetProductsAsync(page, pageSize);
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
	}
}
