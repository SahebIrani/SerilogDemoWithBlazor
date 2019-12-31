using System;
using System.IO;
using System.Threading.Tasks;

using Autofac;
using Autofac.Analysis;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace SimpleMVC.SinjulMSBH
{
	public static class ProgramRunning
	{
		public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
			.AddEnvironmentVariables()
			.Build();

		public static async Task<IHostBuilder> RunWithSerilogAsync(this IHostBuilder hostBuilder)
		{
			Environment.SetEnvironmentVariable("SEQ_URL", "http://localhost:5341");
			string seqEnvironmentValue = Environment.GetEnvironmentVariable("SEQ_URL");

			using (Logger logger = new LoggerConfiguration()
				.ReadFrom.Configuration(Configuration)
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
				.MinimumLevel.Override("Autofac.Analysis", LogEventLevel.Warning)
				.Destructure.ToMaximumDepth(100)
				.Enrich.FromLogContext()
				.WriteTo.Debug()
				.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
				.WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/log.ndjson")
				.WriteTo.File(
				path: "log.txt",
				rollingInterval: RollingInterval.Day,
				retainedFileCountLimit: null,
				fileSizeLimitBytes: null,
				rollOnFileSizeLimit: true
				)
				.WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
				.CreateLogger())
			{
				Log.Logger = logger;

				try
				{
					Log.Information("Starting up before .. !!!!");

					hostBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
						.ReadFrom.Configuration(hostingContext.Configuration)
						.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
						.MinimumLevel.Override("Autofac.Analysis", LogEventLevel.Warning)
						.Destructure.ToMaximumDepth(100)
						.Enrich.FromLogContext()
						.WriteTo.Debug()
						.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
						.WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/log.ndjson")
						.WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
					);


					ContainerBuilder containerBuilder = new ContainerBuilder();
#if DEBUG
					containerBuilder.RegisterModule(new AnalysisModule(logger));
#endif

					Log.Warning("Starting up after .. !!!!");

					await hostBuilder.Build().RunAsync();
				}
				catch (Exception ex)
				{
					Log.Fatal(ex, "Host terminated unexpectedly");
					Log.Fatal(ex, "Application start-up failed");
				}
				finally
				{
					Log.CloseAndFlush();
				}
			}

			return hostBuilder;
		}
	}
}
