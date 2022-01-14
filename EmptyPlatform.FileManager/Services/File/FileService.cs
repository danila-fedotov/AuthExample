using EmptyPlatform.FileManager.Db;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EmptyPlatform.FileManager.Services
{
    internal class FileService : IFileService
    {
        private readonly IDbRepository _dbRepository;
        private readonly IFileStorage _fileStorage;

        public FileService(IDbRepository dbRepository,
            IFileStorage fileStorage)
        {
            _dbRepository = dbRepository;
            _fileStorage = fileStorage;
        }

        protected virtual string GetFileHash(Stream fileStream)
        {
            using var md5 = MD5.Create();

            var hash = md5.ComputeHash(fileStream);

            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public virtual async Task<string> CreateAsync(string fileName, string contentType, long size, Stream fileStream)
        {
            var fileHash = GetFileHash(fileStream);
            var sourseFile = _dbRepository.GetFile(contentType, size, fileHash);
            var file = new Db.File
            {
                FileId = Guid.NewGuid().ToString(),
                FileName = fileName,
                SourceFileId = sourseFile?.FileId
            };

            if (sourseFile is null)
            {
                file.ContentType = contentType;
                file.Size = size;
                file.Hash = fileHash;
            }

            _dbRepository.CreateFile(file);

            if (sourseFile is null)
            {
                try
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    await _fileStorage.WriteAsync(file.FileId, fileStream);
                }
                catch
                {
                    // TODO: rollback transaction
                    throw;
                }
            }

            return file.FileId;
        }

        public virtual Db.File Get(string fileId)
        {
            var file = _dbRepository.GetFile(fileId) ?? throw new ArgumentNullException("FileId", "A file is not found");

            return file;
        }

        public virtual Stream Read(Db.File file)
        {
            var fileStream = _fileStorage.Read(file.SourceFileId ?? file.FileId);

            return fileStream;
        }

        public virtual void Remove(string fileId)
        {
            _dbRepository.RemoveFile(fileId);
        }
    }
}
