using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASMR.Common.Net.Http;
using ASMR.Mobile.Common;
using ASMR.Mobile.Extensions;
using Flurl;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ASMR.Mobile.Services.BackEnd
{
	public class BackEndService 
	{
		private static readonly NativeHttpClient HttpClient = new ();

		protected BackEndService()
		{
		}
		
		protected static Task<HttpContent> GenerateHttpContentAsync<TRequest>(TRequest requestContent)
			where TRequest : class
		{
			return Task.FromResult<HttpContent>(requestContent is not null 
				? new JsonContent<TRequest>(requestContent) 
				: null);
		}

		private static async Task<HttpRequestMessage> GenerateHttpRequestMessageAsync(HttpMethod method, 
			string endpoint, HttpContent httpContent, 
			string acceptMimeType = "application/json",
			bool addApplicationTokens = true,
			bool addClientInformation = true,
			IDictionary<string, IEnumerable<string>> headers = null,
			QueryParamCollection queryParams = null)
		{
			headers ??= new Dictionary<string, IEnumerable<string>>();
			queryParams ??= new QueryParamCollection();
			
			if (addClientInformation)
			{
				queryParams.Add("clientPlatform", Device.RuntimePlatform);
				queryParams.Add("clientVersion", VersionTracking.CurrentVersion);
			}
			
			var requestUri = new BackEndUrl()
				.AppendPathSegments("api", Url.ParsePathSegments(endpoint))
				.SetQueryParams(queryParams)
				.ToUri();
			var httpRequestMessage = new HttpRequestMessage(method, requestUri);
			
			httpRequestMessage.Content = httpContent;
			httpRequestMessage.Headers.Accept
				.Add(new MediaTypeWithQualityHeaderValue(acceptMimeType));
			if (addApplicationTokens)
			{
				await httpRequestMessage.Headers.AddApplicationTokens();
			}

			foreach (var (key, value) in headers)
			{
				httpRequestMessage.Headers.Add(key, value);
			}
			
			return httpRequestMessage;
		}
		
		private static async Task ResetClientAsync()
		{
			NativeHttpClient.CookieContainer.Clear(BackEndUrl.BaseAddress);
			await NativeHttpClient.CookieContainer.AddApplicationTokens(BackEndUrl.BaseAddress);
		}
		
		protected virtual Task<TResponse> RequestAsync<TResponse>(HttpMethod method, string endpoint) 
			where TResponse : class
		{
			return RequestAsync<TResponse, object>(method, endpoint, null);
		}

		protected virtual Task<TResponse> RequestAsync<TResponse>(HttpMethod method, string endpoint, 
			object requestContent) 
			where TResponse : class
		{
			return RequestAsync<TResponse, object>(method, endpoint, requestContent);
		}

		protected virtual async Task<TResponse> RequestAsync<TResponse, TRequest>(HttpMethod method, string endpoint, 
			TRequest requestContent)
			where TResponse : class
			where TRequest : class
		{
			var httpContent = await GenerateHttpContentAsync(requestContent);
			return await RequestAsync<TResponse>(method, endpoint, httpContent);
		}
		
		protected virtual async Task<TResponse> RequestAsync<TResponse, TRequest>(HttpMethod method, string endpoint, 
			HttpContent httpContent)
			where TResponse : class
			where TRequest : class
		{
			var httpMessageRequest = await GenerateHttpRequestMessageAsync(method, endpoint, httpContent);
			return await RequestAsync<TResponse>(method, endpoint, httpMessageRequest);
		}

		protected virtual async Task<TResponse> RequestAsync<TResponse>(HttpMethod method, string endpoint, 
			HttpRequestMessage httpRequestMessage) 
			where TResponse : class
		{
			try
			{
				Debug.WriteLine($"{nameof(NativeHttpClient)}: Request Started", GetType().Name);
				await ResetClientAsync();
				
				using var httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);
				await httpResponseMessage.SaveApplicationTokens();
				
				var response = await httpResponseMessage.ToResponseModel<TResponse>();
				Debug.WriteLine($"{method} {endpoint}: {response}", GetType().Name);
				
				return response;
			}
			catch (Exception exception)
			{
				Debug.WriteLine(exception, GetType().Name);
				return exception.ToResponseModel<TResponse>();
			}
		}
		
		protected virtual Task<TResponse> GetAsync<TResponse>(string endpoint) 
			where TResponse : class
		{
			return RequestAsync<TResponse>(HttpMethod.Get, endpoint);
		}
		
		protected virtual Task<TResponse> PostAsync<TResponse>(string endpoint) 
			where TResponse : class
		{
			return RequestAsync<TResponse>(HttpMethod.Post, endpoint);
		}
		
		protected virtual Task<TResponse> PostAsync<TResponse>(string endpoint, object data) 
			where TResponse : class
		{
			return RequestAsync<TResponse>(HttpMethod.Post, endpoint, data);
		}

		protected virtual Task<TResponse> PostAsync<TResponse, TRequest>(string endpoint, TRequest data) 
			where TResponse : class
			where TRequest : class
		{
			return RequestAsync<TResponse, TRequest>(HttpMethod.Post, endpoint, data);
		}

		protected virtual Task<TResponse> PatchAsync<TResponse>(string endpoint) 
			where TResponse : class
		{
			return RequestAsync<TResponse>(HttpMethod.Patch, endpoint);
		}
		
		protected virtual Task<TResponse> PatchAsync<TResponse>(string endpoint, object data) 
			where TResponse : class
		{
			return RequestAsync<TResponse>(HttpMethod.Patch, endpoint, data);
		}

		protected virtual Task<TResponse> PatchAsync<TResponse, TRequest>(string endpoint, TRequest data) 
			where TResponse : class
			where TRequest : class
		{
			return RequestAsync<TResponse, TRequest>(HttpMethod.Patch, endpoint, data);
		}

		protected virtual Task<TResponse> PutAsync<TResponse>(string endpoint) 
			where TResponse : class
		{
			return RequestAsync<TResponse>(HttpMethod.Put, endpoint);
		}
		
		protected virtual Task<TResponse> PutAsync<TResponse>(string endpoint, object data) 
			where TResponse : class
		{
			return RequestAsync<TResponse>(HttpMethod.Put, endpoint, data);
		}

		protected virtual Task<TResponse> PutAsync<TResponse, TRequest>(string endpoint, TRequest data) 
			where TResponse : class
			where TRequest : class
		{
			return  RequestAsync<TResponse, TRequest>(HttpMethod.Put, endpoint, data);
		}
		
		protected virtual Task<TResponse> DeleteAsync<TResponse>(string endpoint) 
			where TResponse : class
		{
			return RequestAsync<TResponse>(HttpMethod.Delete, endpoint);
		}
	}
}