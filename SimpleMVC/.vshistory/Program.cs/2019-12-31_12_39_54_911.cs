using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Formatting.Compact;

namespace SimpleMVC
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Debug()
				.WriteTo.Console(
					outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
				.WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/log.ndjson")
				.WriteTo.Seq("http://localhost:5341")
				.CreateLogger()
			;

			try
			{
				Log.Information("Starting up");
				await CreateHostBuilder(args).Build().RunAsync();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application start-up failed");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
