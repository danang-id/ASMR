//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// TransactionService.cs
//

using ASMR.Web.Data;
using ASMR.Web.Services.Generic;

namespace ASMR.Web.Services;

public interface ITransactionService : IServiceBase
{
}

public class TransactionService : ServiceBase, ITransactionService
{
	public TransactionService(ApplicationDbContext dbContext) : base(dbContext)
	{
	}
}