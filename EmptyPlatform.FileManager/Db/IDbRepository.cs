using System;

namespace EmptyPlatform.FileManager.Db
{
    public interface IDbRepository : IDisposable
    {
        void CreateFile(File file);

        File GetFile(string fileId);

        File GetFile(string contentType, long size, string hash);

        void RemoveFile(string fileId);
    }
}
