using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EmptyPlatform.Auth.Db
{
    /// <summary>
    /// TODO: add migrations
    /// TODO: utc date
    /// TODO: connection factory
    /// </summary>
    internal class DbRepository : IDbRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbConnection _dbConnection;

        protected virtual string UserId => _httpContextAccessor.HttpContext.User.Identity.Name ?? "root";

        public DbRepository(IHttpContextAccessor httpContextAccessor,
            IDbConnection dbConnection)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbConnection = dbConnection;
        }

        public virtual int CreateSession(string userId, string device, string address)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

UPDATE Session SET ClosedDate=@CreatedDate WHERE UserId=@userId AND Device=@device AND Address=@address AND ClosedDate is null;

INSERT INTO Session (UserId, Device, Address, CreatedDate)
VALUES
(@userId, @device, @address, @CreatedDate);

SELECT last_insert_rowid();

COMMIT;";
            var sessionId = _dbConnection.Query<int>(sqlQuery, new
            {
                userId,
                device,
                address,
                CreatedDate = DateTime.UtcNow,
            }).FirstOrDefault();

            return sessionId;
        }

        public virtual string GetUserIdBySessionId(int sessionId)
        {
            var sqlQuery = "SELECT UserId FROM Session WHERE Id=@sessionId AND ClosedDate is null";
            var userId = _dbConnection.Query<string>(sqlQuery, new { sessionId }).FirstOrDefault();

            return userId;
        }

        public virtual void CloseSession(int sesionId)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

UPDATE Session SET ClosedDate=@ClosedDate WHERE Id=@sesionId and ClosedDate is null;

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                sesionId,
                ClosedDate = DateTime.UtcNow
            });
        }

        public virtual void CloseSessions(string userId)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

UPDATE Session SET ClosedDate=@ClosedDate WHERE UserId=@userId and ClosedDate is null;

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                userId,
                ClosedDate = DateTime.UtcNow
            });
        }

        public virtual void CloseSessions()
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

UPDATE Session SET ClosedDate=@ClosedDate WHERE ClosedDate is null;

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                ClosedDate = DateTime.UtcNow
            });
        }

        public virtual void CreateUser(User user, string actionNote)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

INSERT INTO User (Id, Email, FirstName, SecondName, Birthday, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES
(@Id, @Email, @FirstName, @SecondName, @Birthday, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.SecondName,
                user.Birthday,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual User GetUserById(string userId)
        {
            var user = _dbConnection.Query<User>("SELECT * FROM User WHERE Id=@userId AND _IsActive=1", new { userId }).FirstOrDefault();

            return user;
        }

        public virtual User GetUserByEmail(string email)
        {
            var user = _dbConnection.Query<User>("SELECT * FROM User WHERE Email=@email AND _IsActive=1", new { email }).FirstOrDefault();

            return user;
        }

        public virtual List<User> GetUsers()
        {
            var users = _dbConnection.Query<User>("SELECT * FROM User WHERE _IsActive=1").ToList();

            return users;
        }

        public virtual void UpdateUser(User user, string actionNote)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

UPDATE User SET _IsActive=0 WHERE Id=@Id AND _IsActive=1;

INSERT INTO User (Id, Email, FirstName, SecondName, Birthday, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES
(@Id, @Email, @FirstName, @SecondName, @Birthday, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.SecondName,
                user.Birthday,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual void RemoveUser(string userId, string actionNote)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

UPDATE User SET _IsActive=0 WHERE Id=@userId AND _IsActive=1;

INSERT INTO User (Id, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES (@userId, 0, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                userId,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual void CreatePassword(string userId, string password, string actionNote)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

INSERT INTO UserPassword (UserId, Password, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES (@userId, @password, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                userId,
                password,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual string GetPassword(string userId)
        {
            var sqlQuery = "SELECT Password FROM UserPassword WHERE UserId=@userId AND _IsActive=1";
            var password = _dbConnection.Query<string>(sqlQuery, new { userId }).FirstOrDefault();

            return password;
        }

        public virtual void UpdatePassword(string userId, string password, string actionNote)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

UPDATE UserPassword SET _IsActive=0 WHERE UserId=@userId AND _IsActive=1;

INSERT INTO UserPassword (UserId, Password, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES (@userId, @password, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                userId,
                password,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual void RemovePassword(string userId, string actionNote)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

UPDATE UserPassword SET _IsActive=0 WHERE UserId=@userId AND _IsActive=1;

INSERT INTO UserPassword (UserId, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES (@userId, 0, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                userId,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual List<Role> GetRolesByUserId(string userId)
        {
            var sqlQuery = @"
SELECT r.*, rp.PermissionsAsJson
FROM UserRole ur 
INNER JOIN Role r on r.Id=ur.RoleId and r._IsActive=1
LEFT JOIN RolePermission rp on rp.RoleId=r.Id and rp._IsActive=1
WHERE ur.UserId=@userId and ur._ClosedDate is null";
            var roles = _dbConnection.Query<Role>(sqlQuery, new { userId }).ToList();

            return roles;
        }

        public virtual void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}
