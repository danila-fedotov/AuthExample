using EmptyPlatform.FileManager.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Stream = System.IO.Stream;

namespace EmptyPlatform.FileManager.Services
{
    internal class FileService : IFileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbRepository _dbRepository;
        private readonly IFileStorage _fileStorage;
        protected virtual string UserId => _httpContextAccessor.HttpContext.User.Identity.Name ?? "root";

        public FileService(IHttpContextAccessor httpContextAccessor,
            IDbRepository dbRepository,
            IFileStorage fileStorage)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbRepository = dbRepository;
            _fileStorage = fileStorage;
        }

        protected virtual async Task<PhysicalFile> SavePhysicalFileAsync(string contentType, long size, Stream fileStream)
        {
            var hash = GetFileHash(fileStream);
            var actualPhysicalFile = _dbRepository.FindPhysicalFile(contentType, size, hash);

            if (actualPhysicalFile is not null)
            {
                return actualPhysicalFile;
            }

            var physicalFile = new PhysicalFile()
            {
                PhysicalFileId = Guid.NewGuid().ToString(),
                ContentType = contentType,
                Size = size,
                Hash = hash,
                CreatedDate = DateTime.Now,
                CreatedByUserId = UserId
            };

            using var transaction = _dbRepository.BeginTransaction();

            _dbRepository.CreatePhysicalFile(physicalFile);
            await _fileStorage.CreateAsync(physicalFile.PhysicalFileId, fileStream);

            transaction.Commit();

            return physicalFile;
        }

        protected virtual string GetFileHash(Stream fileStream)
        {
            using var md5 = MD5.Create();

            var hash = md5.ComputeHash(fileStream);

            fileStream.Seek(0, System.IO.SeekOrigin.Begin);

            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public virtual async Task<string> CreateAsync(string fileName, string contentType, long size, Stream fileStream)
        {
            var physicalFile = await SavePhysicalFileAsync(contentType, size, fileStream);
            var file = new File
            {
                FileId = Guid.NewGuid().ToString(),
                FileName = fileName,
                PhysicalFileId = physicalFile.PhysicalFileId,
                CreatedDate = DateTime.Now,
                CreatedByUserId = UserId
            };

            _dbRepository.CreateFile(file);

            return file.FileId;
        }

        public virtual File GetOrDefault(string fileId)
        {
            var file = _dbRepository.FindFile(fileId);

            return file;
        }

        public virtual Stream OpenRead(File file)
        {
            var fileStream = _fileStorage.OpenRead(file.PhysicalFileId);

            return fileStream;
        }

        public virtual void Remove(File file)
        {
            _dbRepository.RemoveFile(file.FileId, UserId);
        }
    }
}
