using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EmptyPlatform.Auth.Api.User
{
    /// <summary>
    /// TODO: emulate a user
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Display(Description = "User management")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public UserController(IUserService userService,
            ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [HttpPost]
        [Display(Description = "User creation")]
        public IActionResult Create(CreateRequest request)
        {
            var user = request.Map();

            _userService.Create(user, request.ActionNote);

            return Ok(user.UserId);
        }

        [HttpGet]
        [Display(Description = "Viewing a user")]
        public IActionResult Get(string userId)
        {
            var user = _userService.Get(userId);
            var userResponse = UserResponse.Map(user);

            return Ok(userResponse);
        }

        [HttpGet("Users")]
        [Display(Description = "Viewing a list of users")]
        public IActionResult GetUsers()
        {
            var users = _userService.Get();
            var response = users.Select(UserResponse.Map);

            return Ok(response);
        }

        [HttpPut]
        [Display(Description = "User change")]
        public IActionResult Update(UpdateRequest request)
        {
            var user = request.Map();

            _userService.Update(user, request.ActionNote);

            return Ok();
        }

        [HttpDelete]
        [Display(Description = "Removing a user")]
        public IActionResult Remove(RemoveRequest request)
        {
            var user = _userService.Get(request.UserId);

            _userService.Remove(user.UserId, request.ActionNote);
            _sessionService.Close(user.UserId);

            return Ok();
        }
    }
}
