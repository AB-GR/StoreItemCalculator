using System;
using System.Collections.Generic;

namespace StoreItemCalculator.Lib.Models
{
	public class Order
	{
		public Order()
		{
			LineItems = new List<OrderLineItem>();
		}

		public List<OrderLineItem> LineItems { get; set; }

		public decimal Discount { get; set; }

		public string DiscountDescription { get; set; }

		public DateTime Date { get; set; }

		public decimal TotalPrice { get; set; }
	}
}
