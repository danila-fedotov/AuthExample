using System;
using System.IO;
using System.Threading.Tasks;

namespace EmptyPlatform.FileManager
{
    internal class FileStorage : IFileStorage
    {
        private readonly FileManagerConfiguration _configuration;

        public FileStorage(FileManagerConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected virtual string GetFullPath(string fileName) => Path.Combine(_configuration.Directory, fileName);

        public virtual async Task CreateAsync(string fileName, Stream fileStream)
        {
            var path = GetFullPath(fileName);
            var isExists = File.Exists(path);

            if (isExists)
            {
                throw new ApplicationException("A file with the same name already exists");
            }

            try
            {
                using var outputStream = File.Create(path);

                await fileStream.CopyToAsync(outputStream);
                outputStream.Close();
            }
            catch
            {
                File.Delete(path);
                throw;
            }
        }

        public virtual Stream OpenRead(string fileName)
        {
            var path = GetFullPath(fileName);
            var isExists = File.Exists(path);

            if (!isExists)
            {
                throw new ApplicationException("A physical file is not found");
            }

            return File.OpenRead(path);
        }
    }
}
