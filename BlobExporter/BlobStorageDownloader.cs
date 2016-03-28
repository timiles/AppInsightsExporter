using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobExporter
{
    internal class BlobStorageDownloader
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _appName;
        private readonly string _instrumentationKey;

        public BlobStorageDownloader(
            string connectionString,
            string containerName,
            string appName,
            Guid instrumentationKey)
        {
            _connectionString = connectionString;
            _containerName = containerName.ToLower();
            _appName = appName.ToLower();
            _instrumentationKey = instrumentationKey.ToString().Replace("-", "").ToLower();
        }

        public IEnumerable<string> DownloadExceptionsSince(DateTime sinceUtcDateTime)
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var containerReference = blobClient.GetContainerReference(_containerName);

            for (var listBlobsCreatedOnDate = sinceUtcDateTime.Date;
                listBlobsCreatedOnDate <= DateTime.UtcNow.Date;
                listBlobsCreatedOnDate = listBlobsCreatedOnDate.AddDays(1))
            {
                var prefix =
                    $@"{this._appName}_{this._instrumentationKey}/Exceptions/{listBlobsCreatedOnDate.ToString("yyyy-MM-dd")}/";
                // use flat blob listing to reduce calls to API
                foreach (var blob in containerReference.ListBlobs(prefix, useFlatBlobListing: true))
                {
                    // should all be CloudBlockBlobs, since we've used flat blot listing.
                    if (blob is CloudBlockBlob && ((CloudBlockBlob) blob).Properties.LastModified > sinceUtcDateTime)
                    {
                        yield return DownloadBlob(blobClient, blob);
                    }
                }
            }
        }

        private static string DownloadBlob(CloudBlobClient blobClient, IListBlobItem blob)
        {
            var cloudBlockBlob = blobClient.GetBlobReferenceFromServer(blob.Uri);
            using (var memoryStream = new MemoryStream())
            {
                cloudBlockBlob.DownloadToStream(memoryStream);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
