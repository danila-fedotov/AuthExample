using System.IO;
using System.Threading.Tasks;

namespace EmptyPlatform.FileManager
{
    internal interface IFileStorage
    {
        Task CreateAsync(string fileName, Stream fileStream);

        Stream OpenRead(string fileName);
    }
}
