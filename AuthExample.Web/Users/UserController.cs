using AuthExample.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AuthExample.Web.Users
{
    /// <summary>
    /// TODO: json format
    /// TODO: session
    /// TODO: model validation
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private string UserId => User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult LogIn(SignInDto signInDto)
        {
            var user = _userService.GetByEmail(signInDto.Email);

            if (user is null)
            {
                return BadRequest("User is not found");
            }

            var isMatchPassword = _userService.MatchPassword(user, signInDto.Password);

            if (!isMatchPassword)
            {
                return BadRequest("The password is incorrect");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var signInResult = SignIn(claimsPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);

            return signInResult;
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            var signOutResult = SignOut(CookieAuthenticationDefaults.AuthenticationScheme);

            return signOutResult;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = _userService.Get(UserId);
            var userDto = UserDto.Map(user);

            return Ok(userDto);
        }

        [HttpPost]
        public IActionResult Update(UserDto userDto, string actionNote)
        {
            _userService.Update(userDto.UserEntity, actionNote);

            return Ok();
        }

        [HttpPost]
        public IActionResult Delete([Required] string id, string actionNote)
        {
            _userService.Delete(id, actionNote);

            return Ok();
        }
    }
}
