namespace StoreItemCalculator.Lib.Models
{
	public class OrderLineItem
	{
		public int SerialNumber { get; set; }

		public string ProductCode { get; set; }

		public string ProductName { get; set; }

		public decimal UnitPrice { get; set; }

		public int Quantity { get; set; }

		public decimal DiscountAmount { get; set; }

		public string DiscountDescription { get; set; }

		public decimal TotalPrice { get; set; }
	}
}
