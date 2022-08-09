using StoreItemCalculator.Lib.Models;

namespace StoreItemCalculator.Lib
{
	public interface IProductDiscountStrategy
	{
		Discount Discount { get; }

		decimal CalculateDiscount(decimal unitPrice, CartLineItem cartLineItem);
	}

	public class FlatDiscountStrategy : IProductDiscountStrategy
	{
		public FlatDiscountStrategy(Discount discount)
		{
			Discount = discount;
		}

		public Discount Discount { get; private set; }

		public decimal CalculateDiscount(decimal unitPrice, CartLineItem cartLineItem) 
			=> Discount != null ? Discount.Percent / 100 * (cartLineItem.Quantity * unitPrice) : 0;
	}

	public class UnitDiscountStrategy : IProductDiscountStrategy
	{
		public UnitDiscountStrategy(Discount discount)
		{
			Discount = discount;
		}

		public Discount Discount { get; private set; }

		public decimal CalculateDiscount(decimal unitPrice, CartLineItem cartLineItem)
		{
			decimal discount = 0;
			if (Discount != null && cartLineItem.Quantity > Discount.UnitsNeeded)
			{
				discount = (cartLineItem.Quantity / Discount.UnitsNeeded) * Discount.UnitsFree * unitPrice;
			}

			return discount;
		}
	}
}
