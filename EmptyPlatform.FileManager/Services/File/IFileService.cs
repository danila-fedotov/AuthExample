using System.IO;
using System.Threading.Tasks;

namespace EmptyPlatform.FileManager.Services
{
    public interface IFileService
    {
        Task<string> CreateAsync(string fileName, string contentType, long size, Stream fileStream);

        Db.File Get(string fileId);

        Stream Read(Db.File file);

        void Remove(string fileId);
    }
}
