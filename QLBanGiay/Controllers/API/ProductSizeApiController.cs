using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Services;

namespace QLBanGiay.Controllers.API
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductSizeApiController : ControllerBase
	{
		private readonly ProductSizeService _productSizeService;

		public ProductSizeApiController(ProductSizeService productSizeService)
		{
			_productSizeService = productSizeService;
		}

		[HttpGet("available-sizes/{productId}")]
		public async Task<IActionResult> GetAvailableSizes(long productId)
		{
			var sizes = await _productSizeService.GetAvailableSizesAsync(productId);
			if (sizes == null || !sizes.Any())
			{
				return NotFound("No available sizes found for this product.");
			}

			return Ok(sizes);
		}
	}
}
