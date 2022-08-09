using StoreItemCalculator.Lib;
using StoreItemCalculator.Lib.Models;
using System;
using System.Globalization;

namespace StoreItemCalculator
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Init products and discounts repository, due to the focus being on design, i have used an in memory repository & not used any db/file.
			var storeRepo = new StoreRepository();
			var orderService = new OrderService(storeRepo);

			Console.WriteLine("Hello welcome to Store Price Calculator !!!");
			Console.WriteLine("Please press enter to view the list of products and discounts");

			Console.ReadLine();
			Console.WriteLine($"Id\t{"Name".PadRight(30)}\t\t\tPrice\tDiscount");
			Console.WriteLine(new string('*', 100));
			
			// Show products & display along with discounts
			var products = storeRepo.GetProducts(new[] { 1, 2, 3, 4, 5, 6, 7 });
			foreach (var product in products)
			{
				Console.WriteLine($"{product.Id}\t{product.Name.PadRight(30)}\t\t\t{product.UnitPrice.ToString("C")}\t{FormatDiscount(product.DiscountStrategy?.Discount)}");
			}
			Console.WriteLine();
			Console.WriteLine("Please choose products for your cart by entering product Id (1-7) followed by quantity(digits & <10)");

			// Capture the entries
			var orderDate = EnterADate("Enter the order date in the format yyyy-MM-dd");
			var noOfProductsToBuy = EnterANumber("Enter the no of products you want to buy", 10);
			var cart = new Cart { OrderDate = orderDate};
			for (int i = 0; i < noOfProductsToBuy; i++)
			{
				var productId = EnterANumber($"Product number {i+1} - Enter product Id", 7);
				var quantity = EnterANumber($"Product number {i+1} - Enter quantity", 10);
				cart.LineItems.Add(new CartLineItem { ProductId = productId, Quantity = quantity });
			}

			Console.WriteLine();
			Console.WriteLine("Prepping your order..........");
			Console.WriteLine();

			var order = orderService.PrepareOrder(cart);

			Console.WriteLine($"SlNo\t{"Name".PadRight(30)}\t\t\tUnit Price\tQuantity\tDiscount\tTotal Price");
			Console.WriteLine(new string('*', 115));
		
			// Display the order
			foreach (var lineItem in order.LineItems)
			{
				Console.WriteLine($"{lineItem.SerialNumber}\t{lineItem.ProductName.PadRight(30)}\t\t\t{lineItem.UnitPrice.ToString("C").PadRight(10)}\t{lineItem.Quantity.ToString().PadRight(10)}\t{lineItem.DiscountAmount.ToString("C").PadRight(10)}\t{lineItem.TotalPrice.ToString("C").PadRight(10)}");
			}

			Console.WriteLine();
			Console.WriteLine($"{"Order Date".PadRight(20)}\t{order.Date.ToString("yyyy-MM-dd")}");
			Console.WriteLine($"{"Weekday Discount".PadRight(20)}\t{(order.Discount > 0 ? $"You have a week day discount of {order.Discount.ToString("C")}" : "No discount on the order date")}");
			Console.WriteLine($"{"Order Total".PadRight(20)}\t{order.TotalPrice.ToString("C")}");
			Console.WriteLine();
			Console.WriteLine("Thanks for using Store Price Calculator");
			Console.ReadLine();
		}

		static int EnterANumber(string message, int upperBound)
		{
			Console.WriteLine(message + $" - should be between 1 & {upperBound}");
			int n;
			while (!int.TryParse(Console.ReadLine(), out n) || n <= 0 || n > upperBound)
			{
				Console.WriteLine("You entered an invalid number");
				Console.WriteLine(message + $" - should be between 1 & {upperBound}");
			}

			return n;
		}

		static DateTime EnterADate(string message)
		{
			Console.WriteLine(message);
			DateTime dateTime;
			while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, DateTimeStyles.None, out dateTime))
			{
				Console.WriteLine("You entered an invalid date");
				Console.WriteLine(message);
			}

			return dateTime;
		}

		static string FormatDiscount(Discount discount)
		{
			var discountMessage = string.Empty;
			if(discount != null)
			{
				if (discount.Type == DiscountType.Flat)
					discountMessage = $"Flat discount {discount.Percent}% off";
				else if (discount.Type == DiscountType.Unit)
					discountMessage = $"Buy {discount.UnitsNeeded} get {discount.UnitsFree} free";
			}
			
			return discountMessage;
		}
	}
}
