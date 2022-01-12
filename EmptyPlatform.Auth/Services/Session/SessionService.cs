using EmptyPlatform.Auth.Db;
using EmptyPlatform.Auth.Db.Entities;
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

        public virtual Session Get(int sessionId)
        {
            var session = DbRepository.GetSessionById(sessionId);

            return session;
        }

        public virtual void Close(int sessionId)
        {
            DbRepository.CloseSession(sessionId);
        }

        public virtual void Close(string userId)
        {
            DbRepository.CloseSessions(userId);
        }
    }
}
