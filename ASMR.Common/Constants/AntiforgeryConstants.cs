//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/9/2021 7:28 AM
//
// AntiforgeryConstants.cs
//

using System;

namespace ASMR.Common.Constants;

public static class AntiforgeryConstants
{
	public const bool CookieHttpOnly = false;

	public const string CookieName = "ASMR.CSRF-Token";

	public static TimeSpan CookieExpiration = TimeSpan.FromMinutes(60);

	/* The Form field name that the client use to include Antiforgery Request Token. */
	public const string FormFieldName = "__CSRF-Token__";

	/* The Header name that the client use to include Antiforgery Request Token. */
	public const string HeaderName = "X-CSRF-Token";

	/* The Cookie name of Antiforgery Request Token. */
	public const string RequestTokenCookieName = "ASMR.CSRF-Request-Token";

	public const bool SuppressXFrameOptionsHeader = false;
}