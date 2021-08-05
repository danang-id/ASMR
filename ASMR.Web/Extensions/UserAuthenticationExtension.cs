using ASMR.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ASMR.Web.Extensions
{
	public static class UserAuthenticationExtension
	{
		public static IApplicationBuilder UseUserAuthentication(this IApplicationBuilder app)
		{
			app.UseMiddleware<UserAuthenticationMiddleware>();
			return app;
		}
	}
}