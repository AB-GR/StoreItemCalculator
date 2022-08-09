namespace StoreItemCalculator.Lib.Models
{
	public class Discount
	{
		public int Id { get; set; }

		public DiscountType Type { get; set; }

		public decimal Percent { get; set; }

		public int UnitsNeeded { get; set; }

		public int UnitsFree { get; set; }

		public int DayOfWeek { get; set; }
	}

	public enum DiscountType
	{
		Flat,
		Unit,
		Weekday
	}
}
