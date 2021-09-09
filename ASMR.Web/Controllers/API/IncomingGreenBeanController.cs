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

namespace ASMR.Web.Controllers.API
{
	public class IncomingGreenBeanController: DefaultAbstractApiController<IncomingGreenBeanController>
	{
		private readonly IBeanService _beanService;
		private readonly IUserService _userService;

		public IncomingGreenBeanController(
			ILogger<IncomingGreenBeanController> logger,
			IBeanService unitService,
			IUserService userService) : base(logger)
		{
			_beanService = unitService;
			_userService = userService;
		}

		[AllowAccess(Role.Administrator, Role.Roaster, Role.Server)]
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] bool showMine)
		{
			var authenticatedUser = await _userService.GetAuthenticatedUser(User);
			var incomingGreenBeans = showMine
				? _beanService.GetIncomingGreenBeansByUser(authenticatedUser.Id)
				: _beanService.GetAllIncomingGreenBeans();
			
			return Ok(new IncomingGreenBeansResponseModel(incomingGreenBeans));
		}
		
		[AllowAccess(Role.Administrator, Role.Roaster, Role.Server)]
		[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS)]
		[HttpPost("{id}")]
		public async Task<IActionResult> Create(string id, [FromBody] CreateIncomingGreenBeanRequestModel model)
		{
			var validationActionResult = GetValidationActionResult();
			if (validationActionResult is not null)
			{
				return validationActionResult;
			}

			if (model.Weight <= 0)
			{
                
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					"The green bean weight to be added to the inventory must be more than 0 gram(s).");
				return BadRequest(new BeanResponseModel(errorModel));
			}

			var bean = await _beanService.GetBeanById(id, IsAuthenticated());
			if (bean is null)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
					"The bean you are trying to add to the inventory does not exist.");
				return BadRequest(new BeanResponseModel(errorModel));
			}

			var authenticatedUser = await _userService.GetAuthenticatedUser(User);
			var incomingGreenBean = await _beanService.CreateIncomingGreenBean(new IncomingGreenBean
			{
				BeanId = bean.Id,
				UserId = authenticatedUser.Id,
				WeightAdded = model.Weight
			});
            
			var beanInventory = bean.Inventory;
			beanInventory.CurrentGreenBeanWeight += incomingGreenBean.WeightAdded;
			bean = await _beanService.ModifyBean(bean.Id, new Bean
			{
				Inventory = beanInventory
			});
            
			await _beanService.CommitAsync();
			return Created(Request.Path, new BeanResponseModel(bean)
			{
				Message = $"Successfully added {incomingGreenBean.WeightAdded} gram(s) of bean '{bean.Name}' " +
				          $"to the inventory."
			});
		}
		
	}
}