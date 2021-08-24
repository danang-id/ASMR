//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/9/2021 6:06 AM
//
// ErrorController.cs
//
using System.Diagnostics;
using System.Net;
using ASMR.Core.Constants;
using ASMR.Core.Generic;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers
{
    public class ErrorController : DefaultAbstractController
    {
        public ErrorController(ILogger<ErrorController> logger): base(logger)
        {
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            if (!Request.Path.StartsWithSegments("/api"))
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var errorModel = new ResponseError(ErrorCodeConstants.GenericServerError,
                exceptionHandlerFeature.Error?.Message ?? "Something went wrong.");

            return StatusCode((int) HttpStatusCode.InternalServerError,
                new DefaultResponseModel(errorModel));
        }

        [HttpGet("not-found")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
