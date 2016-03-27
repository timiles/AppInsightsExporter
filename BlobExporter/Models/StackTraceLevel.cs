namespace BlobExporter.Models
{
    public class StackTraceLevel
    {
        public string Method { get; set; }

        public string Assembly { get; set; }

        public string FileName { get; set; }

        public int LineNumber { get; set; }
    }
}