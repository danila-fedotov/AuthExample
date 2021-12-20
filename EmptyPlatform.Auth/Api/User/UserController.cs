using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmptyPlatform.Auth.Api
{
    /// <summary>
    /// TODO: model validation
    /// TODO: permissions
    /// TODO: session
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private string UserId => User.Identity.Name;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync(SignInDto signInDto)
        {
            var user = _userService.GetByEmail(signInDto.Email);

            if (user is null)
            {
                ModelState.AddModelError("Email", "User is not found");

                return BadRequest(ModelState);
            }

            var isMatchPassword = _userService.MatchPassword(user, signInDto.Password);

            if (!isMatchPassword)
            {
                ModelState.AddModelError("Password", "The password is incorrect");

                return BadRequest(ModelState);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Ok();
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        [HttpPost]
        public IActionResult Create(UserDto userDto, string actionNote)
        {
            var user = userDto.Map();

            _userService.Create(user, actionNote);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = _userService.Get(UserId);
            var userDto = UserDto.Map(user);

            return Ok(userDto);
        }

        [HttpGet("List")]
        public IActionResult GetUsers()
        {
            var users = _userService.Get();
            var userDtos = users.Select(UserDto.Map);

            return Ok(userDtos);
        }

        [HttpPut]
        public IActionResult Update(UserDto userDto, string actionNote)
        {
            var user = userDto.Map();

            if (user is null)
            {
                ModelState.AddModelError("Id", "User is not found");

                return BadRequest(ModelState);
            }

            _userService.Update(user, actionNote);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Remove(RemoveDto removeDto)
        {
            var user = _userService.Get(removeDto.UserId);

            if (user is null)
            {
                ModelState.AddModelError("UserId", "User is not found");

                return BadRequest(ModelState);
            }

            _userService.Remove(user, removeDto.ActionNote);

            return Ok();
        }
    }
}
