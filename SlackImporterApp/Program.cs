using System.Configuration;
using SlackImporter;

namespace SlackImporterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var slackWebhookUrl = ConfigurationManager.AppSettings["SlackWebhookUrl"];
            var slackClient = new SlackClient(slackWebhookUrl);
            slackClient.PostMessage("Test message", "Test username");
        }
    }
}
