using System.IO;
using System.Threading.Tasks;

namespace EmptyPlatform.FileManager.Services
{
    public interface IFileStorage
    {
        Task WriteAsync(string fileName, Stream fileStream);

        Stream Read(string fileName);
    }
}
