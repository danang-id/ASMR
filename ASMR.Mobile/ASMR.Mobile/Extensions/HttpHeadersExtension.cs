using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASMR.Common.Constants;
using ASMR.Mobile.Common;
using ASMR.Mobile.Services;

namespace ASMR.Mobile.Extensions
{
	public static class HttpHeadersExtension
	{
		private static Task AddApplicationToken(this HttpHeaders headers,
			string headerName,
			string storageKey)
        {
			return headers.AddApplicationToken(headerName, storageKey, value => value);
        }

		private static async Task AddApplicationToken(this HttpHeaders headers,
			string headerName,
			string storageKey,
			Func<string, string> valueModifier)
		{
			var value = await TokenManager.GetAsync(storageKey);
			
			Debug.WriteLineIf(!string.IsNullOrEmpty(value), 
				$"Header {headerName}: Read from Token Manager\n" +
				$"\tValue: {value}",
				nameof(HttpHeadersExtension));
			
			if (!string.IsNullOrEmpty(value))
			{
				headers.Add(headerName, valueModifier(value));
			}
		}
		
		public static async Task AddApplicationTokens(this HttpHeaders headers)
		{
			await headers.AddApplicationToken(AntiforgeryConstants.HeaderName,
				AntiforgeryConstants.RequestTokenCookieName);
			await headers.AddApplicationToken(ApplicationConstants.AuthenticationHeaderName,
				ApplicationConstants.JsonWebTokenStorageKey,
				value => $"Bearer {value}");
		}
	}
}