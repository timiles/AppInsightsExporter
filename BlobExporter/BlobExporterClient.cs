using System;
using System.Collections.Generic;
using BlobExporter.Models;

namespace BlobExporter
{
    public class BlobExporterClient
    {
        private readonly string _storageConnectionString;
        private readonly string _blobStorageContainerName;
        private readonly string _appName;
        private readonly Guid _instrumentationKey;

        public BlobExporterClient(
            string storageConnectionString,
            string blobStorageContainerName,
            string appName,
            Guid instrumentationKey)
        {
            _storageConnectionString = storageConnectionString;
            _blobStorageContainerName = blobStorageContainerName;
            _appName = appName;
            _instrumentationKey = instrumentationKey;
        }

        public IEnumerable<ExceptionTelemetry> ReadExceptionsSince(DateTime sinceUtcDateTime)
        {
            var downloader = new BlobStorageDownloader(_storageConnectionString, _blobStorageContainerName, _appName,
                _instrumentationKey);
            var exceptionsJsons = downloader.DownloadExceptionsSince(sinceUtcDateTime);

            foreach (var json in exceptionsJsons)
            {
                yield return TelemetryJsonParser.Parse(json);
            }
        } 
    }
}
