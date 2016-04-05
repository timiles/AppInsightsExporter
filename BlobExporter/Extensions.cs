using BlobExporter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlobExporter
{
    public static class Extensions
    {
        public static string ToFriendlyString(this IEnumerable<StackTraceLevel> stackTrace)
        {
            var includeAssemblyInfo = stackTrace.Any(x => !string.IsNullOrWhiteSpace(x.Assembly));
            var indentSize = includeAssemblyInfo ? 4 : 0;

            var sb = new StringBuilder();

            var previousLevel = new StackTraceLevel();
            foreach (var level in stackTrace)
            {
                if (includeAssemblyInfo)
                {
                    if (level.Assembly != previousLevel.Assembly)
                    {
                        sb.AppendLine(level.Assembly);
                    }

                    sb.Append(' ', indentSize);
                }

                sb.AppendLine(level.Method);

                if (!string.IsNullOrEmpty(level.FileName))
                {
                    sb.Append(' ', indentSize + 2);
                    sb.AppendLine($"({level.FileName} line:{level.LineNumber})");
                }

                previousLevel = level;
            }
            return sb.ToString();
        }
    }
}
