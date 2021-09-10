using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASMR.Common.Constants;
using ASMR.Mobile.Common.Security;

namespace ASMR.Mobile.Extensions
{
	public static class CookieCollectionExtension
	{
		private static async Task SaveCookieIfExistsToStorage(this CookieCollection cookies, string name)
		{
			if (cookies is null)
			{
				return;
			}

			var cookie = cookies.Cast<Cookie>()
				.Where(cookie => cookie.Name == name)
				.FirstOrDefault();
			var removeStorage = cookie is null || cookie.Expired;
			if (removeStorage)
			{
				TokenManager.Remove(name);
				return;
			}

			var currentValue = await TokenManager.GetAsync(name);
			if (currentValue != cookie.Value)
			{
				await TokenManager.SetAsync(name, cookie.Value);
			}
		}
		
		public static Task SaveApplicationTokens(this CookieCollection cookies)
		{
			return Task.WhenAll(cookies.SaveCookieIfExistsToStorage(AuthenticationConstants.CookieName),
				cookies.SaveCookieIfExistsToStorage(AntiforgeryConstants.CookieName),
				cookies.SaveCookieIfExistsToStorage(AntiforgeryConstants.RequestTokenCookieName));
		}
	}
}