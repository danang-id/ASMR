//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// AppReleaseInformation.cs
//

using System.Text.Json.Serialization;

namespace ASMR.Web.ReleaseInformation.Common;

public class AppReleaseInformation : GenericReleaseInformation
{
	[JsonPropertyName("DirectDownload")] public StoreReleaseInformation DirectDownload { get; set; }
}