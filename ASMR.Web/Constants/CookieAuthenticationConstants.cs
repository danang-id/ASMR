//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 6:21 AM
//
// CookieAuthenticationConstants.cs
//
using System;
using System.Net;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Constants
{
    public static class CookieAuthenticationConstants
    {
        public const string AccessDenied = "/error/denied";

        public const string AuthenticationScheme = "ASMR.CookieAuthentication";

        public const string ClaimIssuer = "ASMR.AuthManager";

        public const bool CookieHttpOnly = true;

        public const string CookieName = "ASMR.Auth-Session";

        public static readonly TimeSpan ExpireTimeSpan = TimeSpan.FromDays(1);

        public const string LoginPath = "/gate/signin";

        public const string LogoutPath = "/gate/signout";

        public const string ReturnUrlParameter = "forwardTo";

        public static async Task OnRedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var errorModel = new ResponseError(ErrorCodeConstants.NotAuthenticated,
                "You are not authenticated. Please sign in before continue.");
            await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
                JsonConstants.DefaultJsonSerializerOptions);
        }

        public static async Task OnRedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            var errorModel = new ResponseError(ErrorCodeConstants.NotAuthorized,
                "You are not authorized to access this resource.");
            await context.Response.WriteAsJsonAsync(new DefaultResponseModel(errorModel),
                JsonConstants.DefaultJsonSerializerOptions);
        }
    }
}
