using System;

namespace EmptyPlatform.FileManager.Entities
{
    public class PhysicalFile
    {
        public string PhysicalFileId { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string Hash { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedByUserId { get; set; }
    }
}
