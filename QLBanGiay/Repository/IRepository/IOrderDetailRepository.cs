using QLBanGiay.Models.Models;

namespace QLBanGiay.Repository.IRepository
{
	public interface IOrderDetailRepository
	{
		Task<Orderdetail?> GetOrderDetailAsync(long orderId, long productId, string size);
		Task AddOrderDetailAsync(Orderdetail orderDetail);
		Task UpdateOrderDetailAsync(Orderdetail orderDetail);
	}
}
