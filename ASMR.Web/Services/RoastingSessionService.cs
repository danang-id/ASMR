//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 11:54 PM
//
// RoastingSessionService.cs
//

using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASMR.Web.Services;

public interface IRoastingSessionService : IServiceBase
{
	public IQueryable<RoastingSession> GetAllRoastingSessions();

	public IQueryable<RoastingSession> GetRoastingSessionsByUser(string userId);

	public Task<RoastingSession> GetRoastingSessionById(string id);

	public Task<RoastingSession> CreateRoastingSession(RoastingSession roastingSession);

	public Task<RoastingSession> ModifyRoastingSession(string id,
		RoastingSession roastingSession);
}

public class RoastingSessionService : ServiceBase, IRoastingSessionService
{
	public RoastingSessionService(ApplicationDbContext dbContext) : base(dbContext)
	{
	}

	public IQueryable<RoastingSession> GetAllRoastingSessions()
	{
		return DbContext.RoastingSessions
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.AsQueryable();
	}

	public IQueryable<RoastingSession> GetRoastingSessionsByUser(string userId)
	{
		return DbContext.RoastingSessions
			.Where(e => e.UserId == userId)
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.AsQueryable();
	}

	public Task<RoastingSession> GetRoastingSessionById(string id)
	{
		return DbContext.RoastingSessions
			.Where(e => e.Id == id)
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.FirstOrDefaultAsync();
	}

	public async Task<RoastingSession> CreateRoastingSession(RoastingSession roastingSession)
	{
		var entityEntry = await DbContext.RoastingSessions.AddAsync(roastingSession);
		return entityEntry.Entity;
	}

	public async Task<RoastingSession> ModifyRoastingSession(string id,
		RoastingSession roastingSession)
	{
		var entity = await DbContext.RoastingSessions
			.Where(e => e.Id == id)
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.FirstOrDefaultAsync();
		if (entity is null)
		{
			return null;
		}

		if (roastingSession.GreenBeanWeight > 0)
		{
			entity.GreenBeanWeight = roastingSession.GreenBeanWeight;
		}

		if (roastingSession.RoastedBeanWeight > 0)
		{
			entity.RoastedBeanWeight = roastingSession.RoastedBeanWeight;
		}

		var cancelOrFinishAllowed = entity.CancelledAt is null && entity.FinishedAt is null;
		if (cancelOrFinishAllowed && roastingSession.CancelledAt is not null)
		{
			entity.CancelledAt = roastingSession.CancelledAt;
		}

		if (cancelOrFinishAllowed && roastingSession.FinishedAt is not null)
		{
			entity.FinishedAt = roastingSession.FinishedAt;
		}

		DbContext.RoastingSessions.Update(entity);
		return entity;
	}
}