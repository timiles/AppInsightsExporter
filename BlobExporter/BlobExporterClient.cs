using System;
using System.Collections.Generic;
using BlobExporter.Models;

namespace BlobExporter
{
    public class BlobExporterClient
    {
        private readonly BlobStorageDownloader _blobStorageDownloader;
        private readonly IRunTracker _runTracker;

        public BlobExporterClient(IStorageConfiguration storageConfiguration, IRunTracker runTracker)
        {
            _blobStorageDownloader = new BlobStorageDownloader(
                storageConfiguration.StorageConnectionString,
                storageConfiguration.BlobStorageContainerName,
                storageConfiguration.AppName,
                storageConfiguration.InstrumentationKey);

            _runTracker = runTracker;
        }

        public IEnumerable<ExceptionTelemetry> ReadLatestExceptions()
        {
            var exceptionBlobs = this._blobStorageDownloader.DownloadExceptionsSince(this._runTracker.LastRunDateTime);

            // Use LastModified date to track when last run
            var lastModified = DateTimeOffset.MinValue;
            foreach (var blob in exceptionBlobs)
            {
                if (blob.LastModified.HasValue && blob.LastModified.Value > lastModified)
                {
                    lastModified = blob.LastModified.Value;
                }

                // valid to have multiple json objects in one blob file
                foreach (var json in blob.Content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    yield return TelemetryJsonParser.Parse(json);
                }
            }

            if (lastModified > DateTimeOffset.MinValue)
            {
                this._runTracker.LastRunDateTime = lastModified;
            }
        }
    }
}
