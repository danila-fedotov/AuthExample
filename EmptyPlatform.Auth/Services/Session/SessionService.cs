using EmptyPlatform.Auth.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EmptyPlatform.Auth.Services
{
    internal class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private IDbRepository DbRepository => _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IDbRepository>();

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual int Create(string userId, string device, string address)
        {
            var sessionId = DbRepository.CreateSession(userId, device, address);

            return sessionId;
        }

        public virtual void Close(string userId)
        {
            DbRepository.CloseSessions(userId);
        }

        public virtual void Close(string userId, int sessionId)
        {
            DbRepository.CloseSession(sessionId);
        }

        public virtual bool Has(string userId, int sessionId)
        {
            var actualUserId = DbRepository.GetUserIdBySessionId(sessionId);

            if (string.IsNullOrEmpty(actualUserId))
            {
                return false;
            }

            var isSameUser = actualUserId == userId;

            return isSameUser;
        }
    }
}
