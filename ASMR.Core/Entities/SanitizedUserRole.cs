using ASMR.Core.Enumerations;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class SanitizedUserRole : DefaultAbstractEntity
{
	public Role Role { get; set; }
}