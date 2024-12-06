namespace QLBanGiay.DTO
{
	public class UpdateCartRequest
	{
		public long ProductId { get; set; }
		public string Size { get; set; }
		public int Quantity { get; set; }
	}
}
