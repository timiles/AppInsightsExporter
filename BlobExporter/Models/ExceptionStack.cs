using System.Collections.Generic;

namespace BlobExporter.Models
{
    public class ExceptionStack
    {
        public string Message { get; set; }

        public IEnumerable<StackTraceLevel> StackTrace { get; set; }
    }
}
