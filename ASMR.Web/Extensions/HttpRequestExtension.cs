using System;
using ASMR.Core.Enumerations;
using Flurl;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Extensions
{
	public static class HttpRequestExtension
	{
		private static readonly string ClientPlatformKey = "clientPlatform";
		private static readonly string ClientVersionKey = "clientVersion";
		
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
			if (!request.Query.TryGetValue(ClientPlatformKey, out var query))
			{
				return (ClientPlatform)(-1);
			}
			
			return query.ToString() switch
			{
				"Android" => ClientPlatform.Android,
				"iOS" => ClientPlatform.iOS,
				"Web" => ClientPlatform.Web,
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
}