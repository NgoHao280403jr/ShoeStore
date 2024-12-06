using Microsoft.EntityFrameworkCore;
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Repository
{
    public class OrderRepository
    {
        private readonly QlShopBanGiayContext _context;

        public OrderRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.Include(o => o.Customer).ToList();
        }

        public Order GetOrderById(long orderId)
        {
            return _context.Orders.Include(o => o.Customer).FirstOrDefault(o => o.Orderid == orderId);
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(long orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Orderid == orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Order> SearchOrders(string keyword)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.Deliveryaddress.Contains(keyword) ||
                            o.Phonenumber.Contains(keyword) ||
                            o.Customer.Customername.Contains(keyword))
                .ToList();
        }
    }
}
