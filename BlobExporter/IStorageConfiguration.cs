using System;

namespace BlobExporter
{
    public interface IStorageConfiguration
    {
        string StorageConnectionString { get; }

        string BlobStorageContainerName { get; }

        string AppInsightsName { get; }

        Guid InstrumentationKey { get; }
    }
}