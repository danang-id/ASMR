//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 8/2/2021 1:53 PM
//
// ProductionController.cs
//

using System;
using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Web.Controllers.Attributes;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API;

public class ProductionController : DefaultAbstractApiController<ProductionController>
{
	private readonly IBeanService _beanService;
	private readonly IRoastingSessionService _roastingSessionService;
	private readonly IUserService _userService;

	public ProductionController(
		ILogger<ProductionController> logger,
		IBeanService beanService,
		IRoastingSessionService roastingSessionService,
		IUserService userService
	) : base(logger)
	{
		_beanService = beanService;
		_roastingSessionService = roastingSessionService;
		_userService = userService;
	}

	[AllowAccess(Role.Administrator, Role.Roaster, Role.Server)]
	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] bool showMine, [FromQuery] bool showCancelled)
	{
		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		var roastingSessions = showMine
			? _roastingSessionService.GetRoastingSessionsByUser(authenticatedUser.Id)
			: _roastingSessionService.GetAllRoastingSessions();
		// If cancelled production is shown, skip this filter
		if (!showCancelled)
		{
			roastingSessions = roastingSessions.Where(roasting => roasting.CancelledAt == null);
		}

		return Ok(new ProductionsResponseModel(roastingSessions));
	}

	[AllowAccess(Role.Administrator, Role.Roaster, Role.Server)]
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a roasting session.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		var roastedBeanProduction = await _roastingSessionService.GetRoastingSessionById(id);
		if (roastedBeanProduction is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The roasting session is not found.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		return Ok(new ProductionResponseModel(roastedBeanProduction));
	}

	[AllowAccess(Role.Roaster)]
	[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS)]
	[HttpPost("start")]
	public async Task<IActionResult> Start([FromBody] StartProductionRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		if (model.GreenBeanWeight <= 0)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"The green bean weight to be roasted must be more than 0 gram(s).");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		var bean = await _beanService.GetBeanById(model.BeanId, IsAuthenticated());
		if (bean is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The bean you are trying to roast does not exist.");
			return BadRequest(new BeanResponseModel(errorModel));
		}

		if (bean.Inventory.CurrentGreenBeanWeight < model.GreenBeanWeight)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"There is not enough green bean in inventory. " +
				$"You are trying to roast {model.GreenBeanWeight} gram(s) of {bean.Name} bean, " +
				$"where there's only {bean.Inventory.CurrentGreenBeanWeight} gram(s) available.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		var beanInventory = bean.Inventory;
		beanInventory.CurrentGreenBeanWeight -= model.GreenBeanWeight;
		bean = await _beanService.ModifyBean(bean.Id, new Bean
		{
			Inventory = beanInventory
		});

		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		var roastingSession = await _roastingSessionService.CreateRoastingSession(new RoastingSession
		{
			BeanId = bean.Id,
			UserId = authenticatedUser.Id,
			GreenBeanWeight = model.GreenBeanWeight,
			RoastedBeanWeight = 0
		});

		await _roastingSessionService.CommitAsync();
		return Created(Request.Path, new ProductionResponseModel(roastingSession)
		{
			Message = $"Successfully started roasting session for '{bean.Name}' bean."
		});
	}

	[AllowAccess(Role.Roaster)]
	[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS)]
	[HttpPost("finish/{id}")]
	public async Task<IActionResult> Finish(string id, [FromBody] FinishProductionRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		if (model.RoastedBeanWeight <= 0)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"The roasted bean weight must be more than 0 gram(s).");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a roasting session.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		var roastingSession = await _roastingSessionService.GetRoastingSessionById(id);
		if (roastingSession is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The roasting session is not found.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		if (roastingSession.CancelledAt is not null || roastingSession.FinishedAt is not null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Failed to finish roasting because the roasting session has been completed.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		if (roastingSession.UserId != authenticatedUser.Id)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"You are not authorized to manage this roasting session. " +
				"Only the user who starts the roasting can manage the session.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		if (model.RoastedBeanWeight > roastingSession.GreenBeanWeight)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				$"The roasted bean weight ({model.RoastedBeanWeight} gram(s)) should not be heavier " +
				$"than the green bean weight ({roastingSession.GreenBeanWeight} gram(s)).");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		var beanInventory = roastingSession.Bean.Inventory;
		beanInventory.CurrentRoastedBeanWeight += model.RoastedBeanWeight;
		await _beanService.ModifyBean(roastingSession.BeanId, new Bean
		{
			Inventory = beanInventory
		});

		roastingSession = await _roastingSessionService
			.ModifyRoastingSession(roastingSession.Id, new RoastingSession
			{
				RoastedBeanWeight = model.RoastedBeanWeight,
				FinishedAt = DateTimeOffset.Now
			});

		await _roastingSessionService.CommitAsync();
		return Ok(new ProductionResponseModel(roastingSession)
		{
			Message = $"Successfully finished the roasting for '{roastingSession.Bean.Name}' bean."
		});
	}

	[AllowAccess(Role.Roaster)]
	[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS)]
	[HttpDelete("cancel/{id}")]
	public async Task<IActionResult> Cancel(string id, [FromQuery] bool isBeanBurnt)
	{
		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a roasting session.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		var roastingSession = await _roastingSessionService
			.GetRoastingSessionById(id);
		if (roastingSession is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The roasting session is not found.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		if (roastingSession.CancelledAt is not null || roastingSession.FinishedAt is not null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Failed to cancel roasting because the roasting session has been completed.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		if (roastingSession.UserId != authenticatedUser.Id)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"You are not authorized to manage this roasting session. " +
				"Only the user who starts the roasting can manage the session.");
			return BadRequest(new ProductionResponseModel(errorModel));
		}

		if (!isBeanBurnt)
		{
			var beanInventory = roastingSession.Bean.Inventory;
			beanInventory.CurrentGreenBeanWeight += roastingSession.GreenBeanWeight;
			await _beanService.ModifyBean(roastingSession.BeanId, new Bean
			{
				Inventory = beanInventory
			});
		}

		roastingSession = await _roastingSessionService
			.ModifyRoastingSession(roastingSession.Id, new RoastingSession
			{
				CancelledAt = DateTimeOffset.Now
			});

		await _roastingSessionService.CommitAsync();
		return Ok(new ProductionResponseModel(roastingSession)
		{
			Message = $"Successfully cancelled the roasting for '{roastingSession.Bean.Name}' bean."
		});
	}
}