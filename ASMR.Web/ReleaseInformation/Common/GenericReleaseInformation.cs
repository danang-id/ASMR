//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// GenericReleaseInformation.cs
//

using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation.Common;

public abstract class GenericReleaseInformation
{
	[JsonPropertyName("Version")] public string Version { get; set; }

	[JsonPropertyName("VersionCode")] public int VersionCode { get; set; }
}