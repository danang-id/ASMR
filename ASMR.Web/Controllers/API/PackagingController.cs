//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 11/26/2021 8:58 AM
// 
// PackagingController.cs
//

using System.Collections.ObjectModel;
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
			? _packagingService.GetPackagingByUserId(authenticatedUser.Id)
			: _packagingService.GetAllPackagings();

		return Ok(new PackagingsResponseModel(packagings));
	}

	[AllowAccess(Role.Roaster)]
	[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS)]
	[HttpPost]
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
			return BadRequest(new PackagingResponseModel(errorModel));
		}

		if (model.Results is null || !model.Results.Any())
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Please submit at least 1 (one) packaging result.");
			return BadRequest(new PackagingResponseModel(errorModel));
		}

		var beanInventory = bean.Inventory;
		var packagingResults = new Collection<PackagingResult>();
		foreach (var packagingResultModel in model.Results)
		{
			if (packagingResultModel.Quantity <= 0)
			{
				continue;
			}

			var product = await _productService.GetProductById(packagingResultModel.ProductId);
			if (product is null)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
					"The product you are trying to package to does not exist.");
				return BadRequest(new PackagingResponseModel(errorModel));
			}

			beanInventory.CurrentRoastedBeanWeight -= packagingResultModel.Quantity * product.WeightPerPackaging;
			if (beanInventory.CurrentRoastedBeanWeight < 0)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ResourceEmpty,
					"There is not enough roasted bean in the inventory to make this packaging.");
				return BadRequest(new PackagingResponseModel(errorModel));
			}

			packagingResults.Add(new PackagingResult
			{
				Product = product,
				ProductId = product.Id,
				Quantity = packagingResultModel.Quantity
			});
		}

		// Remaining roasted bean is OK, save that to roasted bean inventory
		await _beanService.ModifyBean(bean.Id, new Bean
		{
			Inventory = beanInventory
		});

		// Then, it is time to calculate the products in the inventory
		foreach (var packagingResult in packagingResults)
		{
			var product = packagingResult.Product;
			await _productService.ModifyProduct(product.Id, new Product
			{
				CurrentInventoryQuantity = product.CurrentInventoryQuantity + packagingResult.Quantity
			});
		}

		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		var packaging = await _packagingService.CreatePackaging(new Packaging
		{
			BeanId = bean.Id,
			UserId = authenticatedUser.Id,
			Results = packagingResults
		});

		await _packagingService.CommitAsync();
		return Ok(new PackagingResponseModel(packaging)
		{
			Message = $"Successfully created packaging of {packagingResults.Count} product(s) of {bean.Name}."
		});
	}

	[AllowAccess(Role.Roaster)]
	[ClientPlatform(ClientPlatform.Android, ClientPlatform.iOS)]
	[HttpDelete("{id}")]
	public async Task<IActionResult> Remove(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a product packaging.");
			return BadRequest(new PackagingResponseModel(errorModel));
		}

		var packaging = await _packagingService.GetPackagingById(id);
		if (packaging is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The product packaging you are trying to remove is not found.");
			return BadRequest(new PackagingResponseModel(errorModel));
		}

		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		if (packaging.UserId != authenticatedUser.Id)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"You are not authorized to remove this product packaging. " +
				"Only the user who creates the product packaging can remove them.");
			return BadRequest(new PackagingResponseModel(errorModel));
		}

		var bean = await _beanService.GetBeanById(packaging.BeanId);
		if (bean is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The bean in which this product was packaged does not exist anymore.");
			return BadRequest(new PackagingResponseModel(errorModel));
		}

		var removePackagingResultTasks = new Collection<Task<PackagingResult>>();
		foreach (var packagingResult in packaging.Results)
		{
			removePackagingResultTasks.Add(_packagingService.RemovePackagingResult(packagingResult.Id));
		}

		await Task.WhenAll(removePackagingResultTasks);
		packaging = await _packagingService.RemovePackaging(packaging.Id);

		await _packagingService.CommitAsync();
		return Ok(new PackagingResponseModel(packaging)
		{
			Message = $"Successfully removed packaging of {packaging.Results.Count()} product(s) of {bean.Name}."
		});
	}
}