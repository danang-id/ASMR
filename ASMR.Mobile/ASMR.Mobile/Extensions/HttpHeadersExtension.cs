using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASMR.Common.Constants;
using Xamarin.Essentials;

namespace ASMR.Mobile.Extensions
{
	public static class HttpHeadersExtension
	{
		private static async Task AddAntiforgeryRequestToken(this HttpHeaders headers)
		{
			string value;
			try
			{
				value = await SecureStorage.GetAsync(AntiforgeryConstants.RequestTokenCookieName);
			}
			catch (Exception)
			{
				value = Preferences.Get(AntiforgeryConstants.RequestTokenCookieName, null);
			}

			if (!string.IsNullOrEmpty(value))
			{
				headers.Add(AntiforgeryConstants.HeaderName, value);
			}
		}
		
		public static Task AddApplicationTokens(this HttpHeaders headers)
		{
			return headers.AddAntiforgeryRequestToken();
		}
	}
}