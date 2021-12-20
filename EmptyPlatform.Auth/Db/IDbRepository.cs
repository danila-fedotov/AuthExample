using System.Collections.Generic;

namespace EmptyPlatform.Auth.Db
{
    internal interface IDbRepository
    {
        void Create(User user, string actionNote);

        List<User> GetUsers();

        User GetUser(string id);

        User GetUserByEmail(string email);

        string GetHashPassword(string userId);

        void Update(User user, string actionNote);

        void ChangePassword(string userId, string password, string actionNote);

        void RemoveUser(string id, string actionNote);
    }
}
