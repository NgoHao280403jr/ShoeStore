using Microsoft.AspNetCore.Mvc;
using QLBanGiay.Attributes;

namespace QLBanGiay.Controllers
{
	[AuthorizeUser]
	public class CartController : Controller
	{
		public IActionResult AddToCart()
		{
			return View();
		}
	}
}
