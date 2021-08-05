//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:30 AM
//
// SelfMigrator.cs
//
using System;
using System.IO;
using System.Linq;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ASMR.Web.Data
{
    public sealed class SelfMigrator
    {
        public static void SeedData(ApplicationDbContext dbContext, IHashingService hashingService)
        {
            var administratorUsername = "admin";
            var administrator = dbContext.Users.FirstOrDefault(entity => entity.Username == administratorUsername);
            if (administrator is null)
            {
                var administratorId = Guid.NewGuid().ToString();
                var mediaFileId = Guid.NewGuid().ToString();
                administrator = new User
                {
                    Id = administratorId,
                    FirstName = "Administrator",
                    LastName = "1",
                    Username = administratorUsername,
                    Image = $"/api/mediafile/{mediaFileId}",
                    HashedPassword = hashingService.HashBase64("@SMR-Adm1n"),
                };
                var mediaFile = new MediaFile
                {
                    Id = mediaFileId,
                    Location = Path.Combine("wwwroot", "images", "asmr-logo.webp"),
                    Name = "user.webp",
                    MimeType = "image/webp",
                    UserId = administratorId
                    
                };
                dbContext.Users.Add(administrator);
                dbContext.MediaFiles.Add(mediaFile);
            }

            var role = dbContext.UserRoles
                    .FirstOrDefault(entity => entity.UserId == administrator.Id && entity.Role == Role.Administrator);
            if (role is null)
            {
                role = new UserRole
                {
                    User = administrator,
                    UserId = administrator.Id,
                    Role = Role.Administrator
                };
                dbContext.UserRoles.Add(role);
            }

            var config = new Configuration
            {
                Key = ConfigurationKey.DataSeedDone,
                Value = true.ToString()
            };
            dbContext.Configurations.Add(config);

            dbContext.SaveChanges();
        }

        public static void Migrate(IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var hashingService = scope.ServiceProvider.GetRequiredService<IHashingService>();

            dbContext.Database.Migrate();

            var config = dbContext.Configurations
                .FirstOrDefault(entity => entity.Key == ConfigurationKey.DataSeedDone);
            if (config is null)
            {
                SeedData(dbContext, hashingService);
            } else if (config.Value != true.ToString())
            {
                SeedData(dbContext, hashingService);
            }
        }
    }
}
