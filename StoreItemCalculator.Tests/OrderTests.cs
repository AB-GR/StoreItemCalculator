using Moq;
using NUnit.Framework;
using StoreItemCalculator.Lib;
using StoreItemCalculator.Lib.Models;
using System.Collections.Generic;
using System.Linq;

namespace StoreItemCalculator.Tests
{
	public class OrderTests
	{
		private readonly IOrderService orderService;
		private readonly Mock<IRepository> mockRepository;
		private List<Product> products;

		public OrderTests()
		{
			mockRepository = new Mock<IRepository>();
			orderService = new OrderService(mockRepository.Object);
		}

		[SetUp]
		public void Setup()
		{
			InitProducts();
			mockRepository.Setup(x => x.GetProducts()).Returns(products);
			mockRepository.Setup(x => x.GetProduct(It.IsAny<int>())).Returns<int>((id) => GetProduct(id));
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

		[Test]
		public void PrepareOrderWithUnitDiscount()
		{
			var cart = new Cart();
			var productsWithUnitDiscount = GetProducts(discountType: DiscountType.Unit);
			foreach (var product in productsWithUnitDiscount)
			{
				cart.LineItems.Add(new CartLineItem { ProductId = product.Id, Quantity = 4 });
			}

			var order = orderService.PrepareOrder(cart);

			Assert.That(order.TotalPrice, Is.EqualTo(540));
		}

		private void InitProducts()
		{
			products = new List<Product> { 
				new Product
				{
					Id = 1,
					Name = "Thumbs up",
					UnitPrice = 20,
					Discount = new Discount{ Type = DiscountType.Flat, Percent = 10 }
				},
				new Product
				{
					Id = 2,
					Name = "Toilet Cleaner",
					UnitPrice = 45,
					Discount = new Discount{ Type = DiscountType.Flat, Percent = 10 }
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
					Discount = new Discount { Type = DiscountType.Unit, UnitsNeeded = 3, UnitsFree = 1 }
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
					Discount = new Discount{ Type = DiscountType.Flat, Percent = 5 }
				},
				new Product
				{
					Id = 7,
					Name = "Bulbs",
					UnitPrice = 100
				}
			};
		}

		private Product GetProduct(int productId)
		{
			return products.FirstOrDefault(x => x.Id == productId);
		}

		private List<Product> GetProducts(DiscountType? discountType = null)
		{
			return products.Where(x => x.Discount?.Type == discountType).ToList();
		}
	}
}
