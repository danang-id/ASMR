using System.Collections.Generic;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities
{
	public class NormalizedUser : DefaultAbstractEntity
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }
		
		public string EmailAddress { get; set; }
		
		public string Username { get; set; }

		public string Image { get; set; }
		
		public IEnumerable<NormalizedUserRole> Roles { get; set; }
	}
}