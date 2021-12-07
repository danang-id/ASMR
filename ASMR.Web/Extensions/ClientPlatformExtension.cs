//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// ClientPlatformExtension.cs
//

using ASMR.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ASMR.Web.Extensions;

public static class ClientPlatformExtension
{
	public static IApplicationBuilder UseClientPlatformVerification(this IApplicationBuilder app)
	{
		app.UseMiddleware<ClientPlatformMiddleware>();

		return app;
	}
}