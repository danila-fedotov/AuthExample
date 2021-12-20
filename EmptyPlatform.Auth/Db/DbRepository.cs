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
    /// </summary>
    internal class DbRepository : IDbRepository, IDisposable
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbConnection _dbConnection;

        protected virtual string UserId => _httpContextAccessor.HttpContext.User.Identity.Name;

        public DbRepository(IHttpContextAccessor httpContextAccessor,
            IDbConnection dbConnection)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbConnection = dbConnection;
        }

        public virtual void Create(User user, string actionNote = null)
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

        public virtual List<User> GetUsers()
        {
            var users = _dbConnection.Query<User>("SELECT * FROM User WHERE _IsActive=1").ToList();

            return users;
        }

        public virtual User GetUser(string id)
        {
            var user = _dbConnection.Query<User>("SELECT * FROM User WHERE Id=@id AND _IsActive=1", new { id }).FirstOrDefault();

            return user;
        }

        public virtual User GetUserByEmail(string email)
        {
            var user = _dbConnection.Query<User>("SELECT * FROM User WHERE Email=@email AND _IsActive=1", new { email }).FirstOrDefault();

            return user;
        }

        public virtual string GetHashPassword(string userId)
        {
            var sqlQuery = "SELECT Password FROM UserPassword WHERE UserId=@userId AND _IsActive=1";
            var password = _dbConnection.Query<string>(sqlQuery, new { userId }).FirstOrDefault();

            return password;
        }

        public virtual void Update(User user, string actionNote)
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

        public void ChangePassword(string userId, string password, string actionNote)
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

        public virtual void RemoveUser(string id, string actionNote)
        {
            var sqlQuery = @"
BEGIN TRANSACTION;

INSERT INTO User (Id, _IsActive, _ActionNote, _CreatedDate, _CreatedByUserId)
VALUES (@id, 0, @actionNote, @CreatedDate, @CreatedByUserId);

UPDATE User SET _IsActive=0 WHERE Id=@id AND _IsActive=1;
UPDATE UserPassword SET _IsActive=0 WHERE UserId=@id AND _IsActive=1;

COMMIT;";

            _dbConnection.Execute(sqlQuery, new
            {
                id,
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
