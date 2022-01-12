using System.Collections.Generic;

namespace EmptyPlatform.Auth.Api.Role
{
    public class PermissionsResponse
    {
        public PermissionResponse Root { get; set; }

        public List<PermissionResponse> Nodes { get; set; }
    }
}
