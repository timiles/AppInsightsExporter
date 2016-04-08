using System;
using System.Collections.Generic;

namespace BlobExporter.Models
{
    public class ExceptionInfo
    {
        public string Operation { get; internal set; }

        public DateTimeOffset EventTime { get; set; }
        
        public IEnumerable<ExceptionStack> ExceptionStacks { get; set; }

        public string UserId { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public IDictionary<string, double> Metrics { get; set; }
    }
}
