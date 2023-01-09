using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVMarket.API.Attributes;
using CVMarket.API.Base;
using CVMarket.Business.Interfaces;
using CVMarket.Core.Requests;

namespace CVMarket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllersBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost, Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request)
        {
            var response = await _userService.SignUp(request);

            return APIResponse(response);
        }

        [HttpPost, Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInRequest request)
        {
            var response = await _userService.SignIn(request);

            return APIResponse(response);
        }

        [HttpGet, Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            var response = await _userService.LogOut();

            return APIResponse(response);
        }

        [HttpDelete, Route("delete/{userId}"), Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            var response = await _userService.DeleteUser(userId);

            return APIResponse(response);
        }
    }
}
