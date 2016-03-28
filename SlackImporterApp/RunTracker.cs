using System;
using System.IO;
using BlobExporter;

namespace SlackImporterApp
{
    internal class RunTracker : IRunTracker
    {
        private const string LastRunFileName = @"_lastrun.txt";

        public DateTimeOffset LastRunDateTime
        {
            get
            {
                try
                {
                    var fileContents = File.ReadAllText(LastRunFileName);
                    return DateTimeOffset.Parse(fileContents);
                }
                catch (Exception ex)
                {
                    // TODO: log exception
                   
                    // default to the past hour
                    return DateTimeOffset.UtcNow.AddHours(-1);
                }
            }
            set
            {
                File.WriteAllText(LastRunFileName, value.ToString());
            }
        }
    }
}