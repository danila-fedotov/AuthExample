using Dapper;
using EmptyPlatform.FileManager.Entities;
using System;
using System.Data;

namespace EmptyPlatform.FileManager
{
    internal class DbRepository : IDbRepository
    {
        private readonly IDbConnection _dbConnection;

        public DbRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public virtual void Dispose()
        {
            _dbConnection?.Dispose();
        }

        public virtual IDbTransaction BeginTransaction()
        {
            _dbConnection.Open();

            var transaction = _dbConnection.BeginTransaction();

            return transaction;
        }

        public void CreatePhysicalFile(PhysicalFile physicalFile)
        {
            var sql = @"
INSERT INTO PhysicalFile (PhysicalFileId, ContentType, Size, Hash, CreatedDate, CreatedByUserId, IsActive)
VALUES
(@PhysicalFileId, @ContentType, @Size, @Hash, @CreatedDate, @CreatedByUserId, 1)";

            _dbConnection.Execute(sql, new
            {
                physicalFile.PhysicalFileId,
                physicalFile.ContentType,
                physicalFile.Size,
                physicalFile.Hash,
                physicalFile.CreatedDate,
                physicalFile.CreatedByUserId
            });
        }

        public virtual void CreateFile(File file)
        {
            var sql = @"
INSERT INTO File (FileId, FileName, PhysicalFileId, CreatedDate, CreatedByUserId, IsActive)
VALUES
(@FileId, @FileName, @PhysicalFileId, @CreatedDate, @CreatedByUserId, 1)";

            _dbConnection.Execute(sql, new
            {
                file.FileId,
                file.FileName,
                file.PhysicalFileId,
                file.CreatedDate,
                file.CreatedByUserId
            });
        }

        public virtual PhysicalFile FindPhysicalFile(string contentType, long size, string hash)
        {
            var sql = @"
SELECT * 
FROM PhysicalFile 
WHERE ContentType=@contentType AND Size=@size AND Hash=@hash AND IsActive=1";
            var physicalFile = _dbConnection.QueryFirstOrDefault<PhysicalFile>(sql, new
            {
                contentType,
                size,
                hash
            });

            return physicalFile;
        }

        public virtual File FindFile(string fileId)
        {
            var sql = @"
SELECT f.*, pf.ContentType, pf.Size 
FROM File f
LEFT JOIN PhysicalFile pf ON pf.PhysicalFileId=f.PhysicalFileId AND pf.IsActive=1
WHERE f.FileId=@fileId AND f.IsActive=1";
            var file = _dbConnection.QueryFirstOrDefault<File>(sql, new { fileId });

            return file;
        }

        public virtual void RemoveFile(string fileId, string userId)
        {
            using var transaction = BeginTransaction();

            var sql = @"
UPDATE File SET IsActive=0 WHERE FileId=@fileId AND IsActive=1;
INSERT INTO File (FileId, CreatedDate, CreatedByUserId, IsActive)
VALUES (@fileId, @CreatedDate, @userId, 0);";

            _dbConnection.Execute(sql, new
            {
                fileId,
                CreatedDate = DateTime.Now,
                userId
            }, transaction);

            transaction.Commit();
        }
    }
}
