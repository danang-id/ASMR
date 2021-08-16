using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation
{
	public class AndroidReleaseInformation : AppReleaseInformation
	{
		[JsonPropertyName("PlayStore")]
		public StoreReleaseInformation PlayStore { get; set; }
	}
}