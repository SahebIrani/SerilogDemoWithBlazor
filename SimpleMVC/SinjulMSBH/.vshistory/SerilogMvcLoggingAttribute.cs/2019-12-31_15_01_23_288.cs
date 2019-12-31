using System;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace SimpleMVC.SinjulMSBH
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class SerilogMvcLoggingAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			IDiagnosticContext diagnosticContext = context.HttpContext.RequestServices.GetService<IDiagnosticContext>();
			diagnosticContext.Set("ActionName", context.ActionDescriptor);
			diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
		}
	}
}