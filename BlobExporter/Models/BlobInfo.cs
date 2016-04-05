using System;

namespace BlobExporter.Models
{
    public class BlobInfo
    {
        public string Path { get; set; }

        public DateTimeOffset? LastModified { get; set; }

        public string Content { get; set; }
    }
}