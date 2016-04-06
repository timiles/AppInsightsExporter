using System.Collections.Generic;

namespace BlobExporter.Models
{
    public class ExceptionBatch
    {
        public IEnumerable<ExceptionInfo> ExceptionInfos { get; set; }

        public BlobInfo OriginalBlobInfo { get; set; }
    }
}
