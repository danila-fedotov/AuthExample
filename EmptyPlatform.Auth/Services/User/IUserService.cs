using EmptyPlatform.Auth.Db;
using System.Collections.Generic;

namespace EmptyPlatform.Auth.Services
{
    public interface IUserService
    {
        void Create(User user, string actionNote);

        User Get(string userId);

        User GetByEmail(string email);

        List<User> Get();

        void Update(User user, string actionNote);

        void Remove(string userId, string actionNote);

        bool MatchPassword(string userId, string password);
    }
}
