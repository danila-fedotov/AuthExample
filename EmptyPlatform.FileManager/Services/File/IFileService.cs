using EmptyPlatform.FileManager.Entities;
using System.Threading.Tasks;
using Stream = System.IO.Stream;

namespace EmptyPlatform.FileManager.Services
{
    public interface IFileService
    {
        Task<string> CreateAsync(string fileName, string contentType, long size, Stream fileStream);

        File GetOrDefault(string fileId);

        Stream OpenRead(File file);

        void Remove(File file);
    }
}
