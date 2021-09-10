using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASMR.Common.Net.Http;
using ASMR.Core.Constants;
using ASMR.Core.Generic;
using ASMR.Mobile.Common.Net;
using ASMR.Mobile.Extensions;
using ASMR.Mobile.Services.Abstraction;
using Flurl;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ASMR.Mobile.Services.Generic
{
	public class BackEndService : IHttpService
	{
		private static readonly NativeHttpClient HttpClient = new();
		private readonly string _pathSegment;

		protected BackEndService()
		{
			_pathSegment = string.Empty;
		}

		protected BackEndService(string pathSegment)
		{
			_pathSegment = pathSegment;
		}

		protected static async Task<HttpRequestMessage> GenerateHttpRequestMessageAsync(HttpMethod method, 
			string endpoint, HttpContent httpContent, 
			IEnumerable<MediaTypeWithQualityHeaderValue> accepts = null,
			bool addApplicationTokens = true,
			bool addClientInformation = true,
			IDictionary<string, IEnumerable<string>> headers = null,
			QueryParamCollection queryParams = null)
		{
			accepts ??= new[] { new MediaTypeWithQualityHeaderValue("application/json") };
			headers ??= new Dictionary<string, IEnumerable<string>>();
			queryParams ??= new QueryParamCollection();
			
			if (addClientInformation)
			{
				queryParams.Add("clientPlatform", Device.RuntimePlatform);
				queryParams.Add("clientVersion", VersionTracking.CurrentVersion);
			}

			foreach (var (name, value) in Url.ParseQueryParams(endpoint))
			{
				queryParams.Add(name, value);
			}
			
			var requestUri = new BackEndUrl()
				.AppendPathSegment("api")
				.AppendPathSegments(Url.ParsePathSegments(endpoint))
				.SetQueryParams(queryParams)
				.ToUri();
			var httpRequestMessage = new HttpRequestMessage(method, requestUri);
			httpRequestMessage.Content = httpContent;
			if (addApplicationTokens)
			{
				await httpRequestMessage.Headers.AddApplicationTokens();
			}

			foreach (var accept in accepts)
			{
				httpRequestMessage.Headers.Accept.Add(accept);
			}

			foreach (var (key, value) in headers)
			{
				httpRequestMessage.Headers.Add(key, value);
			}
			
			return httpRequestMessage;
		}
		
		private static async Task ResetClientAsync()
		{
			NativeHttpClient.CookieContainer.Clear(BackEndUrl.BaseUri);
			await NativeHttpClient.CookieContainer.AddApplicationTokens(BackEndUrl.BaseUri);
		}

		protected Url GetServiceEndpoint()
		{
			return string.IsNullOrEmpty(_pathSegment) ? new Url() : new Url().AppendPathSegment(_pathSegment);
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

		protected virtual Task<TResponse> RequestAsync<TResponse, TRequest>(HttpMethod method, string endpoint, 
			TRequest requestContent)
			where TResponse : class
			where TRequest : class
		{
			var httpContent = requestContent is not null ? new JsonContent<TRequest>(requestContent) : null;
			return RequestAsync<TResponse>(method, endpoint, httpContent);
		}
		
		protected virtual async Task<TResponse> RequestAsync<TResponse>(HttpMethod method, string endpoint, 
			HttpContent httpContent)
			where TResponse : class
		{
			var httpMessageRequest = await GenerateHttpRequestMessageAsync(method, endpoint, httpContent);
			return await RequestAsync<TResponse>(method, endpoint, httpMessageRequest);
		}

		protected virtual async Task<TResponse> RequestAsync<TResponse>(HttpMethod method, string endpoint, 
			HttpRequestMessage httpRequestMessage)
			where TResponse : class
		{
			var debugMessage = $"{method} {endpoint}\n";
			try
			{
				await ResetClientAsync();
				
				using var httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);
				await httpResponseMessage.GetCookieCollection().SaveApplicationTokens();

				var responseContentBytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();
				var response = responseContentBytes.DeserializeAsync<TResponse>();
				var defaultResponse = responseContentBytes.DeserializeAsync<DefaultResponseModel>();
				
				if (defaultResponse.Errors is not null && 
				    defaultResponse.Errors.Any(error => error.Code == ErrorCodeConstants.InvalidAntiforgeryToken))
				{
					debugMessage = $"{debugMessage}\tInvalid antiforgery token, should be fine to resend request.";
					Debug.WriteLine(debugMessage, GetType().Name);
					throw new Exception("Something went wrong, please try again.");
				}

				debugMessage = $"{debugMessage}{response.ToJsonString()}";
				Debug.WriteLine(debugMessage, GetType().Name);
				return response;
			}
			catch (Exception exception)
			{
				Debug.WriteLine(exception, GetType().Name);
				return exception.Cast<TResponse>();
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