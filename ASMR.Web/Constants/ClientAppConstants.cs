using Flurl;

namespace ASMR.Web.Constants;

public static class ClientAppConstants
{
	public const string SourcePath = "ClientApp";
	public const string BuildPath = $"{SourcePath}/build";
	public static readonly Url DevelopmentServerUrl = new("http://127.0.0.1:3000/");
}