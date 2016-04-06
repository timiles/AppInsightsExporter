using System;
using System.Configuration;
using System.IO;
using BlobExporter;
using BlobExporter.Models;
using SlackImporter;

namespace SlackImporterApp
{
    class Program
    {
        private const bool DeleteBlobsOnceImported = false;

        static void Main(string[] args)
        {
            var blobClient = new BlobExporterClient(new StorageConfiguration(), new RunTracker());

            var exceptions = blobClient.ReadLatestExceptions();

            var slackWebhookUrl = ConfigurationManager.AppSettings["SlackWebhookUrl"];
            var slackClient = new SlackClient(slackWebhookUrl);

            foreach (var e in exceptions)
            {
                slackClient.PostMessage($"{e.EventTime.ToString("F")} UTC: `{e.Message}` ```{e.StackTrace.ToFriendlyString()}```");

                Console.WriteLine($"{e.EventTime.ToString("F")} UTC: {e.Message}");

                StoreOriginalBlobInfo(e.OriginalBlobInfo);

                if (DeleteBlobsOnceImported)
                {
                    blobClient.Delete(e.OriginalBlobInfo.Path);
                }
            }
        }

        private static void StoreOriginalBlobInfo(BlobInfo blobInfo)
        {
            var path = "Originals/" + blobInfo.Path;
            Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
            File.AppendAllText(path, blobInfo.Content);
        }
    }
}
