using System.Text.Json;
using ASMR.Common.Constants;

namespace ASMR.Mobile.Extensions
{
	public static class ObjectExtension
	{
		public static string ToJsonString(this object obj)
		{
			return JsonSerializer.Serialize(obj, JsonConstants.IndentedJsonSerializerOptions);
		}
	}
}