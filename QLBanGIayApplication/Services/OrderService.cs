using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay_Application.Repository.IRepository;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public Order GetOrderById(long orderId)
        {
            return _orderRepository.GetOrderById(orderId);
        }

        public void AddOrder(Order order)
        {
            _orderRepository.AddOrder(order);
        }

        public void UpdateOrder(Order order)
        {
            _orderRepository.UpdateOrder(order);
        }

        public void DeleteOrder(long orderId)
        {
            _orderRepository.DeleteOrder(orderId);
        }

        public IEnumerable<Order> SearchOrders(string keyword)
        {
            return _orderRepository.SearchOrders(keyword);
        }
    }
}
