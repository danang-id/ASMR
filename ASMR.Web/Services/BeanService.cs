//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 3:31 PM
//
// UnitService.cs
//

using System;
using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASMR.Web.Services;

public interface IBeanService : IServiceBase
{
	public IQueryable<Bean> GetAllBeans();

	public Task<Bean> GetBeanById(string id, bool includeSensitiveInformation = true);

	public Task<Bean> CreateBean(Bean bean);

	public Task<Bean> ModifyBean(string id, Bean bean);

	public Task<Bean> RemoveBean(string id);

	public IQueryable<IncomingGreenBean> GetAllIncomingGreenBeans();

	public IQueryable<IncomingGreenBean> GetIncomingGreenBeansByUser(string userId);

	public Task<IncomingGreenBean> CreateIncomingGreenBean(IncomingGreenBean incomingGreenBean);
}

public class BeanService : ServiceBase, IBeanService
{
	public BeanService(ApplicationDbContext dbContext) : base(dbContext)
	{
	}

	public IQueryable<Bean> GetAllBeans()
	{
		return DbContext.Beans
			.Include(e => e.Inventory)
			.AsQueryable();
	}

	public Task<Bean> GetBeanById(string id, bool includeSensitiveInformation = true)
	{
		var beans = DbContext.Beans
			.Where(e => e.Id == id);

		return includeSensitiveInformation
			? beans.Include(e => e.Inventory)
				.FirstOrDefaultAsync()
			: beans.FirstOrDefaultAsync();
	}

	public async Task<Bean> CreateBean(Bean bean)
	{
		bean.Inventory = (
			await DbContext.BeanInventories.AddAsync(new BeanInventory
			{
				CurrentGreenBeanWeight = 0,
				CurrentRoastedBeanWeight = 0
			})
		).Entity;

		var entityEntry = await DbContext.Beans.AddAsync(bean);
		return entityEntry.Entity;
	}

	public async Task<Bean> ModifyBean(string id, Bean bean)
	{
		var entity = await DbContext.Beans
			.Where(e => e.Id == id)
			.Include(e => e.Inventory)
			.Include(e => e.Products)
			.Include(e => e.IncomingGreenBeans)
			.Include(e => e.RoastingSessions)
			.FirstOrDefaultAsync();
		if (entity is null)
		{
			return null;
		}

		if (!string.IsNullOrEmpty(bean.Name))
		{
			entity.Name = bean.Name;
		}

		if (!string.IsNullOrEmpty(bean.Description))
		{
			entity.Description = bean.Description;
		}

		if (!string.IsNullOrEmpty(bean.Image))
		{
			entity.Image = bean.Image;
		}

		if (bean.Inventory is not null)
		{
			DbContext.BeanInventories.Update(bean.Inventory);
		}

		DbContext.Beans.Update(entity);
		return entity;
	}

	public async Task<Bean> RemoveBean(string id)
	{
		var entity = await DbContext.Beans
			.Where(e => e.Id == id)
			.Include(e => e.Inventory)
			.Include(e => e.Products)
			.Include(e => e.IncomingGreenBeans)
			.Include(e => e.RoastingSessions)
			.FirstOrDefaultAsync();
		if (entity is null)
		{
			return null;
		}

		DbContext.BeanInventories.Remove(entity.Inventory);
		DbContext.Beans.Remove(entity);
		return entity;
	}

	public async Task<IncomingGreenBean> CreateIncomingGreenBean(IncomingGreenBean incomingGreenBean)
	{
		var entityEntry = await DbContext.IncomingGreenBeans
			.AddAsync(incomingGreenBean);
		return entityEntry.Entity;
	}

	public IQueryable<IncomingGreenBean> GetAllIncomingGreenBeans()
	{
		return DbContext.IncomingGreenBeans
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.AsQueryable();
	}

	public IQueryable<IncomingGreenBean> GetIncomingGreenBeansByUser(string userId)
	{
		return DbContext.IncomingGreenBeans
			.Where(e => e.UserId == userId)
			.Include(e => e.User)
			.Include(e => e.Bean)
			.ThenInclude(e => e.Inventory)
			.AsQueryable();
	}
}