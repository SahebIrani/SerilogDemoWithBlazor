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
			IDiagnosticContext diagnosticContext,
			IShoppingCartService shoppingCartService)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			DiagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
			ShoppingCartService = shoppingCartService ?? throw new ArgumentNullException(nameof(shoppingCartService));
		}

		public ILogger<HomeController> Logger { get; }
		public IDiagnosticContext DiagnosticContext { get; }
		public IShoppingCartService ShoppingCartService { get; }
		public LoggerProviderCollection LoggerProvider { get; } = new LoggerProviderCollection();

		static int _callCount;

		public void Index([FromQuery] string name = "SinjulMSBH")
		{
			Logger.LogInformation("_logger: LogInformation");
			Logger.LogWarning("_logger: LogWarning");
			Logger.LogError("_logger: LogError");
			Logger.LogCritical("_logger: LogCritical");

			Logger.LogInformation("Hello, {Name} .. !!!!", name);

			DiagnosticContext.Set("IndexCallCount", Interlocked.Increment(ref _callCount));

			IEnumerable<ILoggerProvider> providers = LoggerProvider.Providers;

			ShoppingCartService.AddItem("SinjulMSBH", 130);

			throw new Exception("SinjulMBSH");
		}

		public IActionResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
