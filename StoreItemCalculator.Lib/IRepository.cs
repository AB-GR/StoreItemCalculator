using StoreItemCalculator.Lib.Models;
using System.Collections.Generic;

namespace StoreItemCalculator.Lib
{
	public interface IRepository
	{
		List<Product> GetProducts();

		Product GetProduct(int id);
	}
}
