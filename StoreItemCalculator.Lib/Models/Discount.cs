namespace StoreItemCalculator.Lib.Models
{
	public class Discount
	{
		public DiscountType Type { get; set; }

		public decimal Percent { get; set; }

		public int UnitsNeeded { get; set; }

		public int UnitsFree { get; set; }
	}

	public enum DiscountType
	{
		Flat,
		Unit
	}
}
