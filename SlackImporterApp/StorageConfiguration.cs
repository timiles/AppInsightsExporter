using System;
using System.Configuration;
using BlobExporter;

namespace SlackImporterApp
{
    class StorageConfiguration : IStorageConfiguration
    {
        public string StorageConnectionString => ConfigurationManager.AppSettings["StorageConnectionString"];

        public string BlobStorageContainerName => ConfigurationManager.AppSettings["BlobStorageContainerName"];

        public string AppName => ConfigurationManager.AppSettings["AppName"];

        public Guid InstrumentationKey => Guid.Parse(ConfigurationManager.AppSettings["InstrumentationKey"]);
    }
}
