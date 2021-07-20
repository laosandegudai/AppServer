using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;
using ASC.Files.Benchmark.Utils;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(UpdateFileStreamTestConfig))]
    public class UpdateFileStreamBenchmark : BenchmarkBase
    {
        private int _fileId;
        private List<int> _filesId = new List<int>();
        private Task[] _tasks;
        private Stream _stream;
        private List<Stream> _streams = new List<Stream>();
        private int _bytesCount;

        public UpdateFileStreamBenchmark()
        {
            _bytesCount = int.Parse(_config["Files:UpdateFileStreamTest:StreamSizeKB"]) * 1024;
        }

        #region UpdateFileStreamTest
        [GlobalSetup(Target = nameof(UpdateFileStreamTest))]
        public void IterSetupUpdateFileStreamTest()
        {
            _fileId = _dataStorage.Users[0].CreateFileInMy();
            _stream = StreamGenerator.Generate(_bytesCount);
        }

        [Benchmark]
        public void UpdateFileStreamTest()
        {
            _stream.Position = 0;
            _dataStorage.Users[0].UpdateFileStream(_fileId, _stream);
        }
        #endregion

        #region UpdateFileStreamManyUsersTest
        [GlobalSetup(Target = nameof(UpdateFileStreamManyUsersTest))]
        public void GlobalSetupUpdateFileStreamManyUsersTest()
        {
            var templateStream = StreamGenerator.Generate(_bytesCount);

            foreach (var user in _dataStorage.Users)
            {
                _filesId.Add(user.CreateFileInMy());

                templateStream.Position = 0;
                var memStream = new MemoryStream();
                templateStream.CopyTo(memStream);

                _streams.Add(memStream);
            }
        }

        [IterationSetup(Target = nameof(UpdateFileStreamManyUsersTest))]
        public void IterSetupUpdateFileStreamManyUsersTest()
        {
            _tasks = new Task[_dataStorage.Users.Count];

            for (int i = 0; i < _dataStorage.Users.Count; i++)
            {
                var user = _dataStorage.Users[i];
                var fileId = _filesId[i];
                var stream = _streams[i];

                stream.Position = 0;

                _tasks[i] = new Task(() =>
                {
                    user.UpdateFileStream(fileId, stream);
                }, TaskCreationOptions.LongRunning);
                _tasks[i].ConfigureAwait(false);
            }
        }

        [Benchmark]
        public void UpdateFileStreamManyUsersTest()
        {
            foreach (var task in _tasks)
            {
                task.Start();
            }

            Task.WaitAll(_tasks);
        }
        #endregion
    }
}
