using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ASMR.Common.Constants;

namespace ASMR.Mobile.Extensions
{
	public static class HttpResponseMessageExtension
	{
		public static async Task<TResponse> ToResponseModel<TResponse>(this HttpResponseMessage httpResponseMessage)
		{
			await using var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
			return await JsonSerializer.DeserializeAsync<TResponse>(stream, 
				JsonConstants.DefaultJsonSerializerOptions);
		}
	}
}