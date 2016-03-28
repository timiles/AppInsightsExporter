using System;
using System.Collections.Generic;

namespace BlobExporter.JsonModels.Exception
{
    public class ParsedStack
    {
        public string method { get; set; }
        public string assembly { get; set; }
        public string fileName { get; set; }
        public int level { get; set; }
        public int line { get; set; }
    }

    public class BasicException
    {
        public string assembly { get; set; }
        public bool hasFullStack { get; set; }
        public string exceptionType { get; set; }
        public string outerExceptionType { get; set; }
        public string outerExceptionThrownAtMethod { get; set; }
        public string outerExceptionThrownAtAssembly { get; set; }
        public string innermostExceptionType { get; set; }
        public string innermostExceptionThrownAtMethod { get; set; }
        public string innermostExceptionThrownAtAssembly { get; set; }
        public string failedUserCodeMethod { get; set; }
        public string failedUserCodeAssembly { get; set; }
        public string exceptionGroup { get; set; }
        public string id { get; set; }
        public string outerId { get; set; }
        public string typeName { get; set; }
        public string message { get; set; }
        public string handledAt { get; set; }
        public int count { get; set; }
        public string method { get; set; }
        public string outerExceptionMessage { get; set; }
        public string innermostExceptionMessage { get; set; }
        public List<ParsedStack> parsedStack { get; set; }
    }

    public class Internal
    {
        public Data data { get; set; }

        public class Data
        {
            public string id { get; set; }
            public string documentVersion { get; set; }
        }
    }

    public class ScreenResolution
    {
    }

    public class Device
    {
        public string osVersion { get; set; }
        public string type { get; set; }
        public string browser { get; set; }
        public string browserVersion { get; set; }
        public string network { get; set; }
        public ScreenResolution screenResolution { get; set; }
        public string locale { get; set; }
        public string id { get; set; }
        public string roleInstance { get; set; }
        public string oemName { get; set; }
        public string deviceName { get; set; }
        public string deviceModel { get; set; }
    }

    public class Application
    {
    }

    public class Location
    {
        public string continent { get; set; }
        public string country { get; set; }
        public string clientip { get; set; }
        public string province { get; set; }
        public string city { get; set; }
    }

    public class User
    {
        public bool isAuthenticated { get; set; }
        public string anonId { get; set; }
        public string anonAcquisitionDate { get; set; }
        public string authAcquisitionDate { get; set; }
        public string accountAcquisitionDate { get; set; }
    }

    public class Operation
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string name { get; set; }
    }

    public class Cloud
    {
    }

    public class ServerDevice
    {
    }

    public class Metric
    {
        public double value { get; set; }
        public double count { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double stdDev { get; set; }
        public double sampledValue { get; set; }
        public double sum { get; set; }
    }

    public class Custom
    {
        public IEnumerable<IDictionary<string, string>> dimensions { get; set; }
        public IEnumerable<IDictionary<string, Metric>> metrics { get; set; }
    }

    public class Session
    {
    }

    public class Context
    {
        public Device device { get; set; }
        public Application application { get; set; }
        public Location location { get; set; }
        public Data data { get; set; }
        public User user { get; set; }
        public Operation operation { get; set; }
        public Cloud cloud { get; set; }
        public ServerDevice serverDevice { get; set; }
        public Custom custom { get; set; }
        public Session session { get; set; }

        public class Data
        {
            public bool isSynthetic { get; set; }
            public double samplingRate { get; set; }
            public DateTimeOffset eventTime { get; set; }
        }
    }

    public class RootObject
    {
        public List<BasicException> basicException { get; set; }
        public Internal @internal { get; set; }
        public Context context { get; set; }
    }
}
