using System.Diagnostics;

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
			Logger = logger;
			DiagnosticContext = diagnosticContext;
		}

		public ILogger<HomeController> Logger { get; }
		public IDiagnosticContext DiagnosticContext { get; }

		[HttpGet("[action]/{name:alpha}")]
		public IActionResult Index([FromQuery] string name)
		{
			Logger.LogInformation("Hello, {Name} .. !!!!", name);

			return View();
		}

		public IActionResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
