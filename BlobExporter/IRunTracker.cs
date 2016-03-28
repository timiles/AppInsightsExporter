using System;

namespace BlobExporter
{
    public interface IRunTracker
    {
        DateTimeOffset LastRunDateTime { get; set; }
    }
}