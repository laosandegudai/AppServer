using System;
using System.Collections.Generic;
using System.IO;

using ASC.Api.Documents;
using ASC.Common.Logging;
using ASC.Core;
using ASC.Files.Benchmark.Utils;
using ASC.Files.Helpers;
using ASC.Web.Files.Classes;
using ASC.Web.Files.Services.WCFService;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public class TestUser
    {
        public Guid Id { get; private set; }

        private bool isAdmin;
        private ILog log;
        private FilesControllerHelper<int> filesControllerHelper;
        private GlobalFolderHelper globalFolderHelper;
        private FileStorageService<int> fileStorageService;
        private UserManager userManager;
        private SecurityContext securityContext;
        private UserOptions userOptions;
        private IServiceScope scope;
        private BenchmarkFilesHost host;

        public TestUser(BenchmarkFilesHost host, bool isAdmin)
        {
            this.host = host;
            this.isAdmin = isAdmin;
        }

        public TestUser(Guid id, BenchmarkFilesHost host)
        {
            Id = id;
            this.host = host;
        }

        private void UpdateScope()
        {
            scope = host.Host.Services.CreateScope();

            var tenantManager = scope.ServiceProvider.GetService<TenantManager>();
            var tenant = tenantManager.GetTenant(1);
            tenantManager.SetCurrentTenant(tenant);

            if (isAdmin) Id = tenant.OwnerId;

            filesControllerHelper = scope.ServiceProvider.GetService<FilesControllerHelper<int>>();
            globalFolderHelper = scope.ServiceProvider.GetService<GlobalFolderHelper>();
            userManager = scope.ServiceProvider.GetService<UserManager>();
            securityContext = scope.ServiceProvider.GetService<SecurityContext>();
            userOptions = scope.ServiceProvider.GetService<IOptions<UserOptions>>().Value;
            fileStorageService = scope.ServiceProvider.GetService<FileStorageService<int>>();
            log = scope.ServiceProvider.GetService<IOptionsMonitor<ILog>>().CurrentValue;

            securityContext.AuthenticateMe(Id);
        }

        public int CreateFileInMy()
        {
            UpdateScope();
            return filesControllerHelper
               .CreateFile(globalFolderHelper.FolderMy, "TestFile", default).Id;
        }

        public int CreateFile(int folderId)
        {
            UpdateScope();
            return filesControllerHelper
                .CreateFile(folderId, "TestFile", default).Id;
        }

        public void DeleteFile(int fileId)
        {
            UpdateScope();
            filesControllerHelper.DeleteFile(fileId, false, true);
        }

        public void GetFileInfo(int fileId)
        {
            UpdateScope();
            filesControllerHelper.GetFileInfo(fileId);
        }

        public void UpdateFileStream(int fileId, Stream stream)
        {
            UpdateScope();
            filesControllerHelper.UpdateFileStream(stream, fileId);
        }

        public void OpenEdit(int fileId)
        {
            UpdateScope();
            filesControllerHelper.OpenEdit(fileId, 1, null);
        }

        public void SaveEditing(int fileId)
        {
            UpdateScope();
            filesControllerHelper.SaveEditing(fileId, "docx", string.Empty, StreamGenerator.Generate(1024).Stream, null, false);
        }

        public void StartEdit(int fileId)
        {
            UpdateScope();
            filesControllerHelper.StartEdit(fileId, true, null);
        }

        public void AddToFavorites(IEnumerable<int> foldersId, IEnumerable<int> filesId)
        {
            UpdateScope();
            fileStorageService.AddToFavorites(foldersId, filesId);
        }

        public void DeleteFavorites(IEnumerable<int> foldersId, IEnumerable<int> filesId)
        {
            UpdateScope();
            fileStorageService.DeleteFavorites(foldersId, filesId);
        }

        public int CreateFolderInMy()
        {
            UpdateScope();
            return filesControllerHelper.CreateFolder(globalFolderHelper.FolderMy, "TestFolder").Id;
        }

        public int CreateCommonFolder()
        {
            UpdateScope();
            return filesControllerHelper.CreateFolder(globalFolderHelper.FolderCommon, "common").Id;
        }

        public int CreateFolder(int folderId)
        {
            UpdateScope();
            return filesControllerHelper.CreateFolder(folderId, "TestFolder").Id;
        }

        public void DeleteFolder(int folderId)
        {
            UpdateScope();
            filesControllerHelper.DeleteFolder(folderId, false, true);
        }

        public FolderContentWrapper<int> GetFolder(int folderId)
        {
            UpdateScope();
            return filesControllerHelper.GetFolder(folderId, Id, Core.FilterType.None, false);
        }

        public void RenameFolder(int folderId, string newTitle)
        {
            UpdateScope();
            filesControllerHelper.RenameFolder(folderId, newTitle);
        }
    }
}
