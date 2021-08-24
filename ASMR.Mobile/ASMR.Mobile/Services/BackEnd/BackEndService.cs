using System;
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
		private static readonly NativeHttpClient Client = new();

		protected BackEndService()
		{
		}
		
		private static HttpContent ParseContent<TRequest>(TRequest requestContent)
		{
			return requestContent is null ? null : new JsonContent<TRequest>(requestContent);
		}

		private static HttpRequestMessage ParseRequestMessage(HttpMethod method, string endpoint, HttpContent httpContent)
		{
			var queryParams = new QueryParamCollection();
			queryParams.Add("clientPlatform", Device.RuntimePlatform);
			queryParams.Add("clientVersion", VersionTracking.CurrentVersion);
			
			var url = new BackEndUrl()
				.AppendPathSegment("api")
				.AppendPathSegments(Url.ParsePathSegments(endpoint))
				.SetQueryParams(queryParams);
			return new HttpRequestMessage(method, url.ToUri())
			{
				Content = httpContent
			};
		}
		
		private async Task ResetClient()
		{
			Client.DefaultRequestHeaders.Clear();
			Client.DefaultRequestHeaders
				.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client.DefaultRequestHeaders
				.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
			await Client.DefaultRequestHeaders.AddApplicationTokens();
		}
		
		protected virtual Task<TResponse> Request<TResponse>(HttpMethod method, string endpoint) 
			where TResponse : class
		{
			return Request<TResponse, object>(method, endpoint, null);
		}

		protected virtual Task<TResponse> Request<TResponse>(HttpMethod method, string endpoint, object requestContent) 
			where TResponse : class
		{
			return Request<TResponse, object>(method, endpoint, requestContent);
		}

		protected virtual async Task<TResponse> Request<TResponse, TRequest>(HttpMethod method, string endpoint, TRequest requestContent)
			where TResponse : class
			where TRequest : class
		{
			Debug.WriteLine("NativeHttpClient: Request Started", GetType().Name);
			try
			{
				await ResetClient();
				await NativeHttpClient.CookieContainer.AddApplicationTokens(BackEndUrl.BaseAddress);
				
				using var httpContent = ParseContent(requestContent);
				using var httpRequestMessage = ParseRequestMessage(method, endpoint, httpContent);
				using var httpResponseMessage = await Client.SendAsync(httpRequestMessage);
				
				var response = await httpResponseMessage.ToResponseModel<TResponse>();
				Debug.WriteLine($"{method} {endpoint}: {response}", GetType().Name);
				
				await NativeHttpClient.CookieContainer.SaveApplicationTokens(BackEndUrl.BaseAddress);
				
				return response;
			}
			catch (Exception exception)
			{
				Debug.WriteLine($"[{GetType().Name}] {exception}");
				return exception.ToResponseModel<TResponse>();
			}
		}
		
		protected virtual Task<TResponse> Get<TResponse>(string endpoint) 
			where TResponse : class
		{
			return Request<TResponse>(HttpMethod.Get, endpoint);
		}
		
		protected virtual Task<TResponse> Post<TResponse>(string endpoint) 
			where TResponse : class
		{
			return Request<TResponse>(HttpMethod.Post, endpoint);
		}
		
		protected virtual Task<TResponse> Post<TResponse>(string endpoint, object data) 
			where TResponse : class
		{
			return Request<TResponse>(HttpMethod.Post, endpoint, data);
		}

		protected virtual Task<TResponse> Post<TResponse, TRequest>(string endpoint, TRequest data) 
			where TResponse : class
			where TRequest : class
		{
			return Request<TResponse, TRequest>(HttpMethod.Post, endpoint, data);
		}

		protected virtual Task<TResponse> Patch<TResponse>(string endpoint) 
			where TResponse : class
		{
			return Request<TResponse>(HttpMethod.Patch, endpoint);
		}
		
		protected virtual Task<TResponse> Patch<TResponse>(string endpoint, object data) 
			where TResponse : class
		{
			return Request<TResponse>(HttpMethod.Patch, endpoint, data);
		}

		protected virtual Task<TResponse> Patch<TResponse, TRequest>(string endpoint, TRequest data) 
			where TResponse : class
			where TRequest : class
		{
			return Request<TResponse, TRequest>(HttpMethod.Patch, endpoint, data);
		}

		protected virtual Task<TResponse> Put<TResponse>(string endpoint) 
			where TResponse : class
		{
			return Request<TResponse>(HttpMethod.Put, endpoint);
		}
		
		protected virtual Task<TResponse> Put<TResponse>(string endpoint, object data) 
			where TResponse : class
		{
			return Request<TResponse>(HttpMethod.Put, endpoint, data);
		}

		protected virtual Task<TResponse> Put<TResponse, TRequest>(string endpoint, TRequest data) 
			where TResponse : class
			where TRequest : class
		{
			return  Request<TResponse, TRequest>(HttpMethod.Put, endpoint, data);
		}
		
		protected virtual Task<TResponse> Delete<TResponse>(string endpoint) 
			where TResponse : class
		{
			return Request<TResponse>(HttpMethod.Delete, endpoint);
		}
	}
}