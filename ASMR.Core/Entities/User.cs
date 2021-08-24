//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:04 AM
//
// User.cs
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace ASMR.Core.Entities
{
    [DataContract]
    public class User : IdentityUser
    {
        public User()
        {
            CreatedAt = DateTimeOffset.Now;
        }

        [DataMember]
        [Required]
        public override string UserName { get; set; }

        [DataMember]
        [Required]
        public string FirstName { get; set; }

        [DataMember]
        [Required]
        public string LastName { get; set; }

        [DataMember]
        [Required]
        public string Image { get; set; }
        
        [DataMember]
        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [DataMember]
        public DateTimeOffset? LastUpdatedAt { get; set; }

        public NormalizedUser ToNormalizedUser()
        {
            return new NormalizedUser
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = Email,
                Username = UserName,
                Image = Image,
                CreatedAt = CreatedAt,
                LastUpdatedAt = LastUpdatedAt
            };
        }
        
        public NormalizedUser ToNormalizedUser(IEnumerable<UserRole> roles)
        {
            return new NormalizedUser
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = Email,
                Username = UserName,
                Image = Image,
                Roles = roles.Select(role => role.ToNormalizedUserRole()),
                CreatedAt = CreatedAt,
                LastUpdatedAt = LastUpdatedAt
            };
        }
        
        public NormalizedUserWithToken ToNormalizedUserWithToken(string token)
        {
            return new NormalizedUserWithToken
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = Email,
                Username = UserName,
                Image = Image,
                Token = token,
                CreatedAt = CreatedAt,
                LastUpdatedAt = LastUpdatedAt
            };
        }
        
        public NormalizedUserWithToken ToNormalizedUserWithToken(string token, IEnumerable<UserRole> roles)
        {
            return new NormalizedUserWithToken
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = Email,
                Username = UserName,
                Image = Image,
                Roles = roles.Select(role => role.ToNormalizedUserRole()),
                Token = token,
                CreatedAt = CreatedAt,
                LastUpdatedAt = LastUpdatedAt
            };
        }
    }
}
