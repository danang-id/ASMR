//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/9/2021 7:34 AM
//
// AntiforgeryMiddleware.cs
//

using System.Net;
using System.Threading.Tasks;
using ASMR.Common.Constants;
using ASMR.Core.Constants;
using ASMR.Core.Generic;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Middlewares;

public class AntiforgeryMiddleware
{
	private readonly RequestDelegate _next;

	public AntiforgeryMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IAntiforgery antiForgery)
	{
		if (!context.Response.HasStarted)
		{
			var tokenSet = antiForgery.GetAndStoreTokens(context);
			/*if (!string.IsNullOrEmpty(tokenSet.CookieToken))
			{
			    context.Response.Cookies.Append(AntiforgeryConstants.CookieName,
			        tokenSet.CookieToken,
			        new CookieOptions
			        {
			            HttpOnly = true,
			            SameSite = SameSiteMode.Strict,
			            Secure = true
			        });
			}*/
			if (!string.IsNullOrEmpty(tokenSet.RequestToken))
			{
				context.Response.Cookies.Append(AntiforgeryConstants.RequestTokenCookieName,
					tokenSet.RequestToken,
					new CookieOptions
					{
						HttpOnly = false,
						SameSite = SameSiteMode.Strict,
						Secure = true
					});
			}
		}

		if (await antiForgery.IsRequestValidAsync(context))
		{
			await _next(context);
			return;
		}

		context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

		if (context.Request.Path.StartsWithSegments("/api"))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.InvalidAntiforgeryToken,
				"Failed to process your request because your CSRF session has ended.");
			await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
				JsonConstants.DefaultJsonSerializerOptions);
			return;
		}

		await _next(context);
	}
}