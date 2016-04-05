using BlobExporter.Models;
using System.Collections.Generic;
using System.Text;

namespace BlobExporter
{
    public static class Extensions
    {
        public static string ToFriendlyString(this IEnumerable<StackTraceLevel> stackTrace)
        {
            var sb = new StringBuilder();

            var previousLevel = new StackTraceLevel();
            foreach (var level in stackTrace)
            {
                if (level.Assembly != previousLevel.Assembly)
                {
                    sb.AppendLine(level.Assembly);
                }

                sb.Append(" -- ");
                sb.AppendLine(level.Method);

                if (!string.IsNullOrEmpty(level.FileName))
                {
                    sb.AppendLine($"      ({level.FileName}:{level.LineNumber})");
                }

                previousLevel = level;
            }
            return sb.ToString();
        }
    }
}
