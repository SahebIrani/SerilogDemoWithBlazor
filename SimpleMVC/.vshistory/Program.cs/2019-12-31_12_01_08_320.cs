using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;


namespace SimpleMVC
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger()
			;

			try
			{
				Log.Information("Starting up");
				CreateHostBuilder(args).Build().Run();
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application start-up failed");
			}
#pragma warning restore CA1031 // Do not catch general exception types
			finally
			{
				Log.CloseAndFlush();
			}

			CreateHostBuilder(args).Build().Run();
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
