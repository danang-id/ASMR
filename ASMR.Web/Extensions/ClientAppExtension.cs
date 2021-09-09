using Flurl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ASMR.Web.Extensions
{
	public static class ClientAppExtension
	{
		public static IApplicationBuilder UseClientApp(this IApplicationBuilder app, IWebHostEnvironment environment)
		{
			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (environment.IsDevelopment())
				{
					spa.UseProxyToSpaDevelopmentServer(new Url("http://127.0.0.1:3000/"));
				}
			});

			return app;
		}
	}
}