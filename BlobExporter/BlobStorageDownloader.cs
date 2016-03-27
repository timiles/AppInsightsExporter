using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobExporter
{
    public class BlobStorageDownloader
    {
        private readonly string _containerName;
        private readonly string _connectionString;

        public BlobStorageDownloader(string containerName)
        {
            _containerName = containerName;
            _connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
        }

        public IEnumerable<string> Import()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var containerReference = blobClient.GetContainerReference(_containerName);

            // use flat blob listing to reduce calls to API
            foreach (var blob in containerReference.ListBlobs(useFlatBlobListing: true))
            {
                yield return DownloadBlob(blobClient, blob);
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
