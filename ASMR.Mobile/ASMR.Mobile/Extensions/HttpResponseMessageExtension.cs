using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ASMR.Common.Constants;

namespace ASMR.Mobile.Extensions
{
	public static class HttpResponseMessageExtension
	{
		public static CookieContainer GetCookieContainer(this HttpResponseMessage httpResponseMessage)
		{
			var cookieContainer = new CookieContainer();
			var uri = httpResponseMessage.RequestMessage.RequestUri;

			if (!httpResponseMessage.Headers.TryGetValues("Set-Cookie", out var cookies))
			{
				return cookieContainer;
			}

			var cookieString = string.Join(",", cookies);
			cookieContainer.SetCookies(uri, cookieString);
			return cookieContainer;
		}

		public static Task SaveApplicationTokens(this HttpResponseMessage httpResponseMessage)
		{
			var uri = httpResponseMessage.RequestMessage.RequestUri;
			return httpResponseMessage.GetCookieContainer().SaveApplicationTokens(uri);
		}
		
		public static async Task<TResponse> ToResponseModel<TResponse>(this HttpResponseMessage httpResponseMessage)
		{
			await using var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
			return await JsonSerializer.DeserializeAsync<TResponse>(stream, 
				JsonConstants.DefaultJsonSerializerOptions);
		}
	}
}