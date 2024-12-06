namespace QLBanGiay.DTO
{
	public class AddToCartRequest
	{
		public long ProductId { get; set; }
		public string Size { get; set; }
		public int Quantity { get; set; }
	}
}
