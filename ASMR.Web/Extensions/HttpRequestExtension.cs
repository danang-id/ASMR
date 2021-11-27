using System;
using ASMR.Core.Enumerations;
using Flurl;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Extensions;

public static class HttpRequestExtension
{
	private const string ClientPlatformKey = "clientPlatform";
	private const string ClientVersionKey = "clientVersion";

	public static Uri GetBaseUri(this HttpRequest request)
	{
		return new Uri($"{request.Scheme}://{request.Host}");
	}

	public static Url GetBaseUrl(this HttpRequest request)
	{
		return new Url($"{request.Scheme}://{request.Host}");
	}

	public static ClientPlatform GetClientPlatform(this HttpRequest request)
	{
		if (!request.Query.TryGetValue(ClientPlatformKey, out var queryValue))
		{
			return (ClientPlatform)(-1);
		}

		return queryValue.ToString().ToLower() switch
		{
			"android" => ClientPlatform.Android,
			"ios" => ClientPlatform.iOS,
			"web" => ClientPlatform.Web,
			_ => (ClientPlatform)(-1)
		};
	}

	public static Version GetClientVersion(this HttpRequest request)
	{
		return request.Query.TryGetValue(ClientVersionKey, out var query)
			? new Version(query)
			: null;
	}

	public static bool HasValidClientInformation(this HttpRequest request)
	{
		return request.HasValidClientPlatform() && request.HasValidClientVersion();
	}

	public static bool HasValidClientPlatform(this HttpRequest request)
	{
		return (int)request.GetClientPlatform() != -1;
	}

	public static bool HasValidClientVersion(this HttpRequest request)
	{
		return request.GetClientVersion() is not null;
	}
}