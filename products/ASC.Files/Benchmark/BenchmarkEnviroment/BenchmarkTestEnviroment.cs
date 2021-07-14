using System;

using ASC.Web.Files.Classes;

using Microsoft.Extensions.DependencyInjection;

namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public class BenchmarkTestEnviroment : IDisposable
    {
        private BenchmarkDb _db = new BenchmarkDb();

        public void Create()
        {
            _db.Create();

            var admin = new TestUser(new BenchmarkFilesHost(), true);
            admin.CreateFileInMy();
            admin.CreateCommonFolder();

            CreateUniqueDocumentSettings();
        }

        public void Drop()
        {
            _db.Drop();
        }

        public void Dispose()
        {
            Drop();
        }

        private void CreateUniqueDocumentSettings()
        {
            var scope = new BenchmarkFilesHost().Host.Services.CreateScope();
            var global = scope.ServiceProvider.GetService<Global>();
            global.GetDocDbKey();
        }
    }
}
