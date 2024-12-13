using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Services;

namespace QLBanGiay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryApiController : ControllerBase
    {
        private readonly ProductCategoryService _productCategoryService;

        public ProductCategoryApiController(ProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet("categories-with-children")]
        public async Task<IActionResult> GetParentCategoriesWithChildren()
        {
            var categories = await _productCategoryService.GetParentCategoriesWithChildrenAsync();
            return Ok(categories);
        }
    }
}
