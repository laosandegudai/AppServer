using System.IO;

using ASC.Common.Logging;
using ASC.Core;
using ASC.Core.Tenants;
using ASC.Files.Helpers;
using ASC.Web.Files.Classes;
using ASC.Web.Files.Services.WCFService;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ASC.Files.Benchmark
{
    public class BenchmarkFilesSut
    {
        private ILog Log { get; set; }
        private FilesControllerHelper<int> FilesControllerHelper { get; set; }
        private GlobalFolderHelper GlobalFolderHelper { get; set; }
        private FileStorageService<int> FileStorageService { get; set; }
        private UserManager UserManager { get; set; }
        private Tenant CurrentTenant { get; set; }
        private SecurityContext SecurityContext { get; set; }
        private UserOptions UserOptions { get; set; }
        private IServiceScope Scope { get; set; }
        private IHost Host { get; set; }

        private const string TestConnection = "Server=localhost;Database=onlyoffice_benchmark;User ID=root;Password=onlyoffice;Pooling=true;Character Set=utf8;AutoEnlist=false;SSL Mode=none;AllowPublicKeyRetrieval=True";


        public BenchmarkFilesSut()
        {
            Host = Program.CreateHostBuilder(new string[] {
                "--pathToConf" , Path.Combine("..", "..", "..", "..", "..", "config"),
                "--ConnectionStrings:default:connectionString", TestConnection,
                 "--migration:enabled", "true" }).Build();
        }

        public void Setup()
        {
            Scope = Host.Services.CreateScope();

            var tenantManager = Scope.ServiceProvider.GetService<TenantManager>();
            var tenant = tenantManager.GetTenant(1);
            tenantManager.SetCurrentTenant(tenant);
            CurrentTenant = tenant;

            FilesControllerHelper = Scope.ServiceProvider.GetService<FilesControllerHelper<int>>();
            GlobalFolderHelper = Scope.ServiceProvider.GetService<GlobalFolderHelper>();
            UserManager = Scope.ServiceProvider.GetService<UserManager>();
            SecurityContext = Scope.ServiceProvider.GetService<SecurityContext>();
            UserOptions = Scope.ServiceProvider.GetService<IOptions<UserOptions>>().Value;
            FileStorageService = Scope.ServiceProvider.GetService<FileStorageService<int>>();
            Log = Scope.ServiceProvider.GetService<IOptionsMonitor<ILog>>().CurrentValue;

            SecurityContext.AuthenticateMe(CurrentTenant.OwnerId);
        }

        public int CreateFile()
        {
            return FilesControllerHelper
                .CreateFile(GlobalFolderHelper.FolderMy, "TestFile", default).Id;
        }

        public void DeleteFile(int fileId)
        {
            FilesControllerHelper.DeleteFile(fileId, false, true);
        }

        public void GetFileInfo(int fileId)
        {
            FilesControllerHelper.GetFileInfo(fileId);
        }

        public void UpdateFileStream(int fileId, Stream stream)
        {
            FilesControllerHelper.UpdateFileStream(stream, fileId);
        }
    }
}
