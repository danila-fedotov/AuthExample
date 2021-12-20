using EmptyPlatform.Auth.Db;
using System.Collections.Generic;

namespace EmptyPlatform.Auth.Services
{
    public interface IUserService
    {
        void Create(User user, string actionNote);

        List<User> Get();

        User Get(string id);

        User GetByEmail(string email);

        void Update(User user, string actionNote = null);

        void Remove(User user, string actionNote);

        bool MatchPassword(User user, string password);
    }
}
