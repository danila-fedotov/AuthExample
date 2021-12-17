using AuthExample.Auth.Db.Entities;
using System.Collections.Generic;

namespace AuthExample.Auth.Db
{
    public interface IDbRepository
    {
        void Create(User user, string actionNote);

        List<User> GetUsers();

        User GetUser(string id);

        User GetUserByEmail(string email);

        string GetPassword(string userId);

        void Update(User user, string actionNote);

        void DeleteUser(string id, string actionNote);
    }
}
