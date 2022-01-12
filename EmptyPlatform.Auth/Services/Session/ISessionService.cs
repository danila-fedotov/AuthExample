using EmptyPlatform.Auth.Db.Entities;

namespace EmptyPlatform.Auth.Services
{
    public interface ISessionService
    {
        int Create(string userId, string device, string address);

        Session Get(int sessionId);

        void Close(int sessionId);

        void Close(string userId);
    }
}
