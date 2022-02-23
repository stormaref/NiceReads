using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NiceReads.Services;

namespace NiceReads.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _userService.Login(request.Username, request.Password);
            if (!result.succeeded)
            {
                return BadRequest("Username or password is wrong");
            }
            return Ok(result.token);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            var result = await _userService.SignUp(request.Username, request.Password, request.FirstName, request.LastName);
            if (!result.succeeded)
            {
                return BadRequest(result.error);
            }
            return Ok(result.token);
        }
    }

    public class SignUpRequest : LoginRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

