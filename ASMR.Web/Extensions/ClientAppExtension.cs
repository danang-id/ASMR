//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// ClientAppExtension.cs
//

using ASMR.Web.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ASMR.Web.Extensions;

public static class ClientAppExtension
{
	public static IApplicationBuilder UseClientApp(this IApplicationBuilder app, IWebHostEnvironment environment)
	{
		app.UseSpa(spa =>
		{
			spa.Options.SourcePath = ClientAppConstants.SourcePath;

			if (environment.IsDevelopment())
			{
				spa.UseProxyToSpaDevelopmentServer(ClientAppConstants.DevelopmentServerUrl);
			}
		});

		return app;
	}
}