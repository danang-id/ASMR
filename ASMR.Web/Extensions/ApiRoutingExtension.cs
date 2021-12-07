//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// ApiRoutingExtension.cs
//

using ASMR.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ASMR.Web.Extensions;

public static class ApiRoutingExtension
{
	public static IApplicationBuilder UseApiRouting(this IApplicationBuilder app)
	{
		app.UseMiddleware<ApiRoutingMiddleware>();

		return app;
	}
}