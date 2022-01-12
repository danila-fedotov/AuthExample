using Newtonsoft.Json;
using System.Collections.Generic;

namespace EmptyPlatform.Auth.Db
{
    public class Role
    {
        public string RoleId { get; set; }

        public string Name { get; set; }

        public string PermissionsAsJson { get; set; }

        public Dictionary<string, string[]> Permissions
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, string[]>>(PermissionsAsJson ?? string.Empty);
            set => PermissionsAsJson = JsonConvert.SerializeObject(value ?? new());
        }
    }
}
