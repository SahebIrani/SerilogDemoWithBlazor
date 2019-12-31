using System;

using Serilog;

namespace SimpleMVC.SinjulMSBH
{
	public interface IShoppingCartService
	{
		void AddItem(string itemId, int quantity);
	}

	public class ShoppingCartService : IShoppingCartService
	{
		public void AddItem(string itemId, int quantity)
		{
			Log.ForContext<ShoppingCartService>()
				.Information("Adding {ItemId} x {Quantity} to cart .. !!!!", itemId, quantity);
		}
	}

	public class ShoppingCartService2
	{
		readonly Func<DBContext> _createDBContext;

		public ShoppingCartService2(Func<DBContext> createDBContext) => _createDBContext = createDBContext;

		public void AddItem(string itemId, int quantity)
		{
			Log.ForContext<ShoppingCartService>().Information("Adding {ItemId} x {Quantity} to cart", itemId, quantity);

			// Oops, this will leak, we really needed Func<Owned<DBContext>> here.
			// Find and update the current shopping cart :-)
			using var db = _createDBContext();
		}

		public class DBContext : IDisposable
		{
			public void Dispose() => Log.Error("Dispose context .. !!!!");
		}
	}
}
