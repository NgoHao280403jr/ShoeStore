using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IOrderdetailRepository
    {
        IEnumerable<Orderdetail> GetAllOrderDetails();
        IEnumerable<Orderdetail> GetOrderDetailsByOrderId(long orderId);
        Orderdetail GetOrderDetail(long orderId, long productId);
        Orderdetail GetOrderDetaiById(long orderDetailId);
        void AddOrderDetail(Orderdetail orderDetail);
        void UpdateOrderDetail(Orderdetail orderDetail);
        void DeleteOrderDetail(long orderId, long productId);
        void DeleteAllOrderDetailsByOrderId(long orderId);
        IEnumerable<Orderdetail> SearchOrderDetails(string keyword);
    }
}
