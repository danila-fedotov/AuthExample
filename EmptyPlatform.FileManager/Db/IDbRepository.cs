using System;

namespace EmptyPlatform.FileManager.Db
{
    internal interface IDbRepository : IDisposable
    {
        void CreateFile(File file);

        File GetFile(string fileId);

        File GetFile(string contentType, long size, string hash);

        void ForceRemoveFile(string fileId);
     
        void RemoveFile(string fileId);
    }
}
