using StoreItemCalculator.Lib.Models;
using System;

namespace StoreItemCalculator.Lib
{
	public interface IOrderService
	{
		Order PrepareOrder(Cart cart);
	}

	public class OrderService : IOrderService
	{
		private readonly IRepository repository;

		public OrderService(IRepository repository)
		{
			this.repository = repository;
		}

		public Order PrepareOrder(Cart cart)
		{
			var order = new Order();
			int counter = 0;
			foreach (var cartLineItem in cart.LineItems)
			{
				var product = repository.GetProduct(cartLineItem.ProductId);
				var orderLineItem = new OrderLineItem
				{
					ProductName = product.Name,
					Quantity = cartLineItem.Quantity,
					SerialNumber = ++counter,
					UnitPrice = product.UnitPrice,
					TotalPrice = product.UnitPrice * cartLineItem.Quantity
				};

				if(product.Discount != null)
				{
					if(product.Discount.Type == DiscountType.Flat)
					{
						orderLineItem.DiscountAmount = product.Discount.Percent / 100 * orderLineItem.TotalPrice;
					}
					else if(product.Discount.Type == DiscountType.Unit)
					{
						if(orderLineItem.Quantity > product.Discount.UnitsNeeded)
						{
							orderLineItem.DiscountAmount = (orderLineItem.Quantity/ product.Discount.UnitsNeeded) * product.Discount.UnitsFree * product.UnitPrice;
						}
					}

					orderLineItem.DiscountDescription = $"{product.Discount.Type} Discount";
					orderLineItem.TotalPrice -= orderLineItem.DiscountAmount;
				}

				order.TotalPrice += orderLineItem.TotalPrice;
				order.Date = DateTime.UtcNow;
				order.LineItems.Add(orderLineItem);
			}

			return order;
		}
	}
}
