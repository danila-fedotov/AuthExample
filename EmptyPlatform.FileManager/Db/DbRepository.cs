using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.Linq;

namespace EmptyPlatform.FileManager.Db
{
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

        public virtual void Dispose()
        {
            _dbConnection?.Dispose();
        }

        public virtual void CreateFile(File file)
        {
            var sql = @"
INSERT INTO File (FileId, FileName, ContentType, Size, SourceFileId, CreatedDate, CreatedByUserId, IsActive)
VALUES
(@FileId, @FileName, @ContentType, @Size, @SourceFileId, @CreatedDate, @CreatedByUserId, 1)";

            _dbConnection.Execute(sql, new
            {
                file.FileId,
                file.FileName,
                file.ContentType,
                file.Size,
                file.SourceFileId,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = UserId
            });
        }

        public virtual File GetFile(string fileId)
        {
            var sql = "SELECT * FROM File WHERE FileId=@fileId AND IsActive=1";
            var file = _dbConnection.Query<File>(sql, new { fileId }).FirstOrDefault();

            return file;
        }

        public virtual File GetFile(string contentType, long size, string hash)
        {
            var sql = @"SELECT * FROM File WHERE ContentType=@contentType AND Size=@size AND Hash=@hash";
            var file = _dbConnection.Query<File>(sql, new
            {
                contentType,
                size,
                hash
            }).FirstOrDefault();

            return file;
        }

        public void ForceRemoveFile(string fileId)
        {
            var sql = "DELETE FROM File WHERE FileId=@fileId";

            _dbConnection.Execute(sql, new { fileId });
        }

        public void RemoveFile(string fileId)
        {
            var sql = "UPDATE File SET IsActive=0, ClosedDate=@ClosedDate, ClosedByUserId=@ClosedByUserId WHERE FileId=@fileId AND IsActive=1";

            _dbConnection.Execute(sql, new
            {
                fileId,
                ClosedDate = DateTime.UtcNow,
                ClosedByUserId = UserId
            });
        }
    }
}
