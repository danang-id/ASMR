using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASMR.Common.Constants;
using ASMR.Mobile.Common.Security;

namespace ASMR.Mobile.Extensions
{
	public static class CookieContainerExtension
	{
		private static async Task<Cookie> RetrieveCookieFromStorage(string name)
		{
			var value = await TokenManager.GetAsync(name);
			
			return !string.IsNullOrEmpty(value) ? new Cookie(name, value) : null;
		}

		public static void Clear(this CookieContainer cookieContainer, Uri uri)
		{
			foreach (var cookie in cookieContainer.GetCookies(uri).Cast<Cookie>())
			{
				cookie.Expired = true;
			}
		}
		public static async Task AddApplicationTokens(this CookieContainer cookieContainer, Uri uri)
		{
			var authToken = await RetrieveCookieFromStorage(AuthenticationConstants.CookieName);
			var antiforgeryToken = await RetrieveCookieFromStorage(AntiforgeryConstants.CookieName);
			var antiforgeryRequestToken = await RetrieveCookieFromStorage(AntiforgeryConstants.RequestTokenCookieName);

			if (authToken is not null)
			{
				cookieContainer.Add(uri, authToken);
			}
			if (antiforgeryToken is not null)
			{
				cookieContainer.Add(uri, antiforgeryToken);
			}
			if (antiforgeryRequestToken is not null)
			{
				cookieContainer.Add(uri, antiforgeryRequestToken);
			}
		}
	}
}