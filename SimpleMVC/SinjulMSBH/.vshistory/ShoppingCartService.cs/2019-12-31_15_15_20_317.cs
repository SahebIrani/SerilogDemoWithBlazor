using Serilog;

namespace SimpleMVC.SinjulMSBH
{
	public interface IShoppingCartService
	{
		void AddItem(string name, int quantity);
	}

	public class ShoppingCartService : IShoppingCartService
	{
		public void AddItem(string name, int quantity)
		{
			Log.ForContext<ShoppingCartService>().Information("Adding {ItemId} x {Quantity} to cart", 13, 85);
		}
	}
}
