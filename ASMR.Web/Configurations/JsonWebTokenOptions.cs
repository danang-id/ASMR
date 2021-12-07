//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// JsonWebTokenOptions.cs
//

namespace ASMR.Web.Configurations;

public class JsonWebTokenOptions
{
	public string Issuer { get; set; }

	public string Key { get; set; }
}