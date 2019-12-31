using System;

using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleBlazor
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
		}

		public void Configure(IComponentsApplicationBuilder app)
		{
			// Enabling ingestion on the ASP.NET Core server
			//app.UseSerilogIngestion();
			//app.UseSerilogRequestLogging();

			Serilog.Debugging.SelfLog.Enable(m => Console.Error.WriteLine(m));

			app.AddComponent<App>("app");
		}
	}
}
