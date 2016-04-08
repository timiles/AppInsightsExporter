using System;
using System.Configuration;
using System.IO;
using BlobExporter;
using BlobExporter.Models;
using SlackImporter;
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
                    var messageBuilder = new StringBuilder(e.EventTime.ToString("F"));
                    messageBuilder.AppendLine(" UTC:");
                    if (!string.IsNullOrWhiteSpace(e.Operation))
                    {
                        messageBuilder.Append("Operation: `");
                        messageBuilder.Append(e.Operation);
                        messageBuilder.AppendLine("`");
                    }
                    AppendStacks(messageBuilder, e.ExceptionStacks);

                    var message = messageBuilder.ToString();
                    slackClient.PostMessage(message);
                    Console.WriteLine(message);
                }

                StoreOriginalBlobInfo(batch.OriginalBlobInfo);

                if (DeleteBlobsOnceImported)
                {
                    blobClient.Delete(batch.OriginalBlobInfo.Path);
                }
            }
        }

        private static void AppendStacks(StringBuilder messageBuilder, IEnumerable<ExceptionStack> exceptionStacks)
        {
            foreach (var e in exceptionStacks)
            {
                messageBuilder.AppendLine($"`{e.Message}` ```{e.StackTrace.ToFriendlyString()}```");
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
