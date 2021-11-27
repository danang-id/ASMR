//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 11/26/2021 8:58 AM
// 
// PackagingController.cs
//

using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Constants;
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

public class PackagingController : DefaultAbstractApiController<PackagingController>
{
	private readonly IBeanService _beanService;
	private readonly IPackagingService _packagingService;
	private readonly IProductService _productService;
	private readonly IUserService _userService;

	public PackagingController(ILogger<PackagingController> logger,
		IBeanService beanService,
		IPackagingService packagingService,
		IProductService productService,
		IUserService userService
	) : base(logger)
	{
		_beanService = beanService;
		_packagingService = packagingService;
		_productService = productService;
		_userService = userService;
	}

	[AllowAccess(Role.Administrator, Role.Roaster, Role.Server)]
	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] bool showMine)
	{
		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		var packagings = showMine
			? _packagingService.GetPackagingByUser(authenticatedUser.Id)
			: _packagingService.GetAllPackagings();

		return Ok(new PackagingsResponseModel(packagings));
	}

	[AllowAccess(Role.Administrator, Role.Roaster, Role.Server)]
	[HttpGet]
	public async Task<IActionResult> Create([FromBody] CreatePackagingRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		var bean = await _beanService.GetBeanById(model.BeanId);
		if (bean is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The bean you are trying to package does not exist.");
			return BadRequest(new ProductResponseModel(errorModel));
		}

		if (model.Results is null || !model.Results.Any())
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Please submit at least 1 (one) packaging result.");
			return BadRequest(new ProductResponseModel(errorModel));
		}

		// TODO: Create Packaging flow

		return Ok(new PackagingResponseModel());
	}
}