// ReSharper disable InconsistentNaming

using System.Text.Json.Serialization;
using ASMR.Web.ReleaseInformation.Common;

namespace ASMR.Web.ReleaseInformation;

public class ASMRWebReleaseInformation
{
	[JsonPropertyName("BackEnd")] public WebReleaseInformation BackEnd { get; set; }

	[JsonPropertyName("FrontEnd")] public WebReleaseInformation FrontEnd { get; set; }
}