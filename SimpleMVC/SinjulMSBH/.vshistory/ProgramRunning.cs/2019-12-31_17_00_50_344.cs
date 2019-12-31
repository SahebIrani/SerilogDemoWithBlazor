using System;
using System.IO;
using System.Threading.Tasks;

using Autofac;
using Autofac.Analysis;

using Loggly;
using Loggly.Config;

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
				.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
				.MinimumLevel.Override("Autofac.Analysis", LogEventLevel.Warning)
				.Destructure.ToMaximumDepth(100)
				.Enrich.FromLogContext()
				.WriteTo.Debug()
				.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
				//.WriteTo.File(new RenderedCompactJsonFormatter(), "/Logs/audit.txt")
				.WriteTo.File(
					path: "Logs/audit.txt",
					rollingInterval: RollingInterval.Day,
					retainedFileCountLimit: 8,
					fileSizeLimitBytes: 107341824,
					rollOnFileSizeLimit: true,
					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
					shared: true
				)
				.WriteTo.Loggly()
				.WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
				.WriteTo.Async(a => a.RollingFile("Logs/SimpleMVC-{Date}.log", LogEventLevel.Information))
				.CreateLogger())
			{
				Log.Logger = logger;

				try
				{
					Log.Warning("Starting up .. !!!!");

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

					LogglySettings logglySettings = new LogglySettings();
					Configuration.GetSection("Serilog:Loggly").Bind(logglySettings);
					SetupLogglyConfiguration(logglySettings);

					Log.Information("Started Succesfully .. !!!!");

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
		private static void SetupLogglyConfiguration(LogglySettings logglySettings)
		{
			ILogglyConfig config = LogglyConfig.Instance;
			config.CustomerToken = logglySettings.CustomerToken;
			config.ApplicationName = logglySettings.ApplicationName;
			config.Transport = new TransportConfiguration()
			{
				EndpointHostname = logglySettings.EndpointHostname,
				EndpointPort = logglySettings.EndpointPort,
				LogTransport = logglySettings.LogTransport
			};
			config.ThrowExceptions = logglySettings.ThrowExceptions;
			config.TagConfig.Tags.AddRange(new ITag[]{
				new ApplicationNameTag {Formatter = "Application-{0}"},
				new HostnameTag { Formatter = "Host-{0}" }
			});
		}
	}
}
