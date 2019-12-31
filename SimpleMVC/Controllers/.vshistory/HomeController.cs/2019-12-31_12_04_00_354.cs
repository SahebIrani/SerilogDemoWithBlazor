using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SimpleMVC.Models;

namespace SimpleMVC.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(ILogger<HomeController> logger) => Logger = logger;
		public ILogger<HomeController> Logger { get; }

		[HttpGet("[action]/{name:alpha}")]
		public IActionResult Index([FromQuery] string name)
		{
			_logger.LogInformation("Hello, {Name} .. !!!!", name);

			return View();
		}

		public IActionResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
