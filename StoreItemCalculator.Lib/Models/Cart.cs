using System;
using System.Collections.Generic;

namespace StoreItemCalculator.Lib.Models
{
	public class Cart
	{
		public List<CartLineItem> LineItems { get; private set; }

		public DateTime? OrderDate { get; set; }

		public Cart()
		{
			LineItems = new List<CartLineItem>();
		}

		public Cart(List<CartLineItem> lineItems)
		{
			LineItems = lineItems;
		}
	}
}
