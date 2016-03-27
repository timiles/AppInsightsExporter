﻿using System.Collections.Generic;

namespace BlobExporter.Models
{
    public class ExceptionTelemetry
    {
        public string Message { get; set; }

        public IEnumerable<StackTraceLevel> StackTrace { get; set; }

        public string UserId { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public IDictionary<string, double> Metrics { get; set; }
    }
}
