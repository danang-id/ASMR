//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// HttpStatusHandlerExtension.cs
//

using ASMR.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ASMR.Web.Extensions;

public static class HttpStatusHandlerExtension
{
	public static IApplicationBuilder UseHttpStatusHandler(this IApplicationBuilder app)
	{
		app.UseMiddleware<HttpStatusHandlerMiddleware>();

		return app;
	}
}