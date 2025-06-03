using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Repository;
using QLBanGiay.Repository.IRepository;

namespace QLBanGiay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public AdminAPIController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost("reindex-products")]
        public async Task<IActionResult> ReindexProducts()
        {
            await _productRepository.IndexAllProductsToElasticsearchAsync();
            return Ok("Reindex completed.");
        }
    }
}
