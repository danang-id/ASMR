//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/24/2021 12:20 AM
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