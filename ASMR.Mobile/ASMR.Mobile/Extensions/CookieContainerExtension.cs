using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASMR.Common.Constants;
using Xamarin.Essentials;

namespace ASMR.Mobile.Extensions
{
	public static class CookieContainerExtension
	{
		private static async Task<bool> SaveCookieIfExistsToStorage(this IEnumerable<Cookie> cookies, string name)
		{
			if (cookies is null)
			{
				return false;
			}

			var cookie = cookies.Where(cookie => cookie.Name == name).FirstOrDefault();
			try
			{
				if (cookie is null || cookie.Expired)
				{
					SecureStorage.Remove(name);
					return cookie is not null;
				}
				
				
				await SecureStorage.SetAsync(cookie.Name, cookie.Value);
			}
			catch (Exception)
			{
				if (cookie is null || cookie.Expired)
				{
					Preferences.Remove(name);
					return cookie is not null;
				}
				
				Preferences.Set(cookie.Name, cookie.Value);
			}
			
			return true;
		}
		private static async Task<Cookie> RetrieveCookieFromStorage(string name)
		{
			string value;
			try
			{
				value = await SecureStorage.GetAsync(name);
			}
			catch (Exception)
			{
				value = Preferences.Get(name, null);
			}

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
				Debug.WriteLine("CookieContainer: Found Authentication token in storage, added to request");
			}
			if (antiforgeryToken is not null)
			{
				cookieContainer.Add(uri, antiforgeryToken);
				Debug.WriteLine("CookieContainer: Found Antiforgery token in storage, added to request");
			}
			if (antiforgeryRequestToken is not null)
			{
				cookieContainer.Add(uri, antiforgeryRequestToken);
				Debug.WriteLine("CookieContainer: Found Antiforgery Request token in storage, added to request");
			}
		}
		public static async Task SaveApplicationTokens(this CookieContainer cookieContainer, Uri uri)
		{
			var cookies = cookieContainer.GetCookies(uri).Cast<Cookie>().ToList();
			
			Debug.WriteLineIf(await cookies.SaveCookieIfExistsToStorage(AuthenticationConstants.CookieName), 
				"CookieContainer: Found Authentication token in response, saving to storage");
			Debug.WriteLineIf(await cookies.SaveCookieIfExistsToStorage(AntiforgeryConstants.CookieName), 
				"CookieContainer: Found Antiforgery token in response, saving to storage");
			Debug.WriteLineIf(await cookies.SaveCookieIfExistsToStorage(AntiforgeryConstants.RequestTokenCookieName), 
				"CookieContainer: Found Antiforgery Request token in response, saving to storage");
		}
	}
}