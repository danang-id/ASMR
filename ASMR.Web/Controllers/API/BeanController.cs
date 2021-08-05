//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 3:29 PM
//
// RawMaterialController.cs
//
using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Entities;
using ASMR.Core.Generic;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Extensions;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API
{
    public class BeanController : DefaultAbstractApiController<BeanController>
    {
        private readonly IBeanService _beanService;
        private readonly IMediaFileService _mediaFileService;
        private readonly IUserService _userService;

        public BeanController(
            ILogger<BeanController> logger,
            IBeanService unitService,
            IMediaFileService mediaFileService,
            IUserService userService) : base(logger)
        {
            _beanService = unitService;
            _mediaFileService = mediaFileService;
            _userService = userService;
        }

        [Authorize(Roles = "Administrator,Server,Roaster")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var beans = _beanService.GetAllBeans();
            return Ok(new BeansResponseModel(beans));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
                    "Please select a bean.");
                return BadRequest(new BeanResponseModel(errorModel));
            }

            var bean = await _beanService.GetBeanById(id, IsAuthenticated());
            if (bean is null)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
                    "The bean you are looking for is not found.");
                return BadRequest(new BeanResponseModel(errorModel));
            }

            return Ok(new BeanResponseModel(bean));
        }

        [Authorize(Roles = "Administrator,Server")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] CreateBeanRequestModel model)
        {
            var validationActionResult = GetValidationActionResult();
            if (validationActionResult is not null)
            {
                return validationActionResult;
            }

            var formCollection = await Request.ReadFormAsync();
            var formFile = formCollection.Files.GetFile("image");
            if (formFile is null || formFile.Length <= 0)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "Please choose an image for the bean.");
                return BadRequest(new ProductResponseModel(errorModel));
            }
            if (formFile.Length > 2 * 1024 * 1024)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "The bean image file size must be under 2 MB.");
                return BadRequest(new ProductResponseModel(errorModel));
            }
            if (!formFile.IsImage())
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "The file you uploaded is not an image.");
                return BadRequest(new ProductResponseModel(errorModel));
            }

            var authenticatedUser = await _userService.GetAuthenticatedUser(User.Identity);
            var mediaFile = await _mediaFileService.CreateMediaFile(authenticatedUser, formFile);

            var bean = await _beanService.CreateBean(new Bean
            {
                Name = model.Name,
                Description = model.Description,
                Image = $"/api/mediafile/{mediaFile.Id}",
            });

            await _beanService.CommitAsync();
            return Created(Request.Path, new BeanResponseModel(bean));
        }

        [Authorize(Roles = "Administrator,Server")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Modify(string id, [FromForm] UpdateBeanRequestModel model)
        {
            var validationActionResult = GetValidationActionResult();
            if (validationActionResult is not null)
            {
                return validationActionResult;
            }

            if (string.IsNullOrEmpty(id))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
                    "Please select a bean.");
                return BadRequest(new BeanResponseModel(errorModel));
            }

            MediaFile mediaFile = null;
            var formCollection = await Request.ReadFormAsync();
            var formFile = formCollection.Files.GetFile("image");
            if (formFile is not null)
            {
                switch (formFile.Length)
                {
                    case <= 0:
                    {
                        var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                            "Please choose an image for the bean.");
                        return BadRequest(new ProductResponseModel(errorModel));
                    }
                    case > 2 * 1024 * 1024:
                    {
                        var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                            "The bean image file size must be under 2 MB.");
                        return BadRequest(new ProductResponseModel(errorModel));
                    }
                }

                if (!formFile.IsImage())
                {
                    var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                        "The file you uploaded is not an image.");
                    return BadRequest(new ProductResponseModel(errorModel));
                }

                var authenticatedUser = await _userService.GetAuthenticatedUser(User.Identity);
                mediaFile = await _mediaFileService.CreateMediaFile(authenticatedUser, formFile);
            }

            string oldMediaFileId = null;
            var bean = await _beanService.GetBeanById(id, IsAuthenticated());
            if (bean is not null)
            {
                oldMediaFileId = bean.Image.Split("/").LastOrDefault();
            }

            bean = await _beanService.ModifyBean(id, new Bean
            {
                Name = model.Name,
                Description = model.Description,
                Image = mediaFile is null ? null : $"/api/mediafile/{mediaFile.Id}"
            });
            if (bean is null)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
                    "The bean you are trying to modify is not found.");
                return BadRequest(new BeanResponseModel(errorModel));
            }

            if (mediaFile is not null && string.IsNullOrEmpty(oldMediaFileId))
            {
                await _mediaFileService.RemoveMediaFile(oldMediaFileId);
            }

            await _beanService.CommitAsync();
            return Ok(new BeanResponseModel(bean));
        }

        [Authorize(Roles = "Administrator,Server")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
                    "Please select a bean.");
                return BadRequest(new BeanResponseModel(errorModel));
            }

            var bean = await _beanService.RemoveBean(id);
            if (bean is null)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
                    "The bean you are trying to remove is not found.");
                return BadRequest(new BeanResponseModel(errorModel));
            }

            var mediaFileId = bean.Image.Split("/").LastOrDefault();
            if (mediaFileId is not null)
            {
                await _mediaFileService.RemoveMediaFile(mediaFileId);
            }

            await _beanService.CommitAsync();
            return Ok(new BeanResponseModel(bean));
        }
    }
}
