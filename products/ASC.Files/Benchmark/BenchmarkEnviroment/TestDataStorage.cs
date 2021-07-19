using System;
using System.Collections.Generic;

using ASC.Core.Common.EF.Context;
using ASC.Core.Common.EF;
using Microsoft.Extensions.DependencyInjection;

namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public class TestDataStorage
    {
        public List<TestUser> Users { get; private set; }

        private static TestDataStorage instance;

        private TestDataStorage()
        {
            var benchHost = new BenchmarkFilesHost(false);

            var scope = benchHost.Host.Services.CreateScope();

            var appConfig = BenchmarkConfiguration.Build().Config;

            var usersCount = int.Parse(appConfig["Global:UsersCount"]);

            using var db = scope.ServiceProvider.GetService<DbContextManager<TenantDbContext>>();

            Users = new List<TestUser>();

            for (int i = 0; i < usersCount; i++)
            {
                CreateUser(db.Value, benchHost);
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
