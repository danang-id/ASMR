//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/24/2021 12:21 AM
//
// TransactionController.cs
//
using ASMR.Web.Controllers.Generic;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API
{
    public class TransactionController : DefaultAbstractApiController<TransactionController>
    {
        public TransactionController(ILogger<TransactionController> logger) : base(logger)
        {
        }
    }
}