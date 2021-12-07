//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// WebReleaseInformation.cs
//

using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation.Common;

public class WebReleaseInformation : GenericReleaseInformation
{
	[JsonPropertyName("Status")] public StoreReleaseInformation Status { get; set; }
}