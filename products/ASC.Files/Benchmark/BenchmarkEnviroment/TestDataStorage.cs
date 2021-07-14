using System;
using System.Collections.Generic;

using ASC.Core.Common.EF.Context;
using ASC.Core.Common.EF;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public class TestDataStorage
    {
        public List<TestUser> Users { get; private set; }

        private static TestDataStorage instance;
        private const string TestConnection = "Server=localhost;Database=onlyoffice_benchmark;User ID=root;Password=onlyoffice;Character Set=utf8;AutoEnlist=false;SSL Mode=none;AllowPublicKeyRetrieval=True;";

        private TestDataStorage()
        {
            var scope = Program.CreateHostBuilder(new string[] {
                "--pathToConf" , Path.Combine("..", "..", "..", "..", "..", "config"),
                "--ConnectionStrings:default:connectionString", TestConnection,
                 "--migration:enabled", "true" }).Build().Services.CreateScope();

            var appConfig = BenchmarkConfiguration.Build().Config;

            var usersCount = int.Parse(appConfig["Global:UsersCount"]);

            using var db = scope.ServiceProvider.GetService<DbContextManager<TenantDbContext>>();

            var host = new BenchmarkFilesHost();

            Users = new List<TestUser>();

            for (int i = 0; i < usersCount; i++)
            {
                CreateUser(db.Value, host);
            }

            db.Value.SaveChanges();
        }

        private void CreateUser(TenantDbContext context, BenchmarkFilesHost host)
        {
            var id = Guid.NewGuid();

            context.Users.Add(new User
            {
                Id = id,
                Email = "test@gmail.com",
                FirstName = "Test",
                LastName = "User",
                UserName = "TestUser",
                Tenant = 1,
                Status = ASC.Core.Users.EmployeeStatus.Active,
                LastModified = DateTime.Now,
                WorkFromDate = DateTime.Now
            });

            Users.Add(new TestUser(id, host));
        }

        public static TestDataStorage GetStorage()
        {
            if (instance == null)
                instance = new TestDataStorage();

            return instance;
        }
    }
}
