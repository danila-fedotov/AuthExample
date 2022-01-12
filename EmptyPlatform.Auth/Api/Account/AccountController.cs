using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmptyPlatform.Auth.Api.Account
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        private string UserId => User.Identity.Name;

        public AccountController(IUserService userService,
            ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync(SignInRequest request)
        {
            var user = _userService.GetByEmail(request.Email);
            var matchPassword = _userService.MatchPassword(user.UserId, request.Password);

            if (!matchPassword)
            {
                ModelState.AddModelError("Password", "The password is incorrect");

                return BadRequest(ModelState);
            }

            var address = HttpContext.Connection.RemoteIpAddress.ToString();
            var device = HttpContext.Request.Headers["User-Agent"].ToString();
            var sessionId = _sessionService.Create(user.UserId, device, address);
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserId),
                new Claim(ClaimTypes.Authentication, sessionId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Ok();
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOutAsync()
        {
            var sessionValue = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
            var sessionId = sessionValue is null ? 0 : Convert.ToInt32(sessionValue.Value);

            _sessionService.Close(sessionId);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = _userService.Get(UserId);
            var response = UserResponse.Map(user);

            return Ok(response);
        }

        [HttpPut]
        public IActionResult Update(UpdateRequest request)
        {
            var user = request.Map();

            user.UserId = UserId;

            _userService.Update(user, request.ActionNote);

            return Ok();
        }
    }
}
