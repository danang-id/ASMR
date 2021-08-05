//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/9/2021 8:01 AM
//
// HttpStatusHandlerMiddleware.cs
//
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Generic;
using ASMR.Web.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Middlewares
{
    public class HttpStatusHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusHandlerMiddleware> _logger;

        public HttpStatusHandlerMiddleware(RequestDelegate next, ILogger<HttpStatusHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private async Task OnNotFound(HttpContext context)
        {
            if (context.Response.HasStarted || context.Response.ContentLength > 0)
            {
                return;
            }

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.EndpointNotFound,
                    "The end-point you are looking for is not found.");
                await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
                    JsonConstants.DefaultJsonSerializerOptions);
                return;
            }

            context.Request.Path = "/error/pagenotfound";
            await _next(context);
        }

        private static async Task OnMethodNotAllowed(HttpContext context)
        {
            if (context.Response.HasStarted || context.Response.ContentLength > 0)
            {
                return;
            }

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequestMethodNotAllowed,
                    "The method you used for requesting this end-point is not allowed.");
                await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
                    JsonConstants.DefaultJsonSerializerOptions);
            }
        }

        private static async Task OnUnsupportedMediaType(HttpContext context)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequestMediaTypeNotSupported,
                    "The media type used in your request is not supported.");
                await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
                    JsonConstants.DefaultJsonSerializerOptions);
            }
        }

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env)
        {
            try
            {
                await _next(context);

                switch (context.Response.StatusCode)
                {
                    case (int)HttpStatusCode.NotFound:
                        await OnNotFound(context);
                        break;
                    case (int)HttpStatusCode.MethodNotAllowed:
                        await OnMethodNotAllowed(context);
                        break;
                    case (int)HttpStatusCode.UnsupportedMediaType:
                        await OnUnsupportedMediaType(context);
                        break;
                }
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                if (!context.Request.Path.StartsWithSegments("/api"))
                {
                    throw;
                }
                
                var errorModels = new Collection<ResponseError>();
                if (env.IsDevelopment())
                {
                    // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                    _logger.LogError(exception, exception.Message);
                    errorModels.Add(new ResponseError(ErrorCodeConstants.GenericServerError,
                        exception.Message));
                    if (exception.InnerException is not null)
                    {
                        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                        _logger.LogError(exception.InnerException, exception.InnerException.Message);
                        errorModels.Add(new ResponseError(ErrorCodeConstants.GenericServerError,
                            exception.InnerException.Message));
                    }
                }
                else
                {
                    errorModels.Add(new ResponseError(ErrorCodeConstants.GenericServerError,
                        "Something went wrong."));
                }
                
                await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModels),
                    JsonConstants.DefaultJsonSerializerOptions);
            }
        }
    }
}
