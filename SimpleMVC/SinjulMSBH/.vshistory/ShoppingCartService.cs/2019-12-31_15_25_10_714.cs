using System;

using Serilog;

namespace SimpleMVC.SinjulMSBH
{
	public interface IShoppingCartService
	{
		void AddItem(string itemId, int quantity)
		{
			Log.ForContext<ShoppingCartService>()
				.Information("Adding {ItemId} x {Quantity} to cart .. !!!!", itemId, quantity);
		}
	}

	public class ShoppingCartServiceFake
	{
		readonly Func<DBContext> _createDBContext;

		public ShoppingCartServiceFake(Func<DBContext> createDBContext) => _createDBContext = createDBContext;

		public void AddItem(string itemId, int quantity)
		{
			Log.ForContext<ShoppingCartServiceFake>()
				.Information("Adding {ItemId} x {Quantity} to cart", itemId, quantity);

			// Oops, this will leak, we really needed Func<Owned<DBContext>> here.
			// Find and update the current shopping cart :-)
			using var db = _createDBContext();
		}

		public class DBContext : IDisposable
		{
			public void Dispose() => Log.Information("Dispose DBContext .. !!!!");
		}
	}
}
