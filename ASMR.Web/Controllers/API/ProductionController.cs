//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 8/2/2021 1:53 PM
//
// ProductionController.cs
//
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Entities;
using ASMR.Core.Generic;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API
{
	public class ProductionController : DefaultAbstractApiController<ProductionController>
	{
		private readonly IBeanService _beanService;
		private readonly IRoastedBeanProductionService _roastedBeanProductionService;
		private readonly IUserService _userService;
		
		public ProductionController(
			ILogger<ProductionController> logger, 
			IBeanService beanService,
			IRoastedBeanProductionService roastedBeanProductionService,
			IUserService userService
			) : base(logger)
		{
			_beanService = beanService;
			_roastedBeanProductionService = roastedBeanProductionService;
			_userService = userService;
		}
		
		[Authorize(Roles = "Administrator,Server,Roaster")]
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] bool showMine)
		{
			var authenticatedUser = await _userService.GetAuthenticatedUser(User.Identity);
			var roastedBeanProductions = showMine 
				? _roastedBeanProductionService
					.GetRoastedBeanProductionByUser(authenticatedUser.Id)
				: _roastedBeanProductionService
					.GetAllRoastedBeanProductions();
			
			return Ok(new ProductionsResponseModel(roastedBeanProductions));
		}
		
		[Authorize(Roles = "Administrator,Server,Roaster")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
					"Please select a production session.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}
			
			var roastedBeanProduction = await _roastedBeanProductionService
				.GetRoastedBeanProductionById(id);
			if (roastedBeanProduction is null)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
					"The production session is not found.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}
			
			return Ok(new ProductionResponseModel(roastedBeanProduction));
		}
		
		[Authorize(Roles = "Roaster")]
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


			var authenticatedUser = await _userService.GetAuthenticatedUser(User.Identity);
			var roastedBeanProduction = await _roastedBeanProductionService
				.CreateRoastedBeanProduction(new RoastedBeanProduction
				{
					BeanId = bean.Id,
					UserId = authenticatedUser.Id,
					GreenBeanWeight = model.GreenBeanWeight,
					RoastedBeanWeight = 0, 
					IsCancelled = false,
					IsFinalized = false
				});

			await _roastedBeanProductionService.CommitAsync();
			return Created(Request.Path, new ProductionResponseModel(roastedBeanProduction));
		}

		[Authorize(Roles = "Roaster")]
		[HttpPost("finalize/{id}")]
		public async Task<IActionResult> Finalize(string id, [FromBody] FinalizeProductionRequestModel model)
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
					"Please select a production session.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}

			var authenticatedUser = await _userService.GetAuthenticatedUser(User.Identity);
			var roastedBeanProduction = await _roastedBeanProductionService
				.GetRoastedBeanProductionById(id);
			if (roastedBeanProduction is null)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
					"The production session is not found.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}

			if (roastedBeanProduction.IsFinalized || roastedBeanProduction.IsCancelled)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					"Failed to cancel production because the production session has been completed.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}

			if (roastedBeanProduction.UserId != authenticatedUser.Id)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					"You are not authorized to manage this production session. " +
					"Only the user who starts the production can manage the session.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}

			if (model.RoastedBeanWeight > roastedBeanProduction.GreenBeanWeight)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					$"The roasted bean weight ({model.RoastedBeanWeight} gram(s)) should not be heavier " +
					$"than the green bean weight ({roastedBeanProduction.GreenBeanWeight} gram(s)).");
				return BadRequest(new ProductionResponseModel(errorModel));
			}
			
			var beanInventory = roastedBeanProduction.Bean.Inventory;
			beanInventory.CurrentRoastedBeanWeight += model.RoastedBeanWeight;
			await _beanService.ModifyBean(roastedBeanProduction.BeanId, new Bean
			{
				Inventory = beanInventory
			});

			roastedBeanProduction = await _roastedBeanProductionService
				.ModifyRoastedBeanProduction(roastedBeanProduction.Id, new RoastedBeanProduction
				{
					RoastedBeanWeight = model.RoastedBeanWeight,
					IsFinalized = true
				});
			
			await _roastedBeanProductionService.CommitAsync();
			return Ok(new ProductionResponseModel(roastedBeanProduction));
		}
		
		[Authorize(Roles = "Roaster")]
		[HttpDelete("cancel/{id}")]
		public async Task<IActionResult> Cancel(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
					"Please select a production session.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}

			var authenticatedUser = await _userService.GetAuthenticatedUser(User.Identity);
			var roastedBeanProduction = await _roastedBeanProductionService
				.GetRoastedBeanProductionById(id);
			if (roastedBeanProduction is null)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
					"The production session is not found.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}

			if (roastedBeanProduction.IsFinalized || roastedBeanProduction.IsCancelled)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					"Failed to cancel production because the production session has been completed.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}

			if (roastedBeanProduction.UserId != authenticatedUser.Id)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					"You are not authorized to manage this production session. " +
					"Only the user who starts the production can manage the session.");
				return BadRequest(new ProductionResponseModel(errorModel));
			}

			var beanInventory = roastedBeanProduction.Bean.Inventory;
			beanInventory.CurrentGreenBeanWeight += roastedBeanProduction.GreenBeanWeight;
			await _beanService.ModifyBean(roastedBeanProduction.BeanId, new Bean
			{
				Inventory = beanInventory
			});

			roastedBeanProduction = await _roastedBeanProductionService
				.ModifyRoastedBeanProduction(roastedBeanProduction.Id, new RoastedBeanProduction
				{
					IsCancelled = true,
					IsFinalized = true
				});
			
			await _roastedBeanProductionService.CommitAsync();
			return Ok(new ProductionResponseModel(roastedBeanProduction));
		}
	}
}