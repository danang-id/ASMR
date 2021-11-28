//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 11/26/2021 9:08 AM
//
// PackagingService.cs
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

public interface IPackagingService : IServiceBase
{
	public IQueryable<Packaging> GetAllPackagings();

	public IQueryable<Packaging> GetPackagingByUserId(string userId);

	public Task<Packaging> GetPackagingById(string id);

	public Task<Packaging> CreatePackaging(Packaging packaging);

	public Task<Packaging> RemovePackaging(string id);

	public Task<PackagingResult> RemovePackagingResult(string id);
}

public class PackagingService : ServiceBase, IPackagingService
{
	public PackagingService(ApplicationDbContext dbContext) : base(dbContext)
	{
	}

	public IQueryable<Packaging> GetAllPackagings()
	{
		return DbContext.Packagings
			.Include(e => e.Results)
			.ThenInclude(e => e.Product)
			.AsQueryable();
	}

	public IQueryable<Packaging> GetPackagingByUserId(string userId)
	{
		return DbContext.Packagings
			.Where(e => e.UserId == userId)
			.Include(e => e.Results)
			.ThenInclude(e => e.Product)
			.AsQueryable();
	}

	public Task<Packaging> GetPackagingById(string id)
	{
		return DbContext.Packagings
			.Where(e => e.Id == id)
			.Include(e => e.Results)
			.ThenInclude(e => e.Product)
			.FirstOrDefaultAsync();
	}

	public async Task<Packaging> CreatePackaging(Packaging packaging)
	{
		var entityEntry = await DbContext.Packagings.AddAsync(packaging);
		var bean = await DbContext.Beans.Where(e => e.Id == entityEntry.Entity.BeanId)
			.Include(e => e.Inventory)
			.FirstOrDefaultAsync();
		var user = await DbContext.Users.Where(e => e.Id == entityEntry.Entity.UserId)
			.FirstOrDefaultAsync();
		var availableRoastedBeanWeight = bean?.Inventory.CurrentRoastedBeanWeight ?? 0;
		var usedRoastedBeanWeight = packaging.Results.Sum(e => e.Product.WeightPerPackaging * e.Quantity);

		async Task ProgressAnalytics(IQueryable<BusinessAnalytic> analytics)
		{
			var total = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.PackagingTotal)
				.FirstOrDefaultAsync();
			var roastedBeanRemainderWeightAverage = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.PackagingRoastedBeanRemainderWeightAverage)
				.FirstOrDefaultAsync();
			var absorptionRate = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.PackagingAbsorptionRate)
				.FirstOrDefaultAsync();
			var productPackagedTotal = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.ProductPackagedTotal)
				.FirstOrDefaultAsync();
			var productPackagedAverage = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.ProductPackagedAverage)
				.FirstOrDefaultAsync();

			if (total is null ||
			    roastedBeanRemainderWeightAverage is null ||
			    absorptionRate is null ||
			    productPackagedTotal is null ||
			    productPackagedAverage is null ||
			    availableRoastedBeanWeight == 0)
			{
				return;
			}

			var currentRoastedBeanRemainderWeight = roastedBeanRemainderWeightAverage.DecimalValue * total.IntValue;
			currentRoastedBeanRemainderWeight += (availableRoastedBeanWeight - usedRoastedBeanWeight);
			var currentAbsorptionRate = absorptionRate.DecimalValue * total.IntValue;
			currentAbsorptionRate += (usedRoastedBeanWeight / availableRoastedBeanWeight);

			total.IntValue++;
			roastedBeanRemainderWeightAverage.DecimalValue = currentRoastedBeanRemainderWeight / total.IntValue;
			absorptionRate.DecimalValue = currentAbsorptionRate / total.IntValue;
			productPackagedTotal.IntValue += packaging.Results.Count();
			productPackagedAverage.DecimalValue = (decimal)productPackagedTotal.IntValue / total.IntValue;

			ModifyAnalytics(new List<BusinessAnalytic>
			{
				total, roastedBeanRemainderWeightAverage, absorptionRate, productPackagedTotal, productPackagedAverage
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

	public async Task<Packaging> RemovePackaging(string id)
	{
		var packaging = await DbContext.Packagings
			.Where(e => e.Id == id)
			.Include(e => e.Results)
			.ThenInclude(e => e.Product)
			.FirstOrDefaultAsync();
		if (packaging is null)
		{
			return null;
		}

		var entityEntry = DbContext.Packagings.Remove(packaging);
		var bean = await DbContext.Beans.Where(e => e.Id == entityEntry.Entity.BeanId)
			.Include(e => e.Inventory)
			.FirstOrDefaultAsync();
		var user = await DbContext.Users.Where(e => e.Id == entityEntry.Entity.UserId)
			.FirstOrDefaultAsync();

		async Task ProgressAnalytics(IQueryable<BusinessAnalytic> analytics)
		{
			var total = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.PackagingTotal)
				.FirstOrDefaultAsync();
			// It is not feasible to touch BusinessAnalyticKey.PackagingRoastedBeanRemainderWeightAverage
			// and BusinessAnalyticKey.PackagingAbsorptionRate because it is not possible to get how much
			// available roasted bean weight it was when this packaging was created.
			var productPackagedTotal = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.ProductPackagedTotal)
				.FirstOrDefaultAsync();
			var productPackagedAverage = await analytics
				.Where(e => e.Key == BusinessAnalyticKey.ProductPackagedAverage)
				.FirstOrDefaultAsync();

			if (total is null ||
			    productPackagedTotal is null ||
			    productPackagedAverage is null)
			{
				return;
			}

			total.IntValue--;
			productPackagedTotal.IntValue -= packaging.Results.Count();
			productPackagedAverage.DecimalValue = (decimal)productPackagedTotal.IntValue / total.IntValue;

			ModifyAnalytics(new List<BusinessAnalytic>
			{
				total, productPackagedTotal, productPackagedAverage
			});
		}

		var beanAnalytics = GetAnalytics(BusinessAnalyticReference.Bean, bean?.Id);
		var userAnalytics = GetAnalytics(BusinessAnalyticReference.User, user?.Id);
		await Task.WhenAll(new List<Task>
		{
			ProgressAnalytics(beanAnalytics),
			ProgressAnalytics(userAnalytics)
		});

		return packaging;
	}

	public async Task<PackagingResult> RemovePackagingResult(string id)
	{
		var packagingResult = await DbContext.PackagingResults
			.Where(e => e.Id == id)
			.Include(e => e.Product)
			.FirstOrDefaultAsync();
		if (packagingResult is null)
		{
			return null;
		}

		var entityEntry = DbContext.PackagingResults.Remove(packagingResult);
		return entityEntry.Entity;
	}
}