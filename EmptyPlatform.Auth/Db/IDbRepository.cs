using System;
using System.Collections.Generic;

namespace EmptyPlatform.Auth.Db
{
    internal interface IDbRepository : IDisposable
    {
        int CreateSession(string userId, string device, string address);

        string GetUserIdBySessionId(int sessionId);

        void CloseSession(int sessionId);

        void CloseSessions(string userId);

        void CloseSessions();

        void CreateUser(User user, string actionNote);

        User GetUserById(string userId);

        User GetUserByEmail(string email);

        List<User> GetUsers();

        void UpdateUser(User user, string actionNote);

        void RemoveUser(string userId, string actionNote);

        void CreatePassword(string userId, string password, string actionNote);

        string GetPassword(string userId);

        void UpdatePassword(string userId, string password, string actionNote);

        void RemovePassword(string userId, string actionNote);

        List<Role> GetRolesByUserId(string userId);
    }
}
