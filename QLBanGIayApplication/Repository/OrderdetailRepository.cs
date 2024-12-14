using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Repository
{
    public class OrderdetailRepository : IOrderdetailRepository
    {
        private readonly QlShopBanGiayContext _context;

        public OrderdetailRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }

        public IEnumerable<Orderdetail> GetAllOrderDetails()
        {
            return _context.Orderdetails.ToList();
        }

        public IEnumerable<Orderdetail> GetOrderDetailsByOrderId(long orderId)
        {
            return _context.Orderdetails.Where(od => od.Orderid == orderId).ToList();
        }

        public Orderdetail GetOrderDetail(long orderId, long productId)
        {
            return _context.Orderdetails.FirstOrDefault(od => od.Orderid == orderId && od.Productid == productId);
        }

        public void AddOrderDetail(Orderdetail orderDetail)
        {
            _context.Orderdetails.Add(orderDetail);
            _context.SaveChanges();
        }

        public void UpdateOrderDetail(Orderdetail orderDetail)
        {
            _context.Orderdetails.Update(orderDetail);
            _context.SaveChanges();
        }

        public void DeleteOrderDetail(long orderId, long productId)
        {
            var orderDetail = _context.Orderdetails.FirstOrDefault(od => od.Orderid == orderId && od.Productid == productId);
            if (orderDetail != null)
            {
                _context.Orderdetails.Remove(orderDetail);
                _context.SaveChanges();
            }
        }
        public void DeleteAllOrderDetailsByOrderId(long orderId)
        {
            var orderDetail = _context.Orderdetails.Where(id => id.Orderid == orderId).ToList();
            if (orderDetail.Any())
            {
                _context.Orderdetails.RemoveRange(orderDetail);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Orderdetail> SearchOrderDetails(string keyword)
        {
            return _context.Orderdetails.Where(id => id.Product.Productname.Contains(keyword)).ToList();
        }
    }
}
