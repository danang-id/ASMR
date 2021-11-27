//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 11/26/2021 9:08 AM
//
// PackagingService.cs
//

using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASMR.Web.Services;

public interface IPackagingService : IServiceBase
{
	public IQueryable<Packaging> GetAllPackagings();

	public IQueryable<Packaging> GetPackagingByUser(string userId);

	public Task<Packaging> GetPackagingById(string id);

	public Task<Packaging> CreatePackaging(Packaging packaging);

	public IQueryable<PackagingResult> GetAllPackagingResults();

	public Task<PackagingResult> CreatePackagingResult(PackagingResult packagingResult);
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

	public IQueryable<Packaging> GetPackagingByUser(string userId)
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
		return entityEntry.Entity;
	}

	public IQueryable<PackagingResult> GetAllPackagingResults()
	{
		return DbContext.PackagingResults
			.Include(e => e.Product)
			.AsQueryable();
	}

	public async Task<PackagingResult> CreatePackagingResult(PackagingResult packagingResult)
	{
		var entityEntry = await DbContext.PackagingResults.AddAsync(packagingResult);
		return entityEntry.Entity;
	}
}