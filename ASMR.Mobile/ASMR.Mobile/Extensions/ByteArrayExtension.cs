using System.Text.Json;
using ASMR.Common.Constants;

namespace ASMR.Mobile.Extensions
{
	public static class ByteArrayExtension
	{
		public static T DeserializeAsync<T>(this byte[] bytes)
		{
			return JsonSerializer.Deserialize<T>(bytes, JsonConstants.DefaultJsonSerializerOptions);
		}
	}
}