using EmptyPlatform.Auth.Db;
using System.Collections.Generic;

namespace EmptyPlatform.Auth.Services
{
    public interface IRoleService
    {
        Role Get(string roleId);

        List<Role> Get();

        void Update(Role role, string actionNote);
    }
}
