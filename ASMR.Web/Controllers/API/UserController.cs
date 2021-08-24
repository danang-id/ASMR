//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/22/2021 4:45 PM
//
// ProductController.cs
//
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Extensions;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// ReSharper disable ReplaceWithSingleCallToAny
namespace ASMR.Web.Controllers.API
{
    public class UserController : DefaultAbstractApiController<UserController>
    {
        private readonly IMediaFileService _mediaFileService;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public UserController(
            ILogger<UserController> logger,
            IMediaFileService mediaFileService,
            IUserService userService,
            SignInManager<User> signInManager) : base(logger)
        {
            _mediaFileService = mediaFileService;
            _userService = userService;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = new Collection<NormalizedUser>();
            foreach (var user in _userService.GetAllUsers())
            {
                var userRoles = await _userService.GetUserRoles(user);
                users.Add(user.ToNormalizedUser(userRoles));
            }
            
            return Ok(new UsersResponseModel(users));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
                    "Please select a user");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var user = await _userService.GetUserById(id);
            if (user is null)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
                    "The user you are looking for is not found.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var userRoles = await _userService.GetUserRoles(user);

            return Ok(new UserResponseModel(user.ToNormalizedUser(userRoles)));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] CreateUserRequestModel model)
        {
            var validationActionResult = GetValidationActionResult();
            if (validationActionResult is not null)
            {
                return validationActionResult;
            }

            var existingUser = await _userService.GetUserByName(model.Username);
            if (existingUser is not null)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    $"User with username {model.Username} is already exist.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            if (model.Roles is null || !model.Roles.Any())
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "Please assign minimal a role.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            if (model.Roles.Where(role => role == Role.Administrator).Any())
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "Assigning Administrator role is not allowed.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var formCollection = await Request.ReadFormAsync();
            var formFile = formCollection.Files.GetFile("image");
            if (formFile is null || formFile.Length <= 0)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "Please choose an image for the user.");
                return BadRequest(new UserResponseModel(errorModel));
            }
            if (formFile.Length > 2 * 1024 * 1024)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "The user image file size must be under 2 MB.");
                return BadRequest(new UserResponseModel(errorModel));
            }
            if (!formFile.IsImage(Logger))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "The file you uploaded is not an image.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var userId = Guid.NewGuid().ToString();
            var mediaFile = await _mediaFileService.CreateMediaFile(userId, formFile);

            var user = await _userService.CreateUser(new User
            {
                Id = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.EmailAddress,
                EmailConfirmed = true,
                UserName = model.Username,
                Image = $"/api/mediafile/{mediaFile.Id}"
            }, model.Password);

            user = await _userService.AssignRolesToUser(user.Id, model.Roles);
            var userRoles = await _userService.GetUserRoles(user);
            
            await _userService.CommitAsync();

            return Created(Request.Path, new UserResponseModel(user.ToNormalizedUser(userRoles)));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Modify(string id, [FromForm] UpdateUserRequestModel model)
        {
            var validationActionResult = GetValidationActionResult();
            if (validationActionResult is not null)
            {
                return validationActionResult;
            }

            if (string.IsNullOrEmpty(id))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
                    "Please select a user.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var user = await _userService.GetUserById(id);
            if (user is null)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
                    "The user you are trying to modify is not found.");
                return BadRequest(new UserResponseModel(errorModel));
            }
            
            if (user.UserName != model.Username)
            {
                var existingUser = await _userService.GetUserByName(model.Username);
                if (existingUser is not null)
                {
                    var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                        $"User with username {model.Username} is already exist.");
                    return BadRequest(new UserResponseModel(errorModel));
                }
            }

            if (model.Roles is not null && !model.Roles.Any())
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "Please assign minimal a role.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var modelHasRoles = model.Roles is not null && model.Roles.Any();
            var modelRoleIncludesAdministrator = modelHasRoles && model.Roles.Where(role => role == Role.Administrator).Any();
            var userIsAdministrator = await _userService.HasRole(user, Role.Administrator);
            
            if (modelRoleIncludesAdministrator && !userIsAdministrator)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                    "Assigning Administrator role is not allowed.");
                return BadRequest(new UserResponseModel(errorModel));
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
                                "Please choose an image for the user.");
                            return BadRequest(new ProductResponseModel(errorModel));
                        }
                    case > 2 * 1024 * 1024:
                        {
                            var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                                "The user image file size must be under 2 MB.");
                            return BadRequest(new ProductResponseModel(errorModel));
                        }
                }

                if (!formFile.IsImage(Logger))
                {
                    var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
                        "The file you uploaded is not an image.");
                    return BadRequest(new ProductResponseModel(errorModel));
                }

                mediaFile = await _mediaFileService.CreateMediaFile(user.Id, formFile);
            }

            var oldMediaFileId = user.Image.Split("/")
                .LastOrDefault();

            user = await _userService.ModifyUser(id, new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.EmailAddress,
                UserName = model.Username,
                Image = mediaFile is null ? null : $"/api/mediafile/{mediaFile.Id}"
            });
            if (modelHasRoles)
            {
                await _userService.AssignRolesToUser(id, model.Roles);
            }

            var authenticatedUser = await _userService.GetAuthenticatedUser(User);
            if (user is not null && user.Id == authenticatedUser.Id)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, true);
            }

            if (mediaFile is not null && string.IsNullOrEmpty(oldMediaFileId))
            {
                await _mediaFileService.RemoveMediaFile(oldMediaFileId);
            }
            var userRoles = await _userService.GetUserRoles(user);

            await _userService.CommitAsync();
            return Ok(new UserResponseModel(user?.ToNormalizedUser(userRoles)));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch("{id}/password")]
        public async Task<IActionResult> ModifyPassword(string id, [FromBody] UpdateUserPasswordRequestModel model)
        {
            var validationActionResult = GetValidationActionResult();
            if (validationActionResult is not null)
            {
                return validationActionResult;
            }

            if (string.IsNullOrEmpty(id))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
                    "Please select a user.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var user = await _userService.ModifyUserPassword(id, model.Password);
            if (user is null)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
                    "The user you are trying to modify is not found.");
                return BadRequest(new UserResponseModel(errorModel));
            }
            
            var authenticatedUser = await _userService.GetAuthenticatedUser(User);
            if (user.Id == authenticatedUser.Id)
            {
                await _signInManager.SignOutAsync();
            }
            var userRoles = await _userService.GetUserRoles(user);
            
            await _userService.CommitAsync();
            return Ok(new UserResponseModel(user.ToNormalizedUser(userRoles)));
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
                    "Please select a user.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var user = await _userService.RemoveUser(id);
            if (user is null)
            {
                var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
                    "The user you are trying to remove is not found.");
                return BadRequest(new UserResponseModel(errorModel));
            }

            var mediaFileId = user.Image.Split("/").LastOrDefault();
            if (mediaFileId is not null)
            {
                await _mediaFileService.RemoveMediaFile(mediaFileId);
            }
            var userRoles = await _userService.GetUserRoles(user);
            
            await _userService.CommitAsync();
            return Ok(new UserResponseModel(user.ToNormalizedUser(userRoles)));
        }
    }
}
