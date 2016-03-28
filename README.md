# App Insights Exporter

Extract App Insights telemetry from Azure blob storage and integrate into other systems, eg Slack.
----

Microsoft Azure's [Application Insights](https://azure.microsoft.com/services/application-insights/) is great for tracking telemetry of your apps and servers, if what you need is high-level reporting or general availability alerts. But when it comes to real-time notifications of one-off events such as Exceptions, it kinda sucks. :disappointed:

This project is a simple tool for downloading and parsing Exceptions from Azure blob storage\*, with a sample app to show how you could then import that data into a Slack channel\*\*.

\* This method of exporting from App Insights requires the Continuous Export feature, which is only available on a paid tier (cheapest as of March 2016 is **15 GBP/month**).

\*\* To enable your Slack integration, configure some Webhooks [here](https://slack.com/apps/A0F7XDUAZ-incoming-webhooks).

Notes
-----

I have only considered Exceptions so far, since I have no need for real-time notifications of any other kind of telemetry.  Would happily consider other usages if they make sense, or even better, if you send me a pull request! :grin:
