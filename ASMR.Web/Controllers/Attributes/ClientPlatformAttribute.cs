using System;
using System.Collections.Generic;
using System.Linq;
using ASMR.Core.Constants;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;
using ASMR.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASMR.Web.Controllers.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ClientPlatformAttribute : ActionFilterAttribute
{
	public IEnumerable<ClientPlatform> AllowedClientPlatforms { get; }

	public ClientPlatformAttribute(params ClientPlatform[] allowedClientPlatforms)
	{
		AllowedClientPlatforms = allowedClientPlatforms;
	}

	public bool IsClientPlatformAllowed(ClientPlatform clientPlatform)
	{
		return AllowedClientPlatforms.Any(platform => platform == clientPlatform);
	}

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var clientPlatform = context.HttpContext.Request.GetClientPlatform();
		if (!IsClientPlatformAllowed(clientPlatform))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ClientPlatformNotAllowed,
				$"This feature is not available for {clientPlatform.ToString()}.");
			context.Result = new BadRequestObjectResult(new DefaultResponseModel(errorModel));
			return;
		}

		base.OnActionExecuting(context);
	}
}