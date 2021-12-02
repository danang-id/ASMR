//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 11:54 PM
//
// RoastingSessionService.cs
//

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASMR.Web.Services;

public interface IRoastingService : IServiceBase
{
	public IQueryable<Roasting> GetAllRoastings();

	public IQueryable<Roasting> GetRoastingsByUserId(string userId);

	public Task<Roasting> GetRoastingById(string id);

	public Task<Roasting> CreateRoasting(Roasting roasting);

	public Task<Roasting> ModifyRoasting(string id, Roasting roasting);
}

public class RoastingService : ServiceBase, IRoastingService
{
	public RoastingService(ApplicationDbContext dbContext) : base(dbContext)
	{
	}

	public IQueryable<Roasting> GetAllRoastings()
	{
		return DbContext.Roastings
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.AsQueryable();
	}

	public IQueryable<Roasting> GetRoastingsByUserId(string userId)
	{
		return DbContext.Roastings
			.Where(e => e.UserId == userId)
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.AsQueryable();
	}

	public Task<Roasting> GetRoastingById(string id)
	{
		return DbContext.Roastings
			.Where(e => e.Id == id)
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.FirstOrDefaultAsync();
	}

	public async Task<Roasting> CreateRoasting(Roasting roasting)
	{
		var entityEntry = await DbContext.Roastings.AddAsync(roasting);
		var bean = await DbContext.Beans.Where(e => e.Id == entityEntry.Entity.BeanId)
			.FirstOrDefaultAsync();
		var user = await DbContext.Users.Where(e => e.Id == entityEntry.Entity.UserId)
			.FirstOrDefaultAsync();

		async Task ProgressAnalytics(IQueryable<BusinessAnalytic> analytics)
		{
			var total = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.RoastingTotal)
				.FirstOrDefaultAsync();
			var greenBeanWeightTotal = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.RoastingGreenBeanWeightTotal)
				.FirstOrDefaultAsync();
			var greenBeanWeightAverage = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.RoastingGreenBeanWeightAverage)
				.FirstOrDefaultAsync();

			if (total is null || greenBeanWeightTotal is null || greenBeanWeightAverage is null)
			{
				return;
			}

			total.IntValue++;
			greenBeanWeightTotal.DecimalValue += roasting.GreenBeanWeight;
			greenBeanWeightAverage.DecimalValue = greenBeanWeightTotal.DecimalValue / total.IntValue;

			ModifyAnalytics(new List<BusinessAnalytic>
			{
				total, greenBeanWeightTotal, greenBeanWeightAverage
			});
		}

		var beanAnalytics = GetAnalytics(BusinessAnalyticReference.Bean, bean?.Id);
		var userAnalytics = GetAnalytics(BusinessAnalyticReference.User, user?.Id);
		await Task.WhenAll(new List<Task>
		{
			ProgressAnalytics(beanAnalytics),
			ProgressAnalytics(userAnalytics)
		});

		return entityEntry.Entity;
	}

	public async Task<Roasting> ModifyRoasting(string id,
		Roasting roasting)
	{
		var entity = await DbContext.Roastings
			.Where(e => e.Id == id)
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.FirstOrDefaultAsync();
		if (entity is null)
		{
			return null;
		}

		var cancelOrFinishAllowed = entity.CancelledAt is null && entity.FinishedAt is null;
		var isCancelling = cancelOrFinishAllowed &&
		                   roasting.CancelledAt is not null &&
		                   roasting.CancellationReason is not RoastingCancellationReason.NotCancelled;
		var isFinishing = cancelOrFinishAllowed &&
		                  roasting.FinishedAt is not null &&
		                  roasting.RoastedBeanWeight > 0 &&
		                  roasting.RoastedBeanWeight <= entity.GreenBeanWeight;

		if (isCancelling)
		{
			entity.CancelledAt = roasting.CancelledAt;
			entity.CancellationReason = roasting.CancellationReason;
		}

		if (isFinishing)
		{
			entity.FinishedAt = roasting.FinishedAt;
			entity.RoastedBeanWeight = roasting.RoastedBeanWeight;
		}

		var entityEntry = DbContext.Roastings.Update(entity);
		var bean = await DbContext.Beans.Where(e => e.Id == entityEntry.Entity.BeanId)
			.FirstOrDefaultAsync();
		var user = await DbContext.Users.Where(e => e.Id == entityEntry.Entity.UserId)
			.FirstOrDefaultAsync();

		async Task ProgressAnalytics(IQueryable<BusinessAnalytic> analytics)
		{
			var total = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.RoastingTotal)
				.FirstOrDefaultAsync();
			if (total is null)
			{
				return;
			}

			if (isCancelling)
			{
				var cancelledTotal = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingCancelledTotal)
					.FirstOrDefaultAsync();
				var cancelledTotalRate = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingCancelledTotalRate)
					.FirstOrDefaultAsync();
				var cancellationReasonTotal = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingCancellationReasonTotal &&
					            e.AlternateReference == BusinessAnalyticReference.RoastingCancellationReason &&
					            e.AlternateReferenceValue == ((int)roasting.CancellationReason).ToString())
					.FirstOrDefaultAsync();
				var cancellationReasonRate = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingCancellationReasonRate &&
					            e.AlternateReference == BusinessAnalyticReference.RoastingCancellationReason &&
					            e.AlternateReferenceValue == ((int)roasting.CancellationReason).ToString())
					.FirstOrDefaultAsync();

				if (cancelledTotal is not null &&
				    cancelledTotalRate is not null &&
				    cancellationReasonTotal is not null &&
				    cancellationReasonRate is not null)
				{
					cancelledTotal.IntValue++;
					cancelledTotalRate.DecimalValue = (decimal)total.IntValue / cancelledTotal.IntValue;
					cancellationReasonTotal.IntValue++;
					cancellationReasonTotal.DecimalValue =
						(decimal)cancellationReasonTotal.IntValue / cancelledTotal.IntValue;

					ModifyAnalytics(new List<BusinessAnalytic>
					{
						cancelledTotal,
						cancelledTotalRate,
						cancellationReasonTotal,
						cancellationReasonTotal
					});
				}
			}

			if (isFinishing)
			{
				var finishedTotal = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingFinishedTotal)
					.FirstOrDefaultAsync();
				var finishedTotalRate = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingFinishedTotalRate)
					.FirstOrDefaultAsync();
				var greenBeanWeightTotal = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingGreenBeanWeightTotal)
					.FirstOrDefaultAsync();
				var roastedBeanWeightTotal = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingRoastedBeanWeightTotal)
					.FirstOrDefaultAsync();
				var roastedBeanWeightAverage = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingRoastedBeanWeightAverage)
					.FirstOrDefaultAsync();
				var depreciationWeightTotal = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingDepreciationWeightTotal)
					.FirstOrDefaultAsync();
				var depreciationWeightAverage = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingDepreciationWeightAverage)
					.FirstOrDefaultAsync();
				var depreciationRate = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingDepreciationRate)
					.FirstOrDefaultAsync();
				var depreciationAverageRate = await analytics
					.Where(e => e.Key == BusinessAnalyticKey.RoastingDepreciationAverageRate)
					.FirstOrDefaultAsync();

				if (finishedTotal is not null &&
				    finishedTotalRate is not null)
				{
					finishedTotal.IntValue++;
					finishedTotalRate.DecimalValue = (decimal)total.IntValue / finishedTotal.IntValue;

					ModifyAnalytics(new List<BusinessAnalytic>
					{
						finishedTotal,
						finishedTotalRate
					});
				}

				if (finishedTotal is not null &&
				    roastedBeanWeightTotal is not null &&
				    roastedBeanWeightAverage is not null)
				{
					roastedBeanWeightTotal.DecimalValue += roasting.RoastedBeanWeight;
					roastedBeanWeightAverage.DecimalValue =
						roastedBeanWeightTotal.DecimalValue / finishedTotal.IntValue;

					ModifyAnalytics(new List<BusinessAnalytic>
					{
						roastedBeanWeightTotal,
						roastedBeanWeightAverage,
					});
				}

				if (finishedTotal is not null &&
				    greenBeanWeightTotal is not null &&
				    roastedBeanWeightTotal is not null &&
				    depreciationWeightTotal is not null &&
				    depreciationWeightAverage is not null &&
				    depreciationRate is not null &&
				    depreciationAverageRate is not null)
				{
					var deprecation = entity.GreenBeanWeight - entity.RoastedBeanWeight;
					depreciationWeightTotal.DecimalValue += deprecation;
					depreciationWeightAverage.DecimalValue =
						depreciationWeightTotal.DecimalValue / finishedTotal.IntValue;
					depreciationRate.DecimalValue =
						depreciationWeightTotal.DecimalValue / greenBeanWeightTotal.DecimalValue;
					depreciationAverageRate.DecimalValue = depreciationRate.DecimalValue / finishedTotal.IntValue;

					ModifyAnalytics(new List<BusinessAnalytic>
					{
						depreciationWeightTotal,
						depreciationWeightAverage,
						depreciationRate,
						depreciationAverageRate
					});
				}
			}
		}

		var beanAnalytics = GetAnalytics(BusinessAnalyticReference.Bean, bean?.Id);
		var userAnalytics = GetAnalytics(BusinessAnalyticReference.User, user?.Id);
		await Task.WhenAll(new List<Task>
		{
			ProgressAnalytics(beanAnalytics),
			ProgressAnalytics(userAnalytics)
		});

		return entityEntry.Entity;
	}
}