
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Web.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Extensions
{
	public static class HttpContextExtension
	{
		public static Task SignInAsync(this HttpContext context, User user)
		{
			var claims = new List<Claim> 
			{
				new(ClaimTypes.NameIdentifier, user.Id)
			};
			
			var roleClaims = user.Roles
				.Select(userRole => new Claim(ClaimTypes.Role, userRole.Role.ToString()));
			claims.AddRange(roleClaims);

			var claimsIdentity = new ClaimsIdentity(claims,
				CookieAuthenticationConstants.AuthenticationScheme,
				ClaimTypes.NameIdentifier,
				ClaimTypes.Role);
			return context.SignInAsync(CookieAuthenticationConstants.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
		}

		public static Task SignOutAsync(this HttpContext context)
		{
			return context.SignOutAsync(CookieAuthenticationConstants.AuthenticationScheme);
		}
	}
}