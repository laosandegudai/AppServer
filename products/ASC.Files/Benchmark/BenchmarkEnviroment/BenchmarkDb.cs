using System;
using System.IO;
using System.Reflection;

using ASC.Core.Common.EF.Context;
using ASC.Core.Common.EF;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public class BenchmarkDb : IDisposable
    {
        protected IServiceScope Scope { get; set; }
        public const string TestConnection = "Server=localhost;Database=onlyoffice_benchmark;User ID=root;Password=onlyoffice;Pooling=true;Character Set=utf8;AutoEnlist=false;SSL Mode=none;AllowPublicKeyRetrieval=True";
        public void Create()
        {
            var host = Program.CreateHostBuilder(new string[] {
                "--pathToConf", Path.Combine("..", "..", "..", "..", "..", "config"),
                "--ConnectionStrings:default:connectionString", TestConnection,
                "--migration:enabled", "true",
                "--core:products:folder", Path.Combine("..", "..", "..", "products")}).Build();

            Migrate(host.Services);
            Migrate(host.Services, Assembly.GetExecutingAssembly().GetName().Name);

            Scope = host.Services.CreateScope();
        }

        public void Drop()
        {
            var context = Scope.ServiceProvider.GetService<DbContextManager<TenantDbContext>>();
            context.Value.Database.EnsureDeleted();
        }

        private void Migrate(IServiceProvider serviceProvider, string testAssembly = null)
        {
            using var scope = serviceProvider.CreateScope();

            if (!string.IsNullOrEmpty(testAssembly))
            {
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();
                configuration["testAssembly"] = testAssembly;
            }

            using var db = scope.ServiceProvider.GetService<DbContextManager<TenantDbContext>>();
            db.Value.Migrate();
        }

        public void Dispose()
        {
            Drop();
        }
    }
}
