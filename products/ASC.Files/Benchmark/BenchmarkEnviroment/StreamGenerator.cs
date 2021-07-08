using System.IO;
using System.Text;

namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public static class StreamGenerator
    {
        public static TestStream Generate(int bytesCount)
        {
            var bytes = Encoding.UTF8.GetBytes("test");
            var buffer = new byte[bytesCount];

            var stream = new MemoryStream(buffer);

            while (stream.Position < stream.Length)
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            stream.Position = 0;

            return new TestStream(stream);
        }
    }
}
