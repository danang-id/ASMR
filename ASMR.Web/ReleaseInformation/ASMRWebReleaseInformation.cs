//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// ASMRWebReleaseInformation.cs
//

using System.Text.Json.Serialization;
using ASMR.Web.ReleaseInformation.Common;

// ReSharper disable InconsistentNaming
namespace ASMR.Web.ReleaseInformation;

public class ASMRWebReleaseInformation
{
	[JsonPropertyName("BackEnd")] public WebReleaseInformation BackEnd { get; set; }

	[JsonPropertyName("FrontEnd")] public WebReleaseInformation FrontEnd { get; set; }
}