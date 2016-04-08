using System;
using System.Collections.Generic;
using System.Linq;
using BlobExporter.JsonModels.Exception;
using BlobExporter.Models;
using Newtonsoft.Json;

namespace BlobExporter
{
    internal static class ExceptionInfoJsonParser
    {
        public static ExceptionInfo Parse(string json)
        {
            var e = JsonConvert.DeserializeObject<RootObject>(json);

            var basicException = e.basicException[0];
            var stackTraces = e.basicException.Where(x => x.parsedStack != null);

            return new ExceptionInfo
            {
                Operation = e.context.operation?.name,
                EventTime = e.context.data.eventTime,
                ExceptionStacks = stackTraces.Select(stack => new ExceptionStack
                {
                    Message = stack.message,
                    StackTrace = stack.parsedStack.Select(level => new StackTraceLevel
                    {
                        Method = level.method,
                        Assembly = level.assembly,
                        FileName = level.fileName,
                        LineNumber = level.line,
                    })
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
