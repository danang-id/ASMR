//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// ClientPlatformMiddleware.cs
//

using System.Threading.Tasks;
using ASMR.Common.Constants;
using ASMR.Core.Constants;
using ASMR.Core.Generic;
using ASMR.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Middlewares;

public class ClientPlatformMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ClientPlatformMiddleware> _logger;

	public ClientPlatformMiddleware(RequestDelegate next, ILogger<ClientPlatformMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (context.Request.Path.StartsWithSegments("/api/mediafile") ||
		    context.Request.Path.StartsWithSegments("/api/release"))
		{
			await _next(context);
			return;
		}


		if (!context.Request.Path.StartsWithSegments("/api"))
		{
			await _next(context);
			return;
		}

		if (!context.Request.HasValidClientInformation())
		{
			var errorModel = new ResponseError(ErrorCodeConstants.InvalidClientPlatform,
				"Client Information is invalid.");
			await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
				JsonConstants.DefaultJsonSerializerOptions);
			return;
		}

		var clientPlatform = context.Request.GetClientPlatform();
		var clientVersion = context.Request.GetClientVersion();
		_logger.LogInformation(@"Client Platform: {ClientPlatform} version {ClientVersion}",
			clientPlatform, clientVersion);

		await _next(context);
	}
}