using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASMR.Common.Constants;
using ASMR.Mobile.Services;

namespace ASMR.Mobile.Extensions
{
	public static class CookieContainerExtension
	{
		private static async Task SaveCookieIfExistsToStorage(this IEnumerable<Cookie> cookies, string name)
		{
			if (cookies is null)
			{
				return;
			}

			var cookie = cookies.Where(cookie => cookie.Name == name).FirstOrDefault();
			var removeStorage = cookie is null || cookie.Expired;
			if (removeStorage)
            {
				TokenManager.Remove(name);
            }
			else
            {
				await TokenManager.SetAsync(name, cookie.Value);
			}
		}
		private static async Task<Cookie> RetrieveCookieFromStorage(string name)
		{
			var value = await TokenManager.GetAsync(name);

			return !string.IsNullOrEmpty(value) ? new Cookie(name, value) : null;
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
		public static async Task SaveApplicationTokens(this CookieContainer cookieContainer, Uri uri)
		{
			var cookies = cookieContainer.GetCookies(uri).Cast<Cookie>().ToList();

			await cookies.SaveCookieIfExistsToStorage(AuthenticationConstants.CookieName);
			await cookies.SaveCookieIfExistsToStorage(AntiforgeryConstants.CookieName);
			await cookies.SaveCookieIfExistsToStorage(AntiforgeryConstants.RequestTokenCookieName);
		}
	}
}