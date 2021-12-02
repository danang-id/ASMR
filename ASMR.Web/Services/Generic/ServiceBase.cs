//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/27/2021 12:49 PM
//
// ServiceBase.cs
//

using System;
using System.Collections.Generic;
using System.Linq;
using ASMR.Web.Data;
using System.Threading.Tasks;
using ASMR.Common.DataStructure;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;

namespace ASMR.Web.Services.Generic;

public interface IServiceBase
{
	public int Commit();

	public Task CommitAsync();
}

public class ServiceBase : IServiceBase
{
	protected readonly ApplicationDbContext DbContext;

	protected ServiceBase(ApplicationDbContext dbContext)
	{
		DbContext = dbContext;
	}

	protected IQueryable<BusinessAnalytic> GetAnalytics(BusinessAnalyticReference reference, string referenceValue)
	{
		return DbContext.BusinessAnalytics
			.Where(e => e.Reference == reference && e.ReferenceValue == referenceValue)
			.AsQueryable();
	}

	protected IQueryable<BusinessAnalytic> GetAnalytics(BusinessAnalyticReference reference,
		string referenceValue,
		BusinessAnalyticKey analyticKey)
	{
		return DbContext.BusinessAnalytics
			.Where(e => e.Reference == reference &&
			            e.ReferenceValue == referenceValue &&
			            e.Key == analyticKey)
			.AsQueryable();
	}

	protected async Task PopulateAnalytics(BusinessAnalyticReference reference, string referenceValue)
	{
		var isAllowedReference = reference is BusinessAnalyticReference.Bean or BusinessAnalyticReference.User;
		if (!isAllowedReference || string.IsNullOrEmpty(referenceValue))
		{
			return;
		}

		var currentAnalytics = GetAnalytics(reference, referenceValue);
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
			BusinessAnalyticKey.RoastingCancellationReasonRate,

			BusinessAnalyticKey.RoastingCancellationReasonTotal,
			BusinessAnalyticKey.RoastingCancellationReasonRate,

			BusinessAnalyticKey.RoastingCancellationReasonTotal,
			BusinessAnalyticKey.RoastingCancellationReasonRate,
		});

		var currentCancellationReasonTotal = RoastingCancellationReason.NotCancelled;
		var currentCancellationReasonRate = RoastingCancellationReason.NotCancelled;
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
					case BusinessAnalyticKey.IncomingGreenBeanTotal:
						break;
					case BusinessAnalyticKey.IncomingGreenBeanWeightTotal:
						break;
					case BusinessAnalyticKey.IncomingGreenBeanWeightAverage:
						break;
					case BusinessAnalyticKey.IncomingGreenBeanLastTime:
						break;
					case BusinessAnalyticKey.RoastingTotal:
						break;
					case BusinessAnalyticKey.RoastingGreenBeanWeightTotal:
						break;
					case BusinessAnalyticKey.RoastingGreenBeanWeightAverage:
						break;
					case BusinessAnalyticKey.RoastingRoastedBeanWeightTotal:
						break;
					case BusinessAnalyticKey.RoastingRoastedBeanWeightAverage:
						break;
					case BusinessAnalyticKey.RoastingDepreciationWeightTotal:
						break;
					case BusinessAnalyticKey.RoastingDepreciationWeightAverage:
						break;
					case BusinessAnalyticKey.RoastingDepreciationRate:
						break;
					case BusinessAnalyticKey.RoastingDepreciationAverageRate:
						break;
					case BusinessAnalyticKey.RoastingFinishedTotal:
						break;
					case BusinessAnalyticKey.RoastingFinishedTotalRate:
						break;
					case BusinessAnalyticKey.RoastingCancelledTotal:
						break;
					case BusinessAnalyticKey.RoastingCancelledTotalRate:
						break;
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
					case BusinessAnalyticKey.PackagingTotal:
						break;
					case BusinessAnalyticKey.PackagingRoastedBeanRemainderWeightAverage:
						break;
					case BusinessAnalyticKey.PackagingAbsorptionRate:
						break;
					case BusinessAnalyticKey.ProductPackagedTotal:
						break;
					case BusinessAnalyticKey.ProductPackagedAverage:
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(analyticKey), analyticKey, null);
				}

				return analytic;
			})
			.ToList();

		await DbContext.BusinessAnalytics.AddRangeAsync(analytics);
	}

	protected Task PopulateAnalytics(Bean bean)
	{
		var invalidReference = bean is null ||
		                       string.IsNullOrEmpty(bean.Id) ||
		                       bean.Id == Guid.Empty.ToString();
		return invalidReference
			? Task.CompletedTask
			: PopulateAnalytics(BusinessAnalyticReference.Bean, bean.Id);
	}

	protected Task PopulateAnalytics(User user)
	{
		var invalidReference = user is null ||
		                       string.IsNullOrEmpty(user.Id) ||
		                       user.Id == Guid.Empty.ToString();
		return invalidReference
			? Task.CompletedTask
			: PopulateAnalytics(BusinessAnalyticReference.User, user.Id);
	}

	protected void ModifyAnalytics(IEnumerable<BusinessAnalytic> analytics)
	{
		DbContext.BusinessAnalytics.UpdateRange(analytics);
	}

	protected void RemoveAnalytics(Bean bean)
	{
		var invalidReference = bean is null ||
		                       string.IsNullOrEmpty(bean.Id) ||
		                       bean.Id == Guid.Empty.ToString();
		if (invalidReference)
		{
			return;
		}

		var analytics = GetAnalytics(BusinessAnalyticReference.Bean, bean.Id);
		DbContext.BusinessAnalytics.RemoveRange(analytics);
	}

	protected void RemoveAnalytics(User user)
	{
		var invalidReference = user is null ||
		                       string.IsNullOrEmpty(user.Id) ||
		                       user.Id == Guid.Empty.ToString();
		if (invalidReference)
		{
			return;
		}

		var analytics = GetAnalytics(BusinessAnalyticReference.User, user.Id);
		DbContext.BusinessAnalytics.RemoveRange(analytics);
	}

	public int Commit()
	{
		return DbContext.SaveChanges();
	}

	public Task CommitAsync()
	{
		return DbContext.SaveChangesAsync();
	}
}