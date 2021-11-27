using System.Text.Json.Serialization;

// ReSharper disable InconsistentNaming
namespace ASMR.Web.ReleaseInformation.Common;

public class iOSReleaseInformation : AppReleaseInformation
{
	[JsonPropertyName("AppStore")] public StoreReleaseInformation AppStore { get; set; }
}