using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation.Common;

public abstract class GenericReleaseInformation
{
	[JsonPropertyName("Version")] public string Version { get; set; }

	[JsonPropertyName("VersionCode")] public int VersionCode { get; set; }
}