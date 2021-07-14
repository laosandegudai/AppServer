using System;


namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public class BenchmarkTestEnviroment : IDisposable
    {
        private BenchmarkDb db = new BenchmarkDb();

        public void Create()
        {
            db.Create();
            var admin = new TestUser(new BenchmarkFilesHost(), true);
            admin.CreateFileInMy();
            admin.CreateCommonFolder();
        }

        public void Drop()
        {
            db.Drop();
        }

        public void Dispose()
        {
            Drop();
        }
    }
}
