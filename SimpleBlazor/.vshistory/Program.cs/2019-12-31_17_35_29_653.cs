using System;

using Microsoft.AspNetCore.Blazor.Hosting;

using Serilog;
using Serilog.Core;

namespace SimpleBlazor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var levelSwitch = new LoggingLevelSwitch();
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(levelSwitch)
				.Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
				.WriteTo.BrowserHttp(controlLevelSwitch: levelSwitch)
				.WriteTo.BrowserConsole()
				.CreateLogger();

			Log.Information("Hello, browser .. !!!!");

			try
			{
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "An exception occurred while creating the WASM host");
				throw;
			}
		}

		public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
			BlazorWebAssemblyHost.CreateDefaultBuilder()
				.UseBlazorStartup<Startup>();
	}
}
