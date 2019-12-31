using Microsoft.AspNetCore.Blazor.Hosting;

using Serilog;

namespace SimpleBlazor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(levelSwitch)
				.WriteTo.BrowserConsole()
				.WriteTo.BrowserHttp("https://logs.example.com")
				.CreateLogger();

			Log.Information("Hello, browser!");


			CreateHostBuilder(args).Build().Run();
		}

		public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
			BlazorWebAssemblyHost.CreateDefaultBuilder()
				.UseBlazorStartup<Startup>();
	}
}
