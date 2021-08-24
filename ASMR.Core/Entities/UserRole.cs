using System;
using System.ComponentModel.DataAnnotations;
using ASMR.Core.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace ASMR.Core.Entities
{
	public class UserRole : IdentityRole
	{
		public UserRole()
		{
			CreatedAt = DateTimeOffset.Now;
		}
        
		[Required]
		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? LastUpdatedAt { get; set; }

		public NormalizedUserRole ToNormalizedUserRole()
		{
			return new NormalizedUserRole
			{
				Id = Id,
				Role = (Role)Enum.Parse(typeof(Role), Name),
				CreatedAt = CreatedAt,
				LastUpdatedAt = LastUpdatedAt
			};
		}
	}
}