using System;
using System.Configuration;
using System.IO;
using BlobExporter;
using BlobExporter.Models;
using SlackImporter;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SlackImporterApp
{
    class Program
    {
        private const bool DeleteBlobsOnceImported = false;

        static void Main(string[] args)
        {
            var blobClient = new BlobExporterClient(new StorageConfiguration(), new RunTracker());

            var batchedExceptions = blobClient.ReadLatestExceptions();

            var slackWebhookUrl = ConfigurationManager.AppSettings["SlackWebhookUrl"];
            var slackClient = new SlackClient(slackWebhookUrl);

            foreach (var batch in batchedExceptions)
            {
                foreach (var e in batch.ExceptionInfos)
                {
                    slackClient.PostMessage($"{e.EventTime.ToString("F")} UTC: {PrintStacks(e.ExceptionStacks)}");
                    Console.WriteLine($"{e.EventTime.ToString("F")} UTC: {e.ExceptionStacks.First().Message}");
                }

                StoreOriginalBlobInfo(batch.OriginalBlobInfo);

                if (DeleteBlobsOnceImported)
                {
                    blobClient.Delete(batch.OriginalBlobInfo.Path);
                }
            }
        }

        private static string PrintStacks(IEnumerable<ExceptionStack> exceptionStacks)
        {
            var stacks = new StringBuilder();
            foreach (var e in exceptionStacks)
            {
                stacks.AppendLine($"`{e.Message}` ```{e.StackTrace.ToFriendlyString()}```");
            }
            return stacks.ToString();
        }

        private static void StoreOriginalBlobInfo(BlobInfo blobInfo)
        {
            var path = "Originals/" + blobInfo.Path;
            Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
            File.AppendAllText(path, blobInfo.Content);
        }
    }
}
