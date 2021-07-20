using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using ASC.Files.Benchmark.TestsConfiguration;
using ASC.Files.Benchmark.Utils;

using BenchmarkDotNet.Attributes;

namespace ASC.Files.Benchmark.Benchmarks
{
    [Config(typeof(EditFileTestConfig))]
    public class EditFileBenchmark : BenchmarkBase
    {
        private int _fileId;
        private List<int> _filesId = new List<int>();
        private Task[] _tasks;
        private Stream _stream;
        private List<Stream> _streams = new List<Stream>();
        private int _bytesCount;

        public EditFileBenchmark()
        {
            _bytesCount = int.Parse(_config["Files:EditFileTest:StreamSizeKB"]) * 1024;
        }

        #region EditFileTest
        [GlobalSetup(Target = nameof(EditFileTest))]
        public void GlobalSetupEditFileTest()
        {
            _fileId = _dataStorage.Users[0].CreateFileInMy();
            _stream = StreamGenerator.Generate(_bytesCount);
        }

        [Benchmark]
        public void EditFileTest()
        {
            _stream.Position = 0;

            _dataStorage.Users[0].OpenEdit(_fileId);
            _dataStorage.Users[0].StartEdit(_fileId);
            _dataStorage.Users[0].SaveEditing(_fileId, _stream);
        }
        #endregion

        #region EditFileManyUsersTest
        [GlobalSetup(Target =nameof(EditFileManyUsersTest))]
        public void GlobalSetupEditFileManyUsersTest()
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

        [IterationSetup(Target =nameof(EditFileManyUsersTest))]
        public void IterSetupEditFileManyUsersTest()
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
                    user.OpenEdit(fileId);
                    user.StartEdit(fileId);
                    user.SaveEditing(fileId, stream);
                }, TaskCreationOptions.LongRunning);
                _tasks[i].ConfigureAwait(false);
            }
        }

        [Benchmark]
        public void EditFileManyUsersTest()
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
