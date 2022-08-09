using StoreItemCalculator.Lib;
using StoreItemCalculator.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreItemCalculator
{
	internal class StoreRepository : IRepository
	{
		private List<Product> products;
		private List<Discount> discounts;

		public StoreRepository()
		{
			Init();
		}

		private void Init()
		{
			discounts = new List<Discount>
			{
				new Discount{ Id = 1, Type = DiscountType.Flat, Percent = 10 },
				new Discount { Id = 2, Type = DiscountType.Unit, UnitsNeeded = 3, UnitsFree = 1 },
				new Discount { Id = 3, Type = DiscountType.Flat, Percent = 5 },
				new Discount { Id = 3, Type = DiscountType.Weekday, DayOfWeek = 1, Percent = 2 },
				new Discount { Id = 3, Type = DiscountType.Weekday, DayOfWeek = 3, Percent = 5 }
			};

			products = new List<Product> {
				new Product
				{
					Id = 1,
					Name = "Thumbs up",
					UnitPrice = 20,
					DiscountStrategy = new FlatDiscountStrategy(discounts[0])
				},
				new Product
				{
					Id = 2,
					Name = "Toilet Cleaner",
					UnitPrice = 45,
					DiscountStrategy = new FlatDiscountStrategy(discounts[0])
				},
				new Product
				{
					Id = 3,
					Name = "Mango",
					UnitPrice = 80
				},
				new Product
				{
					Id = 4,
					Name = "Cooking Oil Bottle - 1 liter",
					UnitPrice = 180,
					DiscountStrategy = new UnitDiscountStrategy(discounts[1])
				},
				new Product
				{
					Id = 5,
					Name = "SUGAR",
					UnitPrice = 45
				},
				new Product
				{
					Id = 6,
					Name = "Tea",
					UnitPrice = 150,
					DiscountStrategy = new FlatDiscountStrategy(discounts[2])
				},
				new Product
				{
					Id = 7,
					Name = "Bulbs",
					UnitPrice = 100
				}
			};
		}

		public List<Discount> GetDiscounts(DiscountType discountType) 
			=> discounts.Where(x => x.Type == discountType).ToList();

		public List<Product> GetProducts(int[] ids)
			=> products.Where(x => ids.Contains(x.Id)).ToList();
	}
}
