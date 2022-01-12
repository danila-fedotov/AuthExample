using EmptyPlatform.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace EmptyPlatform.Auth.Api.Role
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Description = "Role management")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("Permissions")]
        [Display(Description = "Viewing a list of permissions")]
        public IActionResult GetPermissions()
        {
            var apiControllers = Assembly
               .GetEntryAssembly()
               .GetReferencedAssemblies()
               .Select(Assembly.Load)
               .SelectMany(x => x.DefinedTypes)
               .Where(x => x.GetCustomAttributes<ApiControllerAttribute>().Any())
               .Where(x => x.Name != "AccountController");
            var response = apiControllers.Select(x => new PermissionsResponse
            {
                Root = new PermissionResponse
                {
                    Name = x.Name.Replace("Controller", string.Empty).ToLower(),
                    Description = x.GetCustomAttribute<DisplayAttribute>()?.Description
                },
                Nodes = x.DeclaredMethods.Where(y => y.IsPublic)
                    .Where(y => !y.GetCustomAttributes<AllowAnonymousAttribute>().Any())
                    .Select(y => new PermissionResponse
                    {
                        Name = y.Name.ToLower(),
                        Description = y.GetCustomAttribute<DisplayAttribute>()?.Description
                    }).ToList()
            });

            return Ok(response);
        }

        [HttpGet("Roles")]
        [Display(Description = "Viewing a list of roles")]
        public IActionResult GetRoles()
        {
            var roles = _roleService.Get();
            var response = roles.Select(RoleResponse.Map);

            return Ok(response);
        }

        [HttpPut]
        [Display(Description = "Role change")]
        public IActionResult Update(UpdateRequest request)
        {
            var role = request.Map();

            _roleService.Update(role, request.ActionNote);

            return Ok();
        }
    }
}
