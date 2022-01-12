using System.Collections.Generic;

namespace EmptyPlatform.Auth.Api.Role
{
    public class RoleResponse
    {
        public string RoleId { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string[]> Permissions { get; set; }

        public static RoleResponse Map(Db.Role role) => new()
        {
            RoleId = role.RoleId,
            Name = role.Name,
            Permissions = role.Permissions
        };
    }
}
