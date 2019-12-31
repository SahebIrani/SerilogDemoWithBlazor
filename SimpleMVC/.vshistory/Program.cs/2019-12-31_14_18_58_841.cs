using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

using SimpleMVC.SinjulMSBH;

namespace SimpleMVC
{
	public class Program
	{
		public static async Task Main(string[] args) => await CreateHostBuilder(args).RunWithSerilogAsync();

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
					.ReadFrom.Configuration(hostingContext.Configuration)
					.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
					.Enrich.FromLogContext()
					.WriteTo.Debug()
					.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
					.WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/log.ndjson")
					.WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
				)
			;
	}
}
