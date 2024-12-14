using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Attributes;

namespace QLBanGiay.Controllers
{
	[AuthorizeUser]
	public class CartController : Controller
	{
		public IActionResult AddToCart(int productId)
		{
			ViewBag.ProductId = productId;
			return View();
		}
		public IActionResult CheckOut()
		{
			return View();
		}
        public IActionResult OrderComplete()
        {
            return View();
        }
    }
}
