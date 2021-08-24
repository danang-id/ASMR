using System.Threading.Tasks;
using ASMR.Common.Constants;
using ASMR.Core.Constants;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Middlewares
{
    public class ClientPlatformMiddleware
    {
        private const string ClientPlatformKey = "clientPlatform";
        private const string ClientVersionKey = "clientVersion";

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

            var containsClientPlatform = context.Request.Query.TryGetValue(ClientPlatformKey, out var clientPlatformQuery);
            var containsClientVersion = context.Request.Query.TryGetValue(ClientVersionKey, out var clientVersionQuery);

            if (!containsClientPlatform && !containsClientVersion)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.InvalidClientPlatform,
                    "Client Platform Information is invalid.");
                await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
                    JsonConstants.DefaultJsonSerializerOptions);
                return;
            }

            var clientPlatform = clientPlatformQuery.ToString() switch
            {
                "Android" => ClientPlatform.Android,
                "iOS" => ClientPlatform.iOS,
                "Web" => ClientPlatform.Web,
                _ => (ClientPlatform) (-1)
            };
            if ((int)clientPlatform == -1)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.InvalidClientPlatform,
                    "Client Platform Information is invalid.");
                await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
                    JsonConstants.DefaultJsonSerializerOptions);
                return;
            }

            // var clientVersion = new Version(clientVersionQuery.ToString());

            _logger.LogInformation($"Client Platform: {clientPlatformQuery} version {clientVersionQuery}");

            await _next(context);
        }
    }
}
