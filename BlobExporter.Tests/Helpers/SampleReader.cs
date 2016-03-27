using System.IO;

namespace BlobExporter.Tests.Helpers
{
    internal static class SampleReader
    {
        public static string ReadSample(string fileName)
        {
            return File.ReadAllText(@"Samples/" + fileName);
        }
    }
}
