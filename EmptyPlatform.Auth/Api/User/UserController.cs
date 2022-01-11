using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmptyPlatform.Auth.Api.User
{
    /// <summary>
    /// TODO: permissions
    /// TODO: emulate a user
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Display(Description = "User management")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        private string UserId => User.Identity.Name;

        private int SessionId => Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication)?.Value);

        public UserController(IUserService userService,
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

            if (user is null)
            {
                ModelState.AddModelError("Email", "User is not found");

                return BadRequest(ModelState);
            }

            var matchPassword = _userService.MatchPassword(user, request.Password);

            if (!matchPassword)
            {
                ModelState.AddModelError("Password", "The password is incorrect");

                return BadRequest(ModelState);
            }

            var address = HttpContext.Connection.RemoteIpAddress.ToString();
            var device = HttpContext.Request.Headers["User-Agent"].ToString();
            var sessionId = _sessionService.Create(user.Id, device, address);
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id),
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
            _sessionService.Close(UserId, SessionId);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        [HttpPost]
        [Display(Description = "Creates a user")]
        public IActionResult Create(UpdateRequest request)
        {
            var user = request.Map();

            _userService.Create(user, request.ActionNote);

            return Ok();
        }

        [HttpGet]
        [Display(Description = "Returns information about the current user")]
        public IActionResult Get()
        {
            var user = _userService.Get(UserId);
            var userResponse = UserResponse.Map(user);

            return Ok(userResponse);
        }

        [HttpGet("List")]
        [Display(Description = "Returns information about all users")]
        public IActionResult GetUsers()
        {
            var users = _userService.Get();
            var userResponses = users.Select(UserResponse.Map);

            return Ok(userResponses);
        }

        [HttpPut]
        [Display(Description = "Updates a specific user")]
        public IActionResult Update(UpdateRequest request)
        {
            var user = request.Map();

            if (user is null)
            {
                ModelState.AddModelError("Id", "User is not found");

                return BadRequest(ModelState);
            }

            _userService.Update(user, request.ActionNote);

            return Ok();
        }

        [HttpDelete]
        [Display(Description = "Removes a specific user")]
        public IActionResult Remove(RemoveRequest request)
        {
            var user = _userService.Get(request.UserId);

            if (user is null)
            {
                ModelState.AddModelError("UserId", "User is not found");

                return BadRequest(ModelState);
            }

            _sessionService.Close(user.Id);
            _userService.Remove(user, request.ActionNote);

            return Ok();
        }
    }
}
