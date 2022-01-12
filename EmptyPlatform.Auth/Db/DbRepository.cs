using Dapper;
using EmptyPlatform.Auth.Db.Entities;
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
            var sql = @"
BEGIN TRANSACTION;

UPDATE Session SET ClosedDate=@CreatedDate WHERE UserId=@userId AND Device=@device AND Address=@address AND ClosedDate is null;

INSERT INTO Session (UserId, Device, Address, CreatedDate)
VALUES
(@userId, @device, @address, @CreatedDate);

SELECT last_insert_rowid();

COMMIT;";
            var sessionId = _dbConnection.Query<int>(sql, new
            {
                userId,
                device,
                address,
                CreatedDate = DateTime.UtcNow,
            }).FirstOrDefault();

            return sessionId;
        }

        public virtual Session GetSessionById(int sessionId)
        {
            var sql = "SELECT * FROM Session WHERE SessionId=@sessionId AND ClosedDate is null";
            var session = _dbConnection.Query<Session>(sql, new { sessionId }).FirstOrDefault();

            return session;
        }

        public virtual void CloseSession(int sesionId)
        {
            var sql = "UPDATE Session SET ClosedDate=@ClosedDate WHERE SessionId=@sesionId and ClosedDate is null";

            _dbConnection.Execute(sql, new
            {
                sesionId,
                ClosedDate = DateTime.UtcNow
            });
        }

        public virtual void CloseSessions(string userId)
        {
            var sql = "UPDATE Session SET ClosedDate=@ClosedDate WHERE UserId=@userId and ClosedDate is null";

            _dbConnection.Execute(sql, new
            {
                userId,
                ClosedDate = DateTime.UtcNow
            });
        }

        public virtual void CloseSessions()
        {
            var sql = "UPDATE Session SET ClosedDate=@ClosedDate WHERE ClosedDate is null";

            _dbConnection.Execute(sql, new
            {
                ClosedDate = DateTime.UtcNow
            });
        }

        public virtual void CreateUser(User user, string actionNote)
        {
            var sql = @"
BEGIN TRANSACTION;

INSERT INTO User (UserId, Email, FirstName, SecondName, Birthday, IsActive, ActionNote, CreatedDate, CreatedByUserId)
VALUES
(@UserId, @Email, @FirstName, @SecondName, @Birthday, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sql, new
            {
                user.UserId,
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
            var sql = "SELECT * FROM User WHERE UserId=@userId AND IsActive=1";
            var user = _dbConnection.Query<User>(sql, new { userId }).FirstOrDefault();

            return user;
        }

        public virtual User GetUserByEmail(string email)
        {
            var sql = "SELECT * FROM User WHERE Email=@email AND IsActive=1";
            var user = _dbConnection.Query<User>(sql, new { email }).FirstOrDefault();

            return user;
        }

        public virtual List<User> GetUsersByRoleId(string roleId)
        {
            var sql = @"
SELECT u.* 
FROM UserRole ur
INNER JOIN User u ON u.Id=ur.UserId and u._IsActive=1 
WHERE ur.RoleId=@roleId and ur._ClosedDate is null";
            var users = _dbConnection.Query<User>(sql, new { roleId }).ToList();

            return users;
        }

        public virtual List<User> GetUsers()
        {
            var sql = "SELECT * FROM User WHERE IsActive=1";
            var users = _dbConnection.Query<User>(sql).ToList();

            return users;
        }

        public virtual void UpdateUser(User user, string actionNote)
        {
            var sql = @"
BEGIN TRANSACTION;

UPDATE User SET IsActive=0 WHERE UserId=@UserId AND IsActive=1;

INSERT INTO User (UserId, Email, FirstName, SecondName, Birthday, IsActive, ActionNote, CreatedDate, CreatedByUserId)
VALUES
(@UserId, @Email, @FirstName, @SecondName, @Birthday, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sql, new
            {
                user.UserId,
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
            var sql = @"
BEGIN TRANSACTION;

UPDATE User SET IsActive=0 WHERE UserId=@userId AND IsActive=1;

INSERT INTO User (UserId, IsActive, ActionNote, CreatedDate, CreatedByUserId)
VALUES (@userId, 0, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sql, new
            {
                userId,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual void CreatePassword(string userId, string password, string actionNote)
        {
            var sql = @"
BEGIN TRANSACTION;

INSERT INTO UserPassword (UserId, Password, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES (@userId, @password, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sql, new
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
            var sql = "SELECT Password FROM UserPassword WHERE UserId=@userId AND IsActive=1";
            var password = _dbConnection.Query<string>(sql, new { userId }).FirstOrDefault();

            return password;
        }

        public virtual void UpdatePassword(string userId, string password, string actionNote)
        {
            var sql = @"
BEGIN TRANSACTION;

UPDATE UserPassword SET _IsActive=0 WHERE UserId=@userId AND _IsActive=1;

INSERT INTO UserPassword (UserId, Password, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES (@userId, @password, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sql, new
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
            var sql = @"
BEGIN TRANSACTION;

UPDATE UserPassword SET _IsActive=0 WHERE UserId=@userId AND _IsActive=1;

INSERT INTO UserPassword (UserId, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES (@userId, 0, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sql, new
            {
                userId,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual void CreateRole(Role role, string actionNote)
        {
            var sql = @"
INSERT INTO Role (Id, Name, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES
(@Id, @Name, 1, @actionNote, @CreatedDate, @CreatedByUserId);";

            _dbConnection.Execute(sql, new
            {
                role.RoleId,
                role.Name,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual Role GetRoleById(string roleId)
        {
            var sql = @"
SELECT r.*, rp.PermissionsAsJson
FROM Role r
LEFT JOIN RolePermission rp ON rp.RoleId=r.RoleId and rp.IsActive=1
WHERE r.RoleId=@roleId AND r.IsActive=1";
            var role = _dbConnection.Query<Role>(sql, new { roleId }).FirstOrDefault();

            return role;
        }

        public virtual List<Role> GetRolesByUserId(string userId)
        {
            var sql = @"
SELECT r.*, rp.PermissionsAsJson
FROM UserRole ur 
INNER JOIN Role r on r.RoleId=ur.RoleId and r.IsActive=1
LEFT JOIN RolePermission rp on rp.RoleId=r.RoleId and rp.IsActive=1
WHERE ur.UserId=@userId and ur.ClosedDate is null";
            var roles = _dbConnection.Query<Role>(sql, new { userId }).ToList();

            return roles;
        }

        public virtual List<Role> GetRoles()
        {
            var sql = "SELECT * FROM Role WHERE IsActive=1";
            var roles = _dbConnection.Query<Role>(sql).ToList();

            return roles;
        }

        public virtual void UpdateRole(Role role, string actionNote)
        {
            var sql = @"
BEGIN TRANSACTION;

UPDATE Role SET IsActive=0 WHERE RoleId=@RoleId AND IsActive=1;

INSERT INTO Role (RoleId, Name, IsActive, ActionNote, CreatedDate, CreatedByUserId)
VALUES
(@RoleId, @Name, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sql, new
            {
                role.RoleId,
                role.Name,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual void UpdateRolePermissions(string roleId, string permissionsAsJson, string actionNote)
        {
            var sql = @"
BEGIN TRANSACTION;

UPDATE RolePermission SET IsActive=0 WHERE RoleId=@roleId AND IsActive=1;

INSERT INTO RolePermission (RoleId, PermissionsAsJson, IsActive, ActionNote, CreatedDate, CreatedByUserId)
VALUES
(@roleId, @permissionsAsJson, 1, @actionNote, @CreatedDate, @CreatedByUserId);

COMMIT;";

            _dbConnection.Execute(sql, new
            {
                roleId,
                permissionsAsJson,
                actionNote,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}
