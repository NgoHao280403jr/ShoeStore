using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBanGiay.Models;


namespace QLBanGiay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly QlShopBanGiayContext _context;

        public ProductApiController(QlShopBanGiayContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 12)
        {
            // Tổng số sản phẩm
            var totalItems = await _context.Products.CountAsync();

            // Tính toán dữ liệu phân trang
            var products = await _context.Products
                .Include(p => p.Category) 
                .ThenInclude(c => c.Parentcategory) 
                .Skip((page - 1) * pageSize) 
                .Take(pageSize) 
                .Select(p => new
                {
                    ProductId = p.Productid,
                    ProductName = p.Productname,
                    p.Price,
                    p.Discount,
                    p.Quantity,
                    p.Image,
                    IsActive = p.Isactive,
                    Category = p.Category == null ? null : new
                    {
                        CategoryId = p.Category.Categoryid,
                        CategoryName = p.Category.Categoryname,
                        ParentCategory = p.Category.Parentcategory == null ? null : new
                        {
                            ParentCategoryId = p.Category.Parentcategory.Parentcategoryid,
                            ParentCategoryName = p.Category.Parentcategory.Parentcategoryname
                        }
                    },
                    ParentCategory = p.Parentcategory == null ? null : new
                    {
                        ParentCategoryId = p.Parentcategory.Parentcategoryid,
                        ParentCategoryName = p.Parentcategory.Parentcategoryname
                    }
                })
                .ToListAsync();

            // Chuẩn bị phản hồi phân trang
            var response = new
            {
                Data = products, 
                CurrentPage = page, 
                PageSize = pageSize, 
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize), 
                TotalItems = totalItems 
            };

            return Ok(response);
        }
    }
}
