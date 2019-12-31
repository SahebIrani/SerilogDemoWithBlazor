
using Serilog;

namespace SimpleMVC.SinjulMSBH
{
	public interface IShoppingCartService
	{
		void AddItem(string itemId, int quantity) =>
			Log.ForContext<IShoppingCartService>()
				.Information("Adding {ItemId} x {Quantity} to cart .. !!!!", itemId, quantity);
	}
}