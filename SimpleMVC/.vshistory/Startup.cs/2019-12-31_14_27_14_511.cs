using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using SimpleMVC.SinjulMSBH;

namespace SimpleMVC
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services) =>
			services.AddControllersWithViews(configure => configure.Filters.Add<SerilogMvcLoggingAttribute>());

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			// Write streamlined request completion events, instead of the more verbose ones from the framework.
			// To use the default framework request logging instead, remove this line and set the "Microsoft"
			// level in appsettings.json to "Information".
			// Add this line; you'll need `using Serilog;` up the top, too
			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
