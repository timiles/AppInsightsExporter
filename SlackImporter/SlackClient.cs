// REF: https://gist.github.com/jogleasonjr/7121367
namespace SlackImporter
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;

    public class SlackClient
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public SlackClient(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }

        public void PostMessage(string text, string username = null, string channel = null)
        {
            var payload = new Payload
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            var payloadJson = JsonConvert.SerializeObject(payload);

            using (var client = new WebClient())
            {
                var data = new NameValueCollection {["payload"] = payloadJson };
                var response = client.UploadValues(_uri, "POST", data);

                var responseText = _encoding.GetString(response);
                if (responseText != "ok")
                {
                    throw new ApplicationException("Unexpected response from Slack API: " + responseText);
                }
            }
        }
    }
}
