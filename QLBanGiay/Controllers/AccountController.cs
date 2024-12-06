using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Models.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Lưu trạng thái đăng nhập vào Session
                    HttpContext.Session.SetString("UserId", user.Userid.ToString());
                    HttpContext.Session.SetString("Username", user.Username);

					Console.WriteLine("Đăng nhập thành công!");
					// Chuyển hướng về trang chủ
					return RedirectToAction("Index", "Home");
                }
                else
                {
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
    }
}
