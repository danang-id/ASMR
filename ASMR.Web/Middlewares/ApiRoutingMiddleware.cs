//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 6:56 PM
//
// ApiRoutingMiddleware.cs
//

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Middlewares;

public class ApiRoutingMiddleware
{
	private readonly RequestDelegate _next;

	public ApiRoutingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (context.Request.Path.StartsWithSegments("/api"))
		{
			context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			return;
		}

		await _next(context);
	}
}