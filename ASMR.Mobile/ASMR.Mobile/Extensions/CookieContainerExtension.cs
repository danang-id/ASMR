using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				Debug.WriteLine($"Cookie {name}: Removed from Token Manager",
					nameof(CookieContainerExtension));
				return;
            }

			var currentValue = await TokenManager.GetAsync(name);
			if (currentValue != cookie.Value)
			{
				await TokenManager.SetAsync(name, cookie.Value);
				Debug.WriteLine($"Cookie {name}: Written to Token Manager\n" +
				                $"\tValue: {cookie.Value}", nameof(CookieContainerExtension));
			}
		}
		private static async Task<Cookie> RetrieveCookieFromStorage(string name)
		{
			var value = await TokenManager.GetAsync(name);

			Debug.WriteLineIf(!string.IsNullOrEmpty(value), 
				$"Cookie {name}: Read from Token Manager\n" +
				$"\tValue: {value}",
				nameof(CookieContainerExtension));
			
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
		public static async Task SaveApplicationTokens(this CookieContainer cookieContainer, Uri uri)
		{
			var cookies = cookieContainer.GetCookies(uri).Cast<Cookie>()
				.AsQueryable();

			await cookies.SaveCookieIfExistsToStorage(AuthenticationConstants.CookieName);
			await cookies.SaveCookieIfExistsToStorage(AntiforgeryConstants.CookieName);
			await cookies.SaveCookieIfExistsToStorage(AntiforgeryConstants.RequestTokenCookieName);
		}
	}
}