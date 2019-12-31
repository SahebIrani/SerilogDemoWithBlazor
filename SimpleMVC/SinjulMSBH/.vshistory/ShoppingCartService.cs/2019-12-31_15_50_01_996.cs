
using Serilog;

namespace SimpleMVC.SinjulMSBH
{
	public class ShoppingCartService
	{
		public void AddItem(string itemId, int quantity)
			=> Log.ForContext<ShoppingCartService>()
				.Information("Adding {ItemId} x {Quantity} to cart .. !!!!", itemId, quantity);
	}
}


