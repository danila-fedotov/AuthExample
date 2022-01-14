using System;

namespace EmptyPlatform.FileManager.Db
{
    public class File
    {
        public string FileId { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string Hash { get; set; }

        public string SourceFileId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedByUserId { get; set; }
    }
}
