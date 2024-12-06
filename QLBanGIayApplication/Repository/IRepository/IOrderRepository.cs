using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(long orderId);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(long orderId);
        IEnumerable<Order> SearchOrders(string keyword);
    }
}
