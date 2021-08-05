using System.Threading.Tasks;
using ASMR.Web.Extensions;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Middlewares
{
	public class UserAuthenticationMiddleware
	{
		private readonly RequestDelegate _next;

		public UserAuthenticationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IUserService userService)
		{
			if (userService is null)
			{
				await _next(context);
				return;
			}
			
			var identity = context.User.Identity;
			if (identity is null || !identity.IsAuthenticated)
			{
				await _next(context);
				return;
			}

			var user = await userService.GetUserById(identity.Name);
			if (user is null)
			{
				await _next(context);
				return;
			}

			await context.SignOutAsync();
			await context.SignInAsync(user);
			await _next(context);
		}
		
	}
}