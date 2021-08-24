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
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Data
{
    public class SelfMigrator
    {
        private const string AdministratorUserName = "admin";
        private const string AdministratorPassword = "@SMR-Adm1n";
        
        private static async Task<bool> SeedData(ILogger<SelfMigrator> logger,
            ApplicationDbContext dbContext, 
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            Configuration configuration)
        {
            var administratorRole = await roleManager.FindByNameAsync(Role.Administrator.ToString());
            if (administratorRole is null)
            {
                var createRoleResult = await roleManager
                    .CreateAsync(new UserRole { Name = Role.Administrator.ToString() });
                if (!createRoleResult.Succeeded)
                {
                    foreach (var identityError in createRoleResult.Errors)
                    {
                        logger.LogError($"[Code: {identityError.Code}] {identityError.Description}");
                    }
                    return false;
                }
            }
            
            var serverRole = await roleManager.FindByNameAsync(Role.Server.ToString());
            if (serverRole is null)
            {
                var createRoleResult = await roleManager
                    .CreateAsync(new UserRole { Name = Role.Server.ToString() });
                if (!createRoleResult.Succeeded)
                {
                    foreach (var identityError in createRoleResult.Errors)
                    {
                        logger.LogError($"[Code: {identityError.Code}] {identityError.Description}");
                    }
                    return false;
                }
            }
            
            var roasterRole = await roleManager.FindByNameAsync(Role.Roaster.ToString());
            if (roasterRole is null)
            {
                var createRoleResult = await roleManager
                    .CreateAsync(new UserRole { Name = Role.Roaster.ToString() });
                if (!createRoleResult.Succeeded)
                {
                    foreach (var identityError in createRoleResult.Errors)
                    {
                        logger.LogError($"[Code: {identityError.Code}] {identityError.Description}");
                    }
                    return false;
                }
            }

            var administrator = await userManager.FindByNameAsync(AdministratorUserName);
            if (administrator is null)
            {
                var administratorId = Guid.NewGuid().ToString();
                var mediaFileId = Guid.NewGuid().ToString();
                administrator = new User
                {
                    Id = administratorId,
                    FirstName = "Admin",
                    LastName = "ASMR",
                    Email = "asmr@hamzahjundi.me",
                    EmailConfirmed = true,
                    UserName = AdministratorUserName,
                    Image = $"/api/mediafile/{mediaFileId}"
                };
                var mediaFile = new MediaFile
                {
                    Id = mediaFileId,
                    Location = Path.Combine("wwwroot", "images", "DefaultUserImage.png"),
                    Name = "DefaultUserImage.png",
                    MimeType = "image/png",
                    UserId = administratorId
                    
                };
                var createUserResult = await userManager.CreateAsync(administrator, AdministratorPassword);
                if (!createUserResult.Succeeded)
                {
                    foreach (var identityError in createUserResult.Errors)
                    {
                        logger.LogError($"[Code: {identityError.Code}] {identityError.Description}");
                    }
                    return false;
                }
                
                var addUserToRoleResult = await userManager.AddToRoleAsync(administrator, Role.Administrator.ToString());
                if (!addUserToRoleResult.Succeeded)
                {
                    foreach (var identityError in addUserToRoleResult.Errors)
                    {
                        logger.LogError($"[Code: {identityError.Code}] {identityError.Description}");
                    }
                    return false;
                }
                dbContext.MediaFiles.Add(mediaFile);
            }

            if (configuration is null)
            {
                configuration = new Configuration
                {
                    Key = ConfigurationKey.DataSeedDone,
                    Value = true.ToString()
                };
                dbContext.Configurations.Add(configuration);
            }
            else
            {
                configuration.Value = true.ToString();
                dbContext.Configurations.Update(configuration);
            }

            dbContext.SaveChanges();
            return true;
        }

        public static async Task<bool> Migrate(IServiceProvider serviceProvider)
        {
            await using var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            var logger = serviceProvider.GetRequiredService<ILogger<SelfMigrator>>();

            await dbContext.Database.MigrateAsync();

            var configuration = dbContext.Configurations
                .FirstOrDefault(entity => entity.Key == ConfigurationKey.DataSeedDone);
            if (configuration is null)
            {
                return await SeedData(logger, dbContext, userManager, roleManager, null);
            }

            if (configuration.Value != true.ToString())
            {
                return await SeedData(logger, dbContext, userManager, roleManager, configuration);
            }

            return true;
        }
    }
}
