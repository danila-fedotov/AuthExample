using System.Collections.Generic;

namespace EmptyPlatform.Auth.Api.Role
{
    public class RoleResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string[]> Permissions { get; set; }

        public static RoleResponse Map(Db.Role role) => new()
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = role.Permissions
        };
    }
}
