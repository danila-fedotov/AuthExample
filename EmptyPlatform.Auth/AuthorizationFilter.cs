using EmptyPlatform.Auth.Db;
using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmptyPlatform.Auth
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ISessionService _sessionService;
        private readonly IUserService _userService;

        public AuthorizationFilter(ISessionService sessionService,
            IUserService userService)
        {
            _sessionService = sessionService;
            _userService = userService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var isAllowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));

                if (isAllowAnonymous)
                {
                    return;
                }

                var userId = context.HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(userId))
                {
                    context.Result = new UnauthorizedObjectResult(null);
                    return;
                }

                var user = _userService.Get(userId);
                var isValidSession = user is not null && ValidateSession(context);

                if (!isValidSession)
                {
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Result = new UnauthorizedObjectResult(null);
                    return;
                }

                var isValidPermissions = ValidatePermissions(context, user);

                if (!isValidPermissions)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch (Exception ex)
            {
                // TODO: log
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Result = new UnauthorizedObjectResult(null);
            }
        }

        protected virtual bool ValidateSession(AuthorizationFilterContext context)
        {
            var sessionValue = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication);
            var sessionId = sessionValue is null ? 0 : Convert.ToInt32(sessionValue.Value);

            if (sessionId == 0)
            {
                return false;
            }

            var session = _sessionService.Get(sessionId);

            if (session is null)
            {
                return false;
            }

            var userId = context.HttpContext.User.Identity.Name;

            if (session.UserId != userId)
            {
                // TODO: log
                _sessionService.Close(userId);
                _sessionService.Close(session.UserId);

                return false;
            }

            var device = context.HttpContext.Request.Headers["User-Agent"].ToString();

            if (session.Device != device)
            {
                // TODO: log
                _sessionService.Close(session.SessionId);

                return false;
            }

            return true;
        }

        protected virtual bool ValidatePermissions(AuthorizationFilterContext context, User user)
        {
            try
            {
                var controllerName = context.RouteData.Values["controller"].ToString().ToLower();
                var isAccountController = controllerName == "account";
                
                if (isAccountController)
                {
                    return true;
                }

                var actionName = context.RouteData.Values["action"].ToString().ToLower();
                var hasAccessToController = user.Permissions.TryGetValue(controllerName, out string[] actions);

                if (!hasAccessToController)
                {
                    return false;
                }

                return !actions.Any() || actions.Contains(actionName);
            }
            catch (Exception ex)
            {
                // TODO: log
                return false;
            }
        }
    }
}
