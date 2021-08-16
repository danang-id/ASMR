using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation
{
	public class AppReleaseInformation
	{
		[JsonPropertyName("DirectDownload")]
		public StoreReleaseInformation DirectDownload { get; set; }
		
		[JsonPropertyName("Version")]
		public string Version { get; set; }
	}
}