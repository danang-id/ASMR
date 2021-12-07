//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// StoreReleaseInformation.cs
//

using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation.Common;

public class StoreReleaseInformation
{
	[JsonPropertyName("Available")] public bool Available { get; set; }

	[JsonPropertyName("Link")] public string Link { get; set; }
}