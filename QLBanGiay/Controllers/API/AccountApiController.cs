using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBanGiay.DTO;
using QLBanGiay.Models.Models;

namespace QLBanGiay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly QlShopBanGiayContext _dbContext;

        public AccountApiController(QlShopBanGiayContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Kiểm tra dữ liệu đầu vào
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new { Message = "Passwords do not match." });
            }

            if (await _dbContext.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest(new { Message = "Username already exists." });
            }

            if (await _dbContext.Customers.AnyAsync(c => c.Email == request.Email))
            {
                return BadRequest(new { Message = "Email already exists." });
            }

            // Tạo User mới
            var user = new User
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Roleid = 3, // ID Role mặc định cho khách hàng
                Isactive = true,
                Isbanned = false
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Tạo Customer liên kết với User
            var customer = new Customer
            {
                Customerid = user.Userid,
                Userid = user.Userid,
                Customername = user.Username, // Sử dụng Username làm tên mặc định
                Address = "Not specified", // Địa chỉ mặc định
                Phonenumber = "", // Số điện thoại để trống
                Email = request.Email
            };

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Registration successful!" });
        }
    }
}
