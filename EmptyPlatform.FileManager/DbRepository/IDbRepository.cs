using EmptyPlatform.FileManager.Entities;
using System;
using System.Data;

namespace EmptyPlatform.FileManager
{
    internal interface IDbRepository : IDisposable
    {
        IDbTransaction BeginTransaction();

        void CreatePhysicalFile(PhysicalFile physicalFile);

        void CreateFile(File file);

        PhysicalFile FindPhysicalFile(string contentType, long size, string hash);

        File FindFile(string fileId);

        void RemoveFile(string fileId, string userId);
    }
}
