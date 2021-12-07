//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// DefaultAbstractApiController.cs
//

using System;
using System.Collections.ObjectModel;
using System.Net;
using ASMR.Core.Constants;
using ASMR.Core.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.Generic;

[Produces("application/json")]
[Route("api/[controller]")]
public abstract class DefaultAbstractApiController<T> : ControllerBase where T : class
{
	protected readonly ILogger<T> Logger;

	// ReSharper disable once ContextualLoggerProblem
	protected DefaultAbstractApiController(ILogger<T> logger)
	{
		Logger = logger;
	}

	protected bool IsAuthenticated()
	{
		return User?.Identity is not null && User.Identity.IsAuthenticated;
	}

	protected IActionResult GetValidationActionResult()
	{
		if (ModelState.IsValid)
		{
			return null;
		}

		var errors = new Collection<ResponseError>();
		foreach (var modelState in ModelState.Values)
		{
			foreach (var modelError in modelState.Errors)
			{
				errors.Add(new ResponseError(ErrorCodeConstants.ModelValidationFailed, modelError.ErrorMessage));
			}
		}

		return BadRequest(new DefaultResponseModel(errors));
	}

	protected IActionResult ShowException(Exception exception)
	{
		// ReSharper disable once TemplateIsNotCompileTimeConstantProblem
		Logger.LogError(exception, exception.Message);
		return StatusCode((int)HttpStatusCode.InternalServerError,
			new DefaultResponseModel(exception));
	}
}