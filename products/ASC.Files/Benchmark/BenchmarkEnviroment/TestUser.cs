using System;
using System.Collections.Generic;
using System.IO;

using ASC.Api.Documents;
using ASC.Core;
using ASC.Files.Benchmark.Utils;
using ASC.Files.Helpers;
using ASC.Web.Files.Classes;
using ASC.Web.Files.Services.WCFService;

using Microsoft.Extensions.DependencyInjection;

namespace ASC.Files.Benchmark.BenchmarkEnviroment
{
    public class TestUser
    {
        public Guid Id { get; private set; }

        private bool isAdmin;
        private FilesControllerHelper<int> filesControllerHelper;
        private GlobalFolderHelper globalFolderHelper;
        private FileStorageService<int> fileStorageService;
        private SecurityContext securityContext;
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
            securityContext = scope.ServiceProvider.GetService<SecurityContext>();
            fileStorageService = scope.ServiceProvider.GetService<FileStorageService<int>>();

            securityContext.AuthenticateMe(Id);
        }

        public int CreateFileInMy()
        {
            UpdateScope();
            var result = filesControllerHelper
               .CreateFile(globalFolderHelper.FolderMy, "TestFile", default).Id;
            scope.Dispose();
            scope = null;
            return result;
        }

        public int CreateFile(int folderId)
        {
            UpdateScope();
            var result = filesControllerHelper
                .CreateFile(folderId, "TestFile", default).Id;
            scope.Dispose();
            scope = null;
            return result;
        }

        public void DeleteFile(int fileId)
        {
            UpdateScope();
            filesControllerHelper.DeleteFile(fileId, false, true);
            scope.Dispose();
            scope = null;
        }

        public void GetFileInfo(int fileId)
        {
            UpdateScope();
            filesControllerHelper.GetFileInfo(fileId);
            scope.Dispose();
            scope = null;
        }

        public void UpdateFileStream(int fileId, Stream stream)
        {
            UpdateScope();
            filesControllerHelper.UpdateFileStream(stream, fileId);
            scope.Dispose();
            scope = null;
        }

        public void OpenEdit(int fileId)
        {
            UpdateScope();
            filesControllerHelper.OpenEdit(fileId, 1, null);
            scope.Dispose();
            scope = null;
        }

        public void SaveEditing(int fileId)
        {
            UpdateScope();
            filesControllerHelper.SaveEditing(fileId, "docx", string.Empty, StreamGenerator.Generate(1024).Stream, null, false);
            scope.Dispose();
            scope = null;
        }

        public void StartEdit(int fileId)
        {
            UpdateScope();
            filesControllerHelper.StartEdit(fileId, true, null);
            scope.Dispose();
            scope = null;
        }

        public void AddToFavorites(IEnumerable<int> foldersId, IEnumerable<int> filesId)
        {
            UpdateScope();
            fileStorageService.AddToFavorites(foldersId, filesId);
            scope.Dispose();
            scope = null;
        }

        public void DeleteFavorites(IEnumerable<int> foldersId, IEnumerable<int> filesId)
        {
            UpdateScope();
            fileStorageService.DeleteFavorites(foldersId, filesId);
        }

        public int CreateFolderInMy()
        {
            UpdateScope();
            var result = filesControllerHelper.CreateFolder(globalFolderHelper.FolderMy, "TestFolder").Id;
            scope.Dispose();
            scope = null;
            return result;
        }

        public int CreateCommonFolder()
        {
            UpdateScope();
            var result = filesControllerHelper.CreateFolder(globalFolderHelper.FolderCommon, "common").Id;
            scope.Dispose();
            scope = null;
            return result;
        }

        public int CreateFolder(int folderId)
        {
            UpdateScope();
            var result = filesControllerHelper.CreateFolder(folderId, "TestFolder").Id;
            scope.Dispose();
            scope = null;
            return result;
        }

        public void DeleteFolder(int folderId)
        {
            UpdateScope();
            filesControllerHelper.DeleteFolder(folderId, false, true);
            scope.Dispose();
            scope = null;
        }

        public FolderContentWrapper<int> GetFolder(int folderId)
        {
            UpdateScope();
            var result = filesControllerHelper.GetFolder(folderId, Id, Core.FilterType.None, false);
            scope.Dispose();
            scope = null;
            return result;
        }

        public void RenameFolder(int folderId, string newTitle)
        {
            UpdateScope();
            filesControllerHelper.RenameFolder(folderId, newTitle);
            scope.Dispose();
            scope = null;
        }
    }
}
