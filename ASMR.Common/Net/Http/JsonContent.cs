using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ASMR.Common.Constants;

namespace ASMR.Common.Net.Http;

public class JsonContent<TContent> : HttpContent
{
	private readonly TContent _value;

	public JsonContent(TContent value)
	{
		_value = value;

		Headers.ContentType = new MediaTypeHeaderValue("application/json");
	}

	protected override bool TryComputeLength(out long length)
	{
		length = -1;
		return false;
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		return JsonSerializer.SerializeAsync(stream, _value, JsonConstants.DefaultJsonSerializerOptions);
	}
}