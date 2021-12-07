//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// TransactionController.cs
//

using ASMR.Web.Controllers.Generic;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API;

public class TransactionController : DefaultAbstractApiController<TransactionController>
{
	public TransactionController(ILogger<TransactionController> logger) : base(logger)
	{
	}
}