using System;
using System.Linq;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;

namespace ASMR.Web.Services;

public interface IBusinessAnalyticService : IServiceBase
{
	public IQueryable<BusinessAnalytic> GetAnalytics(Bean bean);

	public IQueryable<BusinessAnalytic> GetAnalytics(User user);
}

public class BusinessAnalyticService : ServiceBase, IBusinessAnalyticService
{
	public BusinessAnalyticService(ApplicationDbContext dbContext) : base(dbContext)
	{
	}

	public IQueryable<BusinessAnalytic> GetAnalytics(Bean bean)
	{
		var invalidReference = bean is null ||
		                       string.IsNullOrEmpty(bean.Id) ||
		                       bean.Id == Guid.Empty.ToString();
		return invalidReference
			? new EnumerableQuery<BusinessAnalytic>(Array.Empty<BusinessAnalytic>())
			: GetAnalytics(BusinessAnalyticReference.Bean, bean.Id);
	}

	public IQueryable<BusinessAnalytic> GetAnalytics(User user)
	{
		var invalidReference = user is null ||
		                       string.IsNullOrEmpty(user.Id) ||
		                       user.Id == Guid.Empty.ToString();
		return invalidReference
			? new EnumerableQuery<BusinessAnalytic>(Array.Empty<BusinessAnalytic>())
			: GetAnalytics(BusinessAnalyticReference.User, user.Id);
	}
}