//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 01:51 PM
//
// GateController.cs
//
using System.Threading.Tasks;
using ASMR.Core.Constants;
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
    public class GateController : DefaultAbstractApiController<GateController>
    {
        private readonly IUserService _userService;

        public GateController(ILogger<GateController> logger, IUserService userService) : base(logger)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] SignInRequestModel model)
        {
            var validationActionResult = GetValidationActionResult();
            if (validationActionResult is not null)
            {
                return validationActionResult;
            }

            var user = await _userService.ValidateSignIn(model.Username, model.Password);
            if (user is null)
            {
                var error = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
                    "Sign in failed. Please check your username and password.");
                return BadRequest(new AuthenticationResponseModel(error));
            }

            await HttpContext.SignInAsync(user);
            return Ok(new AuthenticationResponseModel(user));
        }

        [HttpPost("exit")]
        public async Task<IActionResult> ClearSession()
        {

            await HttpContext.SignOutAsync();

            return Ok(new AuthenticationResponseModel());
        }

        [Authorize]
        [HttpGet("passport")]
        public async Task<IActionResult> GetUserPassport()
        {
            var user = await _userService.GetAuthenticatedUser(User.Identity);

            return Ok(new AuthenticationResponseModel(user));
        }
    }
}
