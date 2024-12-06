using QLBanGiay.Models.Models;

namespace QLBanGiay.Repository.IRepository
{
	public interface IOrderRepository
	{
		Task<Order?> GetPendingOrderAsync(long customerId);
		Task AddOrderAsync(Order order);
		Task SaveChangesAsync();
	}
}
