﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBanGiay.DTO;
using QLBanGiay.Models.Models;
using System;

namespace QLBanGiay.Controllers.API
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderApiController : ControllerBase
	{
		private readonly QlShopBanGiayContext _context;

		public OrderApiController(QlShopBanGiayContext context)
		{
			_context = context;
		}
		[HttpPost("api/cart/{customerId}/add")]
		public async Task<IActionResult> AddToCart(long customerId, [FromBody] AddToCartRequest request)
		{
			var customer = await _context.Customers.FindAsync(customerId);
			if (customer == null)
			{
				return NotFound("Customer not found.");
			}

			// Kiểm tra xem có giỏ hàng nào chưa thanh toán cho khách hàng này không
			var cart = await _context.Orders
				.FirstOrDefaultAsync(o => o.Customerid == customerId && o.Iscart && o.Orderstatus == "Cart");

			// Nếu chưa có giỏ hàng, tạo một giỏ hàng mới
			if (cart == null)
			{
				cart = new Order
				{
					Customerid = customerId,
					Ordertime = DateOnly.FromDateTime(DateTime.Now),
					Orderstatus = "Cart",
					Paymentstatus = "Not Paid",
					Iscart = true
				};

				_context.Orders.Add(cart);
				await _context.SaveChangesAsync();
			}

			// Kiểm tra sản phẩm
			var product = await _context.Products.FindAsync(request.ProductId);
			if (product == null)
			{
				return NotFound("Product not found.");
			}

			// Thêm chi tiết đơn hàng (OrderDetail)
			var orderDetail = new Orderdetail
			{
				Orderid = cart.Orderid,
				Productid = request.ProductId,
				Size = request.Size,
				Quantity = request.Quantity,
				Unitprice = product.Price
			};

			_context.Orderdetails.Add(orderDetail);
			await _context.SaveChangesAsync();

			return Ok(new { message = "Product added to cart successfully." });
		}
		[HttpGet("api/cart/{customerId}")]
		public async Task<IActionResult> GetCart(long customerId)
		{
			var cart = await _context.Orders
				.Where(o => o.Customerid == customerId && o.Iscart && o.Orderstatus == "Cart")
				.Include(o => o.Orderdetails)
					.ThenInclude(od => od.Product)
				.FirstOrDefaultAsync();

			if (cart == null)
			{
				return NotFound("Cart not found.");
			}

			var cartDetails = cart.Orderdetails.Select(od => new
			{
				ProductId = od.Productid,
				ProductName = od.Product.Productname,
				Size = od.Size,
				Quantity = od.Quantity,
				UnitPrice = od.Unitprice,
				SubTotal = od.Subtotal
			}).ToList();

			return Ok(new { cart.Orderid, cartDetails });
		}
		[HttpPut("api/cart/{customerId}/update")]
		public async Task<IActionResult> UpdateCart(long customerId, [FromBody] UpdateCartRequest request)
		{
			var cart = await _context.Orders
				.Where(o => o.Customerid == customerId && o.Iscart && o.Orderstatus == "Cart")
				.Include(o => o.Orderdetails)
				.FirstOrDefaultAsync();

			if (cart == null)
			{
				return NotFound("Cart not found.");
			}

			var orderDetail = await _context.Orderdetails
				.FirstOrDefaultAsync(od => od.Orderid == cart.Orderid && od.Productid == request.ProductId && od.Size == request.Size);

			if (orderDetail == null)
			{
				return NotFound("Product not found in the cart.");
			}

			// Cập nhật số lượng
			orderDetail.Quantity = request.Quantity;
			orderDetail.Subtotal = orderDetail.Quantity * orderDetail.Unitprice;

			await _context.SaveChangesAsync();

			return Ok(new { message = "Cart updated successfully." });
		}

		[HttpPost("api/cart/{customerId}/checkout")]
		public async Task<IActionResult> Checkout(long customerId, [FromBody] CheckoutRequest request)
		{
			var cart = await _context.Orders
				.Where(o => o.Customerid == customerId && o.Iscart && o.Orderstatus == "Cart")
				.Include(o => o.Orderdetails)
				.FirstOrDefaultAsync();

			if (cart == null)
			{
				return NotFound("Cart not found.");
			}

			// Cập nhật thông tin giỏ hàng khi thanh toán
			cart.Customername = request.CustomerName;
			cart.Phonenumber = request.PhoneNumber;
			cart.Deliveryaddress = request.DeliveryAddress;
			cart.Paymentmethod = request.PaymentMethod;
			cart.Orderstatus = "Pending";  // Đổi trạng thái giỏ hàng thành đang chờ xử lý
			cart.Paymentstatus = "Paid";  // Đổi trạng thái thanh toán thành đã thanh toán
			cart.Iscart = false;  // Đánh dấu giỏ hàng đã thanh toán

			await _context.SaveChangesAsync();

			return Ok(new { message = "Order has been placed successfully." });
		}
		[HttpGet("api/orders/{customerId}")]
		public async Task<IActionResult> GetOrders(long customerId)
		{
			var orders = await _context.Orders
				.Where(o => o.Customerid == customerId && o.Orderstatus != "Cart")
				.Include(o => o.Orderdetails)
					.ThenInclude(od => od.Product)
				.ToListAsync();

			if (orders == null || !orders.Any())
			{
				return NotFound("No orders found.");
			}

			var orderDetails = orders.Select(o => new
			{
				OrderId = o.Orderid,
				OrderStatus = o.Orderstatus,
				PaymentStatus = o.Paymentstatus,
				TotalAmount = o.Orderdetails.Sum(od => od.Subtotal),
				Products = o.Orderdetails.Select(od => new
				{
					ProductName = od.Product.Productname,
					Size = od.Size,
					Quantity = od.Quantity,
					UnitPrice = od.Unitprice,
					SubTotal = od.Subtotal
				})
			}).ToList();

			return Ok(orderDetails);
		}

	}
}
