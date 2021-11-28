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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASMR.Common.DataStructure;
using ASMR.Common.Threading;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Data;

public class SelfMigrator
{
	private const string AdministratorId = "07d60e85-2111-43d4-95f3-80864bd71ce5";
	private const string AdministratorFirstName = "Administrator";
	private const string AdministratorLastName = "ASMR";
	private const string AdministratorUserName = "admin";
	private const string AdministratorEmailAddress = "asmr@hamzahjundi.me";
	private const string AdministratorPassword = "@SMR-Adm1n";

	private static async Task PopulateAnalytics(ApplicationDbContext dbContext,
		BusinessAnalyticReference reference, 
		string referenceValue)
	{
		var isAllowedReference = reference is BusinessAnalyticReference.Bean or BusinessAnalyticReference.User;
		if (!isAllowedReference || string.IsNullOrEmpty(referenceValue))
		{
			return;
		}

		var currentAnalytics = dbContext.BusinessAnalytics
			.Where(e => e.Reference == reference && e.ReferenceValue == referenceValue)
			.AsQueryable();
		if (currentAnalytics.Any())
		{
			return;
		}

		var analyticKeys = EnumUtils.GetValues<BusinessAnalyticKey>()
			.ToList();
		// Add more because of inter-references
		analyticKeys.AddRange(new List<BusinessAnalyticKey>
		{
			BusinessAnalyticKey.RoastingCancellationReasonTotal,
			BusinessAnalyticKey.RoastingCancellationReasonTotal,
			BusinessAnalyticKey.RoastingCancellationReasonRate,
			BusinessAnalyticKey.RoastingCancellationReasonRate,
		});

		var currentCancellationReasonTotal = RoastingCancellationReason.WrongRoastingDataSubmitted;
		var currentCancellationReasonRate = RoastingCancellationReason.WrongRoastingDataSubmitted;
		var analytics = analyticKeys
			.Select(analyticKey =>
			{
				var analytic = new BusinessAnalytic
				{
					Id = Guid.NewGuid().ToString(),
					Key = analyticKey,
					Reference = reference,
					ReferenceValue = referenceValue,
					AlternateReference = BusinessAnalyticReference.NoReference,
					AlternateReferenceValue = string.Empty
				};

				switch (analyticKey)
				{
					case BusinessAnalyticKey.RoastingCancellationReasonTotal:
						analytic.AlternateReference = BusinessAnalyticReference.RoastingCancellationReason;
						analytic.AlternateReferenceValue = ((int)currentCancellationReasonTotal).ToString();
						currentCancellationReasonTotal++;
						break;
					case BusinessAnalyticKey.RoastingCancellationReasonRate:
						analytic.AlternateReference = BusinessAnalyticReference.RoastingCancellationReason;
						analytic.AlternateReferenceValue = ((int)currentCancellationReasonRate).ToString();
						currentCancellationReasonRate++;
						break;
				}

				return analytic;
			})
			.ToList();

		await dbContext.BusinessAnalytics.AddRangeAsync(analytics);
	}
	
	private static async Task<bool> SeedDataAsync(IServiceProvider serviceProvider, 
		ApplicationDbContext dbContext,
		Configuration configuration)
	{
		var emailService = serviceProvider.GetRequiredService<IEmailService>();
		var logger = serviceProvider.GetRequiredService<ILogger<SelfMigrator>>();
		var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
		var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

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
					logger.LogError("[Code: {Code}] {Description}", 
						identityError.Code, identityError.Description);
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
					logger.LogError("[Code: {Code}] {Description}", 
						identityError.Code, identityError.Description);
				}

				return false;
			}
		}

		var administrator = await userManager.FindByIdAsync(AdministratorId);
		if (administrator is null)
		{
			var mediaFileId = Guid.NewGuid().ToString();
			administrator = new User
			{
				Id = AdministratorId,
				FirstName = AdministratorFirstName,
				LastName = AdministratorLastName,
				Email = AdministratorEmailAddress,
				EmailConfirmed = true,
				IsWaitingForApproval = false,
				IsApproved = true,
				UserName = AdministratorUserName,
				Image = $"/api/mediafile/{mediaFileId}"
			};
			var mediaFile = new MediaFile
			{
				Id = mediaFileId,
				Location = Path.Combine("wwwroot", "images", "DefaultUserImage.png"),
				Name = "DefaultUserImage.png",
				MimeType = "image/png",
				UserId = AdministratorId
			};
			var createUserResult = await userManager.CreateAsync(administrator, AdministratorPassword);
			if (!createUserResult.Succeeded)
			{
				foreach (var identityError in createUserResult.Errors)
				{
					logger.LogError("[Code: {Code}] {Description}", 
						identityError.Code, identityError.Description);
				}

				return false;
			}

			var addUserToRoleResult = await userManager.AddToRoleAsync(administrator, Role.Administrator.ToString());
			if (!addUserToRoleResult.Succeeded)
			{
				foreach (var identityError in addUserToRoleResult.Errors)
				{
					logger.LogError("[Code: {Code}] {Description}", 
						identityError.Code, identityError.Description);
				}

				return false;
			}

			dbContext.MediaFiles.Add(mediaFile);

			administrator = await userManager.FindByIdAsync(AdministratorId);
			var upsertContactError = await emailService.UpsertContactAsync(administrator);
			if (upsertContactError is not null)
			{
				logger.LogError(upsertContactError.SendGridErrorMessage);
				return false;
			}

			var welcomeMailError = await emailService.SendWelcomeMailAsync(administrator,
				Role.Administrator.ToString());
			if (welcomeMailError is not null)
			{
				logger.LogError(welcomeMailError.SendGridErrorMessage);
				return false;
			}
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

		await PopulateAnalytics(dbContext, BusinessAnalyticReference.User, AdministratorId);

		dbContext.SaveChanges();
		return true;
	}

	public static async Task<bool> MigrateAsync(IServiceProvider serviceProvider)
	{
		await using var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
		await dbContext.Database.MigrateAsync();

		var configuration = dbContext.Configurations
			.FirstOrDefault(entity => entity.Key == ConfigurationKey.DataSeedDone);
		if (configuration is null)
		{
			return await SeedDataAsync(serviceProvider, dbContext, null);
		}

		if (configuration.Value != true.ToString())
		{
			return await SeedDataAsync(serviceProvider, dbContext, configuration);
		}

		return true;
	}

	public static bool Migrate(IServiceProvider serviceProvider)
	{
		Task<bool> InnerMigrateAsync() => MigrateAsync(serviceProvider);
		return TaskHelpers.RunSync(InnerMigrateAsync);
	}
}