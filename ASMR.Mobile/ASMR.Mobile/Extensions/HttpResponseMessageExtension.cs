using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASMR.Mobile.Extensions
{
	public static class HttpResponseMessageExtension
	{
		public static CookieCollection GetCookieCollection(this HttpResponseMessage httpResponseMessage)
		{
			var cookieContainer = new CookieContainer();
			var uri = httpResponseMessage.RequestMessage.RequestUri;

			if (!httpResponseMessage.Headers.TryGetValues("Set-Cookie", out var cookies))
			{
				return new CookieCollection();
			}

			var cookieString = string.Join(",", cookies);
			cookieContainer.SetCookies(uri, cookieString);
			return cookieContainer.GetCookies(uri);
		}
		
		public static async Task<TResponse> Cast<TResponse>(this HttpResponseMessage httpResponseMessage)
		{
			var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();
			return bytes.DeserializeAsync<TResponse>();
		}
	}
}