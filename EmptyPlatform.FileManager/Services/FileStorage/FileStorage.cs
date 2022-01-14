using System;
using System.IO;
using System.Threading.Tasks;

namespace EmptyPlatform.FileManager.Services
{
    internal class FileStorage : IFileStorage
    {
        private readonly FileManagerConfiguration _configuration;

        public FileStorage(FileManagerConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected virtual string GetFullPathToFile(string fileName) => $"{_configuration.Directory}{fileName}";

        public virtual async Task WriteAsync(string fileName, Stream fileStream)
        {
            var path = GetFullPathToFile(fileName);
            var isExists = File.Exists(path);

            if (isExists)
            {
                throw new Exception("A file with the same name already exists");
            }

            using var streamToWrite = File.OpenWrite(path);

            await fileStream.CopyToAsync(streamToWrite);
        }

        public virtual Stream Read(string fileName)
        {
            var path = GetFullPathToFile(fileName);
            var isExists = File.Exists(path);

            if (!isExists)
            {
                throw new Exception("A physical file is not found");
            }

            return File.OpenRead(path);
        }
    }
}
