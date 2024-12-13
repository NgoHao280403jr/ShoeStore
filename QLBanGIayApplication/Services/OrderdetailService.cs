using QLBanGiay_Application.Repository;
using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Services
{
    public class OrderdetailService
    {
        private readonly IOrderdetailRepository _orderDetailRepository;
        public OrderdetailService(IOrderdetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public IEnumerable<Orderdetail> GetAllOrderDetails()
        {
            return _orderDetailRepository.GetAllOrderDetails();
        }

        public IEnumerable<Orderdetail> GetOrderDetailsByOrderId(long orderId)
        {
            return _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
        }

        public Orderdetail GetOrderDetail(long orderId, long productId)
        {
            return _orderDetailRepository.GetOrderDetail(orderId, productId);
        }

        public void AddOrderDetail(Orderdetail orderDetail)
        {
            _orderDetailRepository.AddOrderDetail(orderDetail);
        }

        public void UpdateOrderDetail(Orderdetail orderDetail)
        {
            _orderDetailRepository.UpdateOrderDetail(orderDetail);
        }

        public void DeleteOrderDetail(long orderId, long productId)
        {
            _orderDetailRepository.DeleteOrderDetail(orderId, productId);
        }
        public void DeleteAllInvoiceDetailsByInvoiceId(long orderId)
        {
            _orderDetailRepository.DeleteAllOrderDetailsByOrderId(orderId);
        }

        public IEnumerable<Orderdetail> SearchOrderDetails(string keyword)
        {
            return _orderDetailRepository.SearchOrderDetails(keyword);
        }
    }
}
