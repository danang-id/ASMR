//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// RoastingCancellationReason
//
namespace ASMR.Core.Enumerations;

public enum RoastingCancellationReason
{
	NotCancelled,
	WrongRoastingDataSubmitted,
	RoastingTimeout,
	RoastingFailure,
}