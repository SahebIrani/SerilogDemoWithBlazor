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
			Log.ForContext<ShoppingCartService>().Information("Adding {ItemId} x {Quantity} to cart", itemId, quantity);
		}
	}
}
