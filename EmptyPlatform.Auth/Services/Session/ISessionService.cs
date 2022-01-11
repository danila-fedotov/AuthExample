namespace EmptyPlatform.Auth.Services
{
    public interface ISessionService
    {
        int Create(string userId, string device, string address);

        void Close(string userId);

        void Close(string userId, int sessionId);

        bool Has(string userId, int sessionId);
    }
}
