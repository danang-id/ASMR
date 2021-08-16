using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation
{
	public class StoreReleaseInformation
	{
		[JsonPropertyName("Available")]
		public bool Available { get; set; }
		
		[JsonPropertyName("Link")]
		public string Link { get; set; }
	}
}