using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Models.Models;
using System.Text;

namespace QLBanGiay.Controllers
{
    public class AccountController : Controller
    {
        private readonly QlShopBanGiayContext _context;

        public AccountController(QlShopBanGiayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            Console.WriteLine("Login action được gọi");                  
            var user = _context.Users
               .FirstOrDefault(u => u.Username == model.Username);

            if (user != null && HashPassword(model.Password)== user.Password)
            {
                // Lưu trạng thái đăng nhập vào Session
                HttpContext.Session.SetString("UserId", user.Userid.ToString());
                HttpContext.Session.SetString("Username", user.Username);
				TempData["SuccessMessage"] = "You have logged in successfully!";
				Console.WriteLine("Đăng nhập thành công!");
                // Chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }
            else
            {
				// Nếu không đúng thông tin đăng nhập
				TempData["ErrorMessage"] = "Incorrect username or password. Please try again.";
				ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                Console.WriteLine("Đăng nhập thất bại!");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            // Xóa Session khi đăng xuất
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
