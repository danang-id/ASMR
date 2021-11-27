using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation.Common;

public class AppReleaseInformation : GenericReleaseInformation
{
	[JsonPropertyName("DirectDownload")] public StoreReleaseInformation DirectDownload { get; set; }
}