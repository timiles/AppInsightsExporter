using System;
using System.Collections.Generic;
using BlobExporter.Models;

namespace BlobExporter
{
    public class BlobExporterClient
    {
        private readonly BlobStorageClient _blobStorageClient;
        private readonly IRunTracker _runTracker;

        public BlobExporterClient(IStorageConfiguration storageConfiguration, IRunTracker runTracker)
        {
            _blobStorageClient = new BlobStorageClient(
                storageConfiguration.StorageConnectionString,
                storageConfiguration.BlobStorageContainerName,
                storageConfiguration.AppName,
                storageConfiguration.InstrumentationKey);

            _runTracker = runTracker;
        }

        public IEnumerable<ExceptionInfo> ReadLatestExceptions()
        {
            var exceptionBlobs = this._blobStorageClient.DownloadExceptionsSince(this._runTracker.LastRunDateTime);

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
                    var exceptionTelemetry = ExceptionInfoJsonParser.Parse(json);
                    exceptionTelemetry.OriginalBlobInfo = blob;
                    yield return exceptionTelemetry;
                }
            }

            if (lastModified > DateTimeOffset.MinValue)
            {
                this._runTracker.LastRunDateTime = lastModified;
            }
        }

        public void Delete(string path)
        {
            this._blobStorageClient.DeleteBlob(path);
        }
    }
}
