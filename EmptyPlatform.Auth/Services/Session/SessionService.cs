using EmptyPlatform.Auth.Db;
using EmptyPlatform.Auth.Db.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace EmptyPlatform.Auth.Services
{
    internal class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbRepository _dbRepository;

        protected virtual HttpContext HttpContext => _httpContextAccessor.HttpContext;

        protected virtual string UserId => HttpContext.User.Identity.Name ?? "root";

        protected virtual int SessionId => Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication)?.Value);

        protected virtual string Address => HttpContext.Connection.RemoteIpAddress.ToString();

        protected virtual string Device => HttpContext.Request.Headers["User-Agent"].ToString();

        public SessionService(IHttpContextAccessor httpContextAccessor,
            IDbRepository dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbRepository = dbRepository;
        }

        public virtual int Create(string userId)
        {
            var sessionId = _dbRepository.CreateSession(userId, Device, Address);

            return sessionId;
        }

        public virtual Session Get(int sessionId)
        {
            var session = _dbRepository.GetSessionById(sessionId);

            return session;
        }

        public virtual void Close(int sessionId)
        {
            _dbRepository.CloseSession(sessionId);
        }

        public virtual void Close(string userId)
        {
            _dbRepository.CloseSessions(userId);
        }

        public bool Validate()
        {
            var session = Get(SessionId);

            if (session is null)
            {
                return false;
            }

            if (session.UserId != UserId)
            {
                // TODO: log
                Close(UserId);
                Close(session.UserId);

                return false;
            }

            if (session.Device != Device)
            {
                // TODO: log
                Close(session.SessionId);

                return false;
            }

            return true;
        }
    }
}
