//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// NativeHttpClient.cs
//
using System;
using System.Net;
using System.Net.Http;

namespace ASMR.Common.Net.Http;

public class NativeHttpClient : HttpClient
{
	public static CookieContainer CookieContainer { get; } = new();

	private static object _defaultClientHandler;

	public static object DefaultClientHandler
	{
		get => _defaultClientHandler;
		set
		{
			if (_defaultClientHandler is not null)
			{
				return;
			}

			if (value is not HttpMessageHandler)
			{
				throw new Exception($"Incorrect type of {nameof(DefaultClientHandler)}");
			}

			_defaultClientHandler = value;
		}
	}

	public NativeHttpClient() : base((HttpMessageHandler)DefaultClientHandler)
	{
	}
}