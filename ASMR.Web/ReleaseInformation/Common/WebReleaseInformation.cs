using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation.Common
{
	public class WebReleaseInformation : GenericReleaseInformation
	{
		[JsonPropertyName("Status")]
		public StoreReleaseInformation Status { get; set; }
	}
}