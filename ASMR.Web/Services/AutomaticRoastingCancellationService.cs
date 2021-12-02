using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Services;

public class AutomaticRoastingCancellationService : TimedHostedService
{
	// ReSharper disable once SuggestBaseTypeForParameterInConstructor
	public AutomaticRoastingCancellationService(IServiceScopeFactory serviceScopeFactory,
		ILogger<AutomaticRoastingCancellationService> logger)
		: base("Automatic Roasting Cancellation Service", TimeSpan.FromMinutes(1),
			serviceScopeFactory, logger)
	{
	}

	private static async Task CancelRoasting(IServiceScope scope, Roasting roasting)
	{
		var roastingService = scope.ServiceProvider.GetRequiredService<IRoastingService>();
		roasting.CancelledAt = DateTimeOffset.Now;
		roasting.CancellationReason = RoastingCancellationReason.RoastingTimeout;

		await roastingService.ModifyRoasting(roasting.Id, roasting);
	}

	protected override async Task RunJobAsync(CancellationToken cancellationToken)
	{
		var now = DateTimeOffset.Now;
		var yesterday = now.AddDays(-1);

		using var scope = ServiceScopeFactory.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		var roastings = dbContext.Roastings
			.Where(e => e.CreatedAt < now &&
			            e.CreatedAt < yesterday &&
			            e.FinishedAt == null &&
			            e.CancelledAt == null)
			.AsQueryable();

		foreach (var roasting in roastings)
		{
			await CancelRoasting(scope, roasting);
		}

		Logger.LogInformation("Cancelled {TimedOutRoasting} timed-out roasting(s)", roastings.Count());

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}