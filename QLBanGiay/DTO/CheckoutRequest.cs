namespace QLBanGiay.DTO
{
	public class CheckoutRequest
	{
		public string PhoneNumber { get; set; }
		public string DeliveryAddress { get; set; }
		public string PaymentMethod { get; set; }
		public string CustomerName { get; set; }
	}
}
