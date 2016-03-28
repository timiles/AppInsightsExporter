using System.Configuration;
using BlobExporter;
using SlackImporter;

namespace SlackImporterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var blobClient = new BlobExporterClient(new StorageConfiguration(), new RunTracker());

            var exceptions = blobClient.ReadLatestExceptions();

            var slackWebhookUrl = ConfigurationManager.AppSettings["SlackWebhookUrl"];
            var slackClient = new SlackClient(slackWebhookUrl);

            foreach (var e in exceptions)
            {
                slackClient.PostMessage($"{e.EventTime.ToString("F")}: {e.Message}");
            }
        }
    }
}
