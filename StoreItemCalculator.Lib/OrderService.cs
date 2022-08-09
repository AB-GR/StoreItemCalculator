using StoreItemCalculator.Lib.Models;
using System;
using System.Linq;

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
			var products = repository.GetProducts(cart.LineItems.Select(x => x.ProductId).ToArray());
			foreach (var cartLineItem in cart.LineItems)
			{
				var product = products.FirstOrDefault(x => x.Id == cartLineItem.ProductId);
				if(product == null)
				{
					continue;
				}

				var orderLineItem = new OrderLineItem
				{
					ProductName = product.Name,
					Quantity = cartLineItem.Quantity,
					SerialNumber = ++counter,
					UnitPrice = product.UnitPrice,
					TotalPrice = product.UnitPrice * cartLineItem.Quantity
				};

				if(product.DiscountStrategy != null)
				{
					orderLineItem.DiscountAmount = product.DiscountStrategy.CalculateDiscount(product.UnitPrice, cartLineItem);
					orderLineItem.DiscountDescription = $"{product.DiscountStrategy.Discount.Type} Discount";
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
