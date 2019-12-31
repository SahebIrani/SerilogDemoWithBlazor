using System;
using System.Diagnostics;
using System.Threading;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Extensions.Logging;

using SimpleMVC.Models;

namespace SimpleMVC.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(
			ILogger<HomeController> logger,
			IDiagnosticContext diagnosticContext,
			LoggerProviderCollection loggerProviderCollection)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			DiagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
			LoggerProviderCollection = loggerProviderCollection ?? throw new ArgumentNullException(nameof(loggerProviderCollection));
		}

		public ILogger<HomeController> Logger { get; }
		public IDiagnosticContext DiagnosticContext { get; }
		public LoggerProviderCollection LoggerProviderCollection { get; }

		static int _callCount;


		[HttpGet("[action]/{name:alpha}")]
		public IActionResult Index([FromQuery] string name)
		{
			Logger.LogInformation("Hello, {Name} .. !!!!", name);

			DiagnosticContext.Set("IndexCallCount", Interlocked.Increment(ref _callCount));

			return View();
		}

		public IActionResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
