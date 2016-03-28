using System;

namespace BlobExporter
{
    public interface IStorageConfiguration
    {
        string StorageConnectionString { get; }

        string BlobStorageContainerName { get; }

        string AppName { get; }

        Guid InstrumentationKey { get; }
    }
}