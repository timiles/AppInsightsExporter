using System;
using System.Collections.Generic;
using System.Linq;
using BlobExporter.JsonModels.Exception;
using BlobExporter.Models;
using Newtonsoft.Json;

namespace BlobExporter
{
    internal static class TelemetryJsonParser
    {
        public static ExceptionTelemetry Parse(string json)
        {
            var e = JsonConvert.DeserializeObject<RootObject>(json);

            var basicException = e.basicException[0];
            var stackTrace = e.basicException.Last().parsedStack;

            return new ExceptionTelemetry
            {
                Message = basicException.innermostExceptionMessage,
                StackTrace = stackTrace.Select(x => new StackTraceLevel
                {
                    Method = x.method,
                    Assembly = x.assembly,
                    FileName = x.fileName,
                    LineNumber = x.line,
                }),
                UserId = e.context.user.anonId,
                Properties = FlattenDictionary(e.context.custom.dimensions, x => x),
                Metrics = FlattenDictionary(e.context.custom.metrics, x => x.value)
            };
        }

        private static IDictionary<string, TValue> FlattenDictionary<TSource, TValue>(IEnumerable<IDictionary<string, TSource>> dictionary, Func<TSource, TValue> getValue)
        {
            var flattened = new Dictionary<string, TValue>();
            foreach (var d in dictionary)
            {
                foreach (var key in d.Keys)
                {
                    flattened.Add(key, getValue(d[key]));
                }
            }
            return flattened;
        }
    }
}
