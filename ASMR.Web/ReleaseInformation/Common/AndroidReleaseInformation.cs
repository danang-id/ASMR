//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// AndroidReleaseInformation.cs
//

using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation.Common;

public class AndroidReleaseInformation : AppReleaseInformation
{
	[JsonPropertyName("PlayStore")] public StoreReleaseInformation PlayStore { get; set; }
}