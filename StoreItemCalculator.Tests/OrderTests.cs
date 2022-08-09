using Moq;
using NUnit.Framework;
using StoreItemCalculator.Lib;
using StoreItemCalculator.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreItemCalculator.Tests
{
	public class OrderTests
	{
		private readonly IOrderService orderService;
		private readonly Mock<IRepository> mockRepository;
		private List<Product> products;
		private	List<Discount> discounts;

		public OrderTests()
		{
			mockRepository = new Mock<IRepository>();
			orderService = new OrderService(mockRepository.Object);
		}

		[SetUp]
		public void Setup()
		{
			Init();
			mockRepository.Setup(x => x.GetProducts(It.IsAny<int[]>())).Returns<int[]>((ids) => GetProducts(ids));
			mockRepository.Setup(x => x.GetDiscounts(It.IsAny<DiscountType>())).Returns(new List<Discount>());
		}

		[Test]
		public void PrepareOrderWithoutDiscount()
		{
			var cart = new Cart();
			var productsWithoutDiscount = GetProducts();
			foreach (var product in productsWithoutDiscount)
			{
				cart.LineItems.Add(new CartLineItem { ProductId = product.Id, Quantity = 1 });
			}

			var order = orderService.PrepareOrder(cart);

			Assert.That(order.TotalPrice, Is.EqualTo(225));
		}

		[Test]
		public void PrepareOrderWithFlatDiscount()
		{
			var cart = new Cart();
			var productsWithFlatDiscount = GetProducts(discountType: DiscountType.Flat);
			foreach (var product in productsWithFlatDiscount)
			{
				cart.LineItems.Add(new CartLineItem { ProductId = product.Id, Quantity = 1 });
			}

			var order = orderService.PrepareOrder(cart);

			Assert.That(order.TotalPrice, Is.EqualTo(201));
		}

		[TestCase(3, 540)]
		[TestCase(4, 540)]
		[TestCase(5, 720)]
		[TestCase(6, 900)]
		[TestCase(7, 1080)]
		public void PrepareOrderWithUnitDiscount(int quantity, decimal expectedTotal)
		{
			var cart = new Cart();
			var productsWithUnitDiscount = GetProducts(discountType: DiscountType.Unit);
			foreach (var product in productsWithUnitDiscount)
			{
				cart.LineItems.Add(new CartLineItem { ProductId = product.Id, Quantity = quantity});
			}

			var order = orderService.PrepareOrder(cart);

			Assert.That(order.TotalPrice, Is.EqualTo(expectedTotal));
		}

		[TestCase(true)]
		[TestCase(false)]
		public void PrepareOrderWithDayDiscount(bool dayWithDiscount)
		{
			mockRepository.Setup(x => x.GetDiscounts(It.IsAny<DiscountType>())).Returns<DiscountType>((discountType) => GetDiscounts(discountType));

			var cart = new Cart { OrderDate = DateTime.Parse(dayWithDiscount ? "2022/08/08" : "2022/08/09") };
			var productsWithoutDiscount = GetProducts();
			foreach (var product in productsWithoutDiscount)
			{
				cart.LineItems.Add(new CartLineItem { ProductId = product.Id, Quantity = 1 });
			}

			var order = orderService.PrepareOrder(cart);

			Assert.That(order.TotalPrice, Is.EqualTo(dayWithDiscount ? 220.5 : 225));
		}

		[Test]
		public void PrepareOrderForAllProducts()
		{
			var cart = new Cart();
			var allProducts = GetProducts(fetchAll: true);
			foreach (var product in allProducts)
			{
				cart.LineItems.Add(new CartLineItem { ProductId = product.Id, Quantity = 1 });
			}

			var order = orderService.PrepareOrder(cart);

			Assert.That(order.TotalPrice, Is.EqualTo(606));
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

		private List<Product> GetProducts(int[] productIds) 
			=> products.Where(x => productIds.Contains(x.Id)).ToList();

		private List<Discount> GetDiscounts(DiscountType discountType) 
			=> discounts.Where(x => x.Type == discountType).ToList();

		private List<Product> GetProducts(DiscountType? discountType = null, bool fetchAll = false) 
			=> products.Where(x => fetchAll || x.DiscountStrategy?.Discount?.Type == discountType).ToList();
	}
}
