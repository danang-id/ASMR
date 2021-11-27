//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/9/2021 7:37 AM
//
// AntiforgeryExtension.cs
//

using ASMR.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ASMR.Web.Extensions;

public static class AntiforgeryExtension
{
	public static IApplicationBuilder UseAntiforgery(this IApplicationBuilder app)
	{
		app.UseMiddleware<AntiforgeryMiddleware>();

		return app;
	}
}