using System.IO;

namespace ASC.Files.Benchmark.Utils
{
    public class TestStream
    {
        public Stream Stream { get; }

        public TestStream(Stream stream)
        {
            Stream = stream;
        }

        public override string ToString()
        {
            return $"{Stream.Length / 1024} KB";
        }
    }
}
