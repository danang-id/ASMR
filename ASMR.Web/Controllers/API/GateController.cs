//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 01:51 PM
//
// GateController.cs
//

using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Entities;
using ASMR.Core.Generic;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API
{
    public class GateController : DefaultAbstractApiController<GateController>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public GateController(ILogger<GateController> logger, 
            ITokenService tokenService,
            IUserService userService,
            SignInManager<User> signInManager) : base(logger)
        {
            _tokenService = tokenService;
            _userService = userService;
            _signInManager = signInManager;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] SignInRequestModel model)
        {
            var validationActionResult = GetValidationActionResult();
            if (validationActionResult is not null)
            {
                return validationActionResult;
            }
            
            var user = await _userService.GetUserByName(model.Username);
            var userRoles = (await _userService.GetUserRoles(user))
                .ToList();
            
            var signInResult = await _signInManager
                .PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
            if (signInResult.Succeeded)
            {
                var normalizedUser = _tokenService.BuildToken(user, userRoles);
                return Ok(new AuthenticationResponseModel(normalizedUser));
            }

            if (signInResult.IsLockedOut)
            {
                var lockedOutError = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
                    "We are sorry but your account is being locked because we detected some " +
                    "suspicious sign inactivity on your account.");
                return BadRequest(new AuthenticationResponseModel(lockedOutError));
            }
            
            if (signInResult.IsNotAllowed)
            {
                var notAllowedError = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
                    "Your account has not been confirmed. Please confirm your account to be able to sign in.");
                return BadRequest(new AuthenticationResponseModel(notAllowedError));
            }
            
            if (signInResult.RequiresTwoFactor)
            {
                var notAllowedError = new ResponseError(ErrorCodeConstants.RequiresTwoFactor,
                    "Two Factor authentication required.");
                return BadRequest(new AuthenticationResponseModel(notAllowedError));
            }
            
            var error = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
                "Sign in failed, please check your username and password.");
            return BadRequest(new AuthenticationResponseModel(error));
        }

        [HttpPost("exit")]
        public async Task<IActionResult> ClearSession()
        {
            await _signInManager.SignOutAsync();

            return Ok(new AuthenticationResponseModel());
        }

        [Authorize]
        [HttpGet("passport")]
        public async Task<IActionResult> GetUserPassport()
        {
            var user = await _userService.GetAuthenticatedUser(User);
            var userRoles = await _userService.GetUserRoles(user);

            return Ok(new AuthenticationResponseModel(user.ToNormalizedUserWithToken(string.Empty, userRoles)));
        }
    }
}
