namespace StoreItemCalculator.Lib.Models
{
	public class Product
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public decimal UnitPrice { get; set; }

		public IProductDiscountStrategy DiscountStrategy { get; set; }
	}
}
