using System.Text.Json.Serialization;
using ASMR.Web.ReleaseInformation.Common;

// ReSharper disable InconsistentNaming
namespace ASMR.Web.ReleaseInformation;

public class ASMRMobileReleaseInformation
{
	[JsonPropertyName("Android")] public AndroidReleaseInformation Android { get; set; }

	[JsonPropertyName("iOS")] public iOSReleaseInformation iOS { get; set; }
}