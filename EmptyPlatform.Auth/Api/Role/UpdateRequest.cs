using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api.Role
{
    public class UpdateRequest
    {
        public string RoleId { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        public Dictionary<string, string[]> Permissions { get; set; }

        public string ActionNote { get; set; }

        public Db.Role Map() => new()
        {
            Id = RoleId,
            Name = Name,
            Permissions = Permissions
        };
    }
}
