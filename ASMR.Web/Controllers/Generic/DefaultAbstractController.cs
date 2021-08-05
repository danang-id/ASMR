//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/9/2021 8:16 AM
//
// DefaultAbstractController.cs
//
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.Generic
{
    [AutoValidateAntiforgeryToken]
    public abstract class DefaultAbstractController : Controller
    {
        protected readonly ILogger Logger;

        public DefaultAbstractController(ILogger logger)
        {
            Logger = logger;
        }
    }
}
