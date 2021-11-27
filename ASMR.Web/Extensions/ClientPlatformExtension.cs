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