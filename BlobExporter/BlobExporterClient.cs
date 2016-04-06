using System;
using System.Collections.Generic;
using BlobExporter.Models;
using System.Linq;

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

        public IEnumerable<ExceptionBatch> ReadLatestExceptions()
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
                var lines = blob.Content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                yield return new ExceptionBatch
                {
                    ExceptionInfos = lines.Select(ExceptionInfoJsonParser.Parse).OrderBy(x => x.EventTime),
                    OriginalBlobInfo = blob,
                };
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
