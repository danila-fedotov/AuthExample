using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace EmptyPlatform.Auth
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public SwaggerDocumentFilter(IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(userId))
                {
                    RemoveAllEndPoints(swaggerDoc, context);
                    return;
                }

                var user = _userService.Get(userId);

                foreach (var apiDescription in context.ApiDescriptions)
                {
                    var controllerName = apiDescription.ActionDescriptor.RouteValues["controller"].ToLower();
                    var actionName = apiDescription.ActionDescriptor.RouteValues["action"].ToLower();
                    var hasAccess = _userService.ValidateAccess(user.Permissions, controllerName, actionName);

                    if (!hasAccess)
                    {
                        swaggerDoc.Paths.Remove($"/{apiDescription.RelativePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: log
                RemoveAllEndPoints(swaggerDoc, context);
            }
        }

        protected virtual void RemoveAllEndPoints(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var apiDescription in context.ApiDescriptions)
            {
                var controllerName = apiDescription.ActionDescriptor.RouteValues["controller"].ToLower();
                var isAccountController = controllerName == "account";

                if (!isAccountController)
                {
                    swaggerDoc.Paths.Remove($"/{apiDescription.RelativePath}");
                }
            }
        }
    }
}
