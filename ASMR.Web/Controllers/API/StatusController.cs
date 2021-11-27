//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/9/2021 8:49 AM
//
// StatusController.cs
//

using ASMR.Core.ResponseModel;
using ASMR.Web.Controllers.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API;

public class StatusController : DefaultAbstractApiController<StatusController>
{
	public StatusController(ILogger<StatusController> logger) : base(logger)
	{
	}

	[HttpGet]
	public IActionResult GetStatus()
	{
		var applicationVersion = GetType().Assembly.GetName().Version;
		var runtimeVersion = System.Environment.Version;

		return Ok(new StatusResponseModel(true, applicationVersion, runtimeVersion));
	}
}