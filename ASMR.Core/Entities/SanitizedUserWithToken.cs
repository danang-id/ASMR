//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// SanitizedUserWithToken.cs
//
namespace ASMR.Core.Entities;

public class SanitizedUserWithToken : SanitizedUser
{
	public string Token { get; set; }
}