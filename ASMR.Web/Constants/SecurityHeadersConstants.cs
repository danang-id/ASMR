//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/30/2021 6:04 PM
//
// SecurityHeadersConstants.cs
//

using Microsoft.AspNetCore.Builder;

namespace ASMR.Web.Constants;

public static class SecurityHeadersConstants
{
	private const string ServerName = "Pandora";

	public static readonly HeaderPolicyCollection DefaultHeaderPolicyCollection = new HeaderPolicyCollection()
		.AddDefaultSecurityHeaders()
		.RemoveCustomHeader("X-Powered-By")
		.AddCustomHeader("Server", ServerName);

	public static readonly HeaderPolicyCollection DevelopmentHeaderPolicyCollection = new HeaderPolicyCollection()
		.AddFrameOptionsDeny()
		.AddXssProtectionBlock()
		.AddContentTypeOptionsNoSniff()
		.AddReferrerPolicyStrictOriginWhenCrossOrigin()
		.RemoveServerHeader()
		.AddContentSecurityPolicy(builder =>
		{
			builder.AddObjectSrc().None();
			builder.AddFormAction().Self();
			builder.AddFrameAncestors().None();
		})
		.RemoveCustomHeader("X-Powered-By")
		.AddCustomHeader("Server", ServerName);
}