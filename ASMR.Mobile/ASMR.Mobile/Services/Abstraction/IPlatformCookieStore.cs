using System;
using System.Collections.Generic;
using System.Net;

namespace ASMR.Mobile.Services.Abstraction
{
	public interface IPlatformCookieStore
	{
		/// <summary> 
		/// Add cookie to the cookie storage manager on the platform for url. 
		/// </summary> 
		void Add(Uri uri, Cookie cookie);

		/// <summary> 
		/// Get cookies from the cookie storage manager on the platform for url. 
		/// </summary> 
		IEnumerable<Cookie> Get(Uri uri);

		/// <summary> 
		/// Clear cookies for site/url (otherwise auth tokens for your provider 
		/// will hang around after a logout, which causes problems if you want 
		/// to log in as someone else) 
		/// </summary> 
		void Clear(Uri uri);

#if DEBUG
		/// <summary> 
		/// Debug method, just lists all cookies in the <see cref="Get"/> list 
		/// </summary> 
		void Dump(Uri uri);
#endif
	}
}