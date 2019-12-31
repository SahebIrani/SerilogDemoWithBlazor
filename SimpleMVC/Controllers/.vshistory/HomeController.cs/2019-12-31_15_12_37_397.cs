using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Extensions.Logging;

using SimpleMVC.Models;
using SimpleMVC.SinjulMSBH;

namespace SimpleMVC.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(
			ILogger<HomeController> logger,
			IDiagnosticContext diagnosticContext)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			DiagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
		}

		public ILogger<HomeController> Logger { get; }
		public IDiagnosticContext DiagnosticContext { get; }
		public LoggerProviderCollection LoggerProviderCollection { get; }

		static int _callCount;

		public ViewResult Index([FromQuery] string name = "SinjulMSBH")
		{
			Logger.LogInformation("Hello, {Name} .. !!!!", name);

			DiagnosticContext.Set("IndexCallCount", Interlocked.Increment(ref _callCount));

			IEnumerable<ILoggerProvider> providers = new LoggerProviderCollection().Providers;

			Log.ForContext<ShoppingCartService>().Information("Adding {ItemId} x {Quantity} to cart", 13, 85);

			return View();
		}

		public IActionResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
