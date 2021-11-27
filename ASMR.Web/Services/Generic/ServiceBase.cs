//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/27/2021 12:49 PM
//
// ServiceBase.cs
//

using ASMR.Web.Data;
using System.Threading.Tasks;

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

	public int Commit()
	{
		return DbContext.SaveChanges();
	}

	public Task CommitAsync()
	{
		return DbContext.SaveChangesAsync();
	}
}