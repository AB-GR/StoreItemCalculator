using StoreItemCalculator.Lib.Models;

namespace StoreItemCalculator.Lib
{
	// Using strategy pattern to inject a discount calculation strategy when loading from the data layer
	// This injection would ideally be done on the business side
	// Using this pattern is advantageous as in future additional strategies can be added and also modifications will
	// happen in the individual strategy without affecting the Order Service
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
				var noOfBlocks = cartLineItem.Quantity / Discount.UnitsNeeded;

				while(noOfBlocks > 0)
				{
					var remaining = cartLineItem.Quantity - (noOfBlocks * Discount.UnitsNeeded);

					if (remaining >= noOfBlocks)
					{
						discount = noOfBlocks * Discount.UnitsFree * unitPrice;
						break;
					}
					else
					{
						noOfBlocks--;
					}
				}
			}

			return discount;
		}
	}
}
