using System;

namespace BlobExporter.Models
{
    public class BlobInfo
    {
        public DateTimeOffset? LastModified { get; set; }

        public string Content { get; set; }
    }
}