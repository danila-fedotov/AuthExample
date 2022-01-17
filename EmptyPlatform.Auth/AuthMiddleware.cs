using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EmptyPlatform.Auth
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ISessionService sessionService, IUserService userService)
        {
            var isAllowAnonymous = context.GetEndpoint().Metadata.GetMetadata<AllowAnonymousAttribute>() is not null;

            if (!isAllowAnonymous)
            {
                var userId = context.User.Identity.Name;

                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var user = userService.Get(userId);
                var isValidSession = user is not null && sessionService.Validate();

                if (!isValidSession)
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var controllerName = context.Request.RouteValues["controller"].ToString().ToLower();
                var actionName = context.Request.RouteValues["action"].ToString().ToLower();
                var hasAccess = userService.ValidateAccess(user.Permissions, controllerName, actionName);

                if (!hasAccess)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
