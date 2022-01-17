using System;

namespace EmptyPlatform.FileManager.Entities
{
    public class File
    {
        public string FileId { get; set; }

        public string FileName { get; set; }

        public string PhysicalFileId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedByUserId { get; set; }

        public string ContentType { get; set; }
    }
}
