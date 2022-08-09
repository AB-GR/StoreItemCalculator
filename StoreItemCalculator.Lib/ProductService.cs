using StoreItemCalculator.Lib.Models;
using System.Collections.Generic;

namespace StoreItemCalculator.Lib
{
	public interface IProductService
	{
		Product GetProduct(int productId);
		List<Product> GetProducts();
	}

	public class ProductService : IProductService
	{
		public Product GetProduct(int productId)
		{
			return new Product { Id = productId, UnitPrice = 10 };
		}

		public List<Product> GetProducts()
		{
			return new List<Product> { new Product() };
		}
	}
}
