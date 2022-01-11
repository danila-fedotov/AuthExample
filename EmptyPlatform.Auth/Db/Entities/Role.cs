using Newtonsoft.Json;
using System.Collections.Generic;

namespace EmptyPlatform.Auth.Db
{
    public class Role
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PermissionsAsJson { get; set; }

        public Dictionary<string, string[]> Permissions => JsonConvert.DeserializeObject<Dictionary<string, string[]>>(PermissionsAsJson);
    }
}
