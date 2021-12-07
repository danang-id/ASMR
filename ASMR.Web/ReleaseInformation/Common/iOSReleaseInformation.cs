//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// iOSReleaseInformation.cs
//

using System.Text.Json.Serialization;

// ReSharper disable InconsistentNaming
namespace ASMR.Web.ReleaseInformation.Common;

public class iOSReleaseInformation : AppReleaseInformation
{
	[JsonPropertyName("AppStore")] public StoreReleaseInformation AppStore { get; set; }
}