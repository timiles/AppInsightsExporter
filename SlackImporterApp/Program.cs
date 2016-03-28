using System;
using System.Configuration;
using BlobExporter;
using SlackImporter;

namespace SlackImporterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            var blobStorageContainerName = ConfigurationManager.AppSettings["BlobStorageContainerName"];
            var appName = ConfigurationManager.AppSettings["AppName"];
            var instrumentationKey = Guid.Parse(ConfigurationManager.AppSettings["InstrumentationKey"]);
            var slackWebhookUrl = ConfigurationManager.AppSettings["SlackWebhookUrl"];


            var blobClient = new BlobExporterClient(storageConnectionString, blobStorageContainerName, appName,
                instrumentationKey, new RunTracker());

            var exceptions = blobClient.ReadLatestExceptions();

            var slackClient = new SlackClient(slackWebhookUrl);

            foreach (var e in exceptions)
            {
                slackClient.PostMessage($"{e.EventTime.ToString("F")}: {e.Message}");
            }
        }
    }
}
