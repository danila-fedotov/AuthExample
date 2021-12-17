using AuthExample.Auth.Db.Entities;
using System.Collections.Generic;

namespace AuthExample.Auth
{
    public interface IUserService
    {
        List<User> Get();

        User Get(string id);

        User GetByEmail(string email);

        void Update(User user, string actionNote = null);

        void Delete(string id, string actionNote = null);

        bool MatchPassword(User user, string password);
    }
}
