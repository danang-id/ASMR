using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;
using ASMR.Core.ResponseModel;
using ASMR.Web.Controllers.Attributes;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API;

public class BusinessAnalyticController : DefaultAbstractApiController<BusinessAnalyticController>
{
	private readonly IBusinessAnalyticService _businessAnalyticService;
	private readonly IBeanService _beanService;
	private readonly IUserService _userService;

	public BusinessAnalyticController(ILogger<BusinessAnalyticController> logger,
		IBusinessAnalyticService businessAnalyticService,
		IBeanService beanService,
		IUserService userService) : base(logger)
	{
		_businessAnalyticService = businessAnalyticService;
		_beanService = beanService;
		_userService = userService;
	}


	[AllowAccess(Role.Administrator, Role.Roaster, Role.Server)]
	[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS, ClientPlatform.Web)]
	[HttpGet]
	public async Task<IActionResult> GetMine()
	{
		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		var businessAnalytics = _businessAnalyticService.GetAnalytics(authenticatedUser);
		return Ok(new BusinessAnalyticsResponseModel(businessAnalytics));
	}

	[AllowAccess(Role.Administrator, Role.Roaster, Role.Server)]
	[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS, ClientPlatform.Web)]
	[HttpGet("bean/{beanId}")]
	public async Task<IActionResult> GetByBeanId(string beanId)
	{
		if (string.IsNullOrEmpty(beanId))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a bean.");
			return BadRequest(new BusinessAnalyticsResponseModel(errorModel));
		}

		var bean = await _beanService.GetBeanById(beanId);
		if (bean is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The bean of which you are trying to look analytics for is not found.");
			return BadRequest(new BusinessAnalyticsResponseModel(errorModel));
		}

		var businessAnalytics = _businessAnalyticService.GetAnalytics(bean);
		return Ok(new BusinessAnalyticsResponseModel(businessAnalytics));
	}

	[AllowAccess(Role.Administrator)]
	[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS, ClientPlatform.Web)]
	[HttpGet("user/{userId}")]
	public async Task<IActionResult> GetByUserId(string userId)
	{
		if (string.IsNullOrEmpty(userId))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a user.");
			return BadRequest(new BusinessAnalyticsResponseModel(errorModel));
		}

		var user = await _userService.GetUserById(userId);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The user of which you are trying to look analytics for is not found.");
			return BadRequest(new BusinessAnalyticsResponseModel(errorModel));
		}

		var businessAnalytics = _businessAnalyticService.GetAnalytics(user);
		return Ok(new BusinessAnalyticsResponseModel(businessAnalytics));
	}
}