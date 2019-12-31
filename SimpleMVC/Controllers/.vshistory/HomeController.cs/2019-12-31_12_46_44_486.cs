using System;
using System.Diagnostics;
using System.Threading;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Serilog;

using SimpleMVC.Models;

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
