using EmptyPlatform.FileManager.Db;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using File = System.IO.File;

namespace EmptyPlatform.FileManager.Services
{
    internal class FileService : IFileService
    {
        private readonly IDbRepository _dbRepository;
        private readonly FileManagerConfiguration _configuration;

        public FileService(IDbRepository dbRepository,
            FileManagerConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }

        protected virtual string GetPathToFile(string fileId)
        {
            var path = $"{_configuration.Directory}{fileId}";

            return path;
        }

        protected virtual async Task WriteAsync(string fileId, Stream sourceStream)
        {
            var path = GetPathToFile(fileId);

            using var destinationStream = File.OpenWrite(path);

            await sourceStream.CopyToAsync(destinationStream);
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
                    await WriteAsync(file.FileId, fileStream);
                }
                catch
                {
                    _dbRepository.ForceRemoveFile(file.FileId);
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

        public virtual Stream Read(string fileId)
        {
            var path = GetPathToFile(fileId);
            var isExists = File.Exists(path);

            if (!isExists)
            {
                throw new Exception("A physical file is not found");
            }

            return File.OpenRead(path);
        }

        public virtual void Remove(string fileId)
        {
            _dbRepository.RemoveFile(fileId);
        }
    }
}
