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

        

        public int CreateFileInMy()
        {
            int id = 0;

            InitialScope(() =>
            {
                id = filesControllerHelper
               .CreateFile(globalFolderHelper.FolderMy, "TestFile", default).Id;
            });

            return id;
        }

        public int CreateFile(int folderId)
        {
            int id = 0;

            InitialScope(() =>
            {
                id = filesControllerHelper
                .CreateFile(folderId, "TestFile", default).Id;
            });

            return id;
        }

        public void DeleteFile(int fileId)
        {
            InitialScope(() =>
            {
                filesControllerHelper.DeleteFile(fileId, false, true);
            });
        }

        public void GetFileInfo(int fileId)
        {
            InitialScope(() =>
            {
                filesControllerHelper.GetFileInfo(fileId);
            });
        }

        public void UpdateFileStream(int fileId, Stream stream)
        {
            InitialScope(() =>
            {
                filesControllerHelper.UpdateFileStream(stream, fileId);
            });
        }

        public void OpenEdit(int fileId)
        {
            InitialScope(() =>
            {
                filesControllerHelper.OpenEdit(fileId, 1, null);
            });
        }

        public void SaveEditing(int fileId)
        {
            InitialScope(() =>
            {
                filesControllerHelper
                .SaveEditing(fileId, "docx",
                string.Empty, StreamGenerator.Generate(1024).Stream, null, false);
            });
        }

        public void StartEdit(int fileId)
        {
            InitialScope(() =>
            {
                filesControllerHelper.StartEdit(fileId, true, null);
            });
        }

        public void ShareFile(int fileId, Guid userId)
        {
            InitialScope(() =>
            {
                filesControllerHelper.SetFileSecurityInfo(fileId,
                    new List<FileShareParams>()
                    {
                        new FileShareParams() { Access = Core.Security.FileShare.ReadWrite, ShareTo = userId }
                    }, false, "share");
            });
        }

        public void AddToFavorites(IEnumerable<int> foldersId, IEnumerable<int> filesId)
        {
            InitialScope(() =>
            {
                fileStorageService.AddToFavorites(foldersId, filesId);
            });
        }

        public void DeleteFavorites(IEnumerable<int> foldersId, IEnumerable<int> filesId)
        {
            InitialScope(() =>
            {
                fileStorageService.DeleteFavorites(foldersId, filesId);
            });
        }

        public int CreateFolderInMy()
        {
            int id = 0;

            InitialScope(() =>
            {
                id = filesControllerHelper
                .CreateFolder(globalFolderHelper.FolderMy, "TestFolder").Id;
            });

            return id;
        }

        public int CreateCommonFolder()
        {
            int id = 0;

            InitialScope(() =>
            {
                id = filesControllerHelper
                .CreateFolder(globalFolderHelper.FolderCommon, "common").Id;
            });

            return id;
        }

        public int CreateFolder(int folderId)
        {
            int id = 0;

            InitialScope(() =>
            {
                id = filesControllerHelper.CreateFolder(folderId, "TestFolder").Id;
            });

            return id;
        }

        public void DeleteFolder(int folderId)
        {
            InitialScope(() =>
            {
                filesControllerHelper.DeleteFolder(folderId, false, true);
            });
        }

        public void GetFolder(int folderId)
        {
            InitialScope(() => 
            { 
                filesControllerHelper.GetFolder(folderId, Id, Core.FilterType.None, false);
            });
        }

        public void RenameFolder(int folderId, string newTitle)
        {
            InitialScope(() =>
            {
                filesControllerHelper.RenameFolder(folderId, newTitle);
            });
        }

        public void ShareFolder(int folderId, Guid userId)
        {
            InitialScope(() =>
            {
                filesControllerHelper.SetFolderSecurityInfo(folderId,
                    new List<FileShareParams>()
                    {
                        new FileShareParams() { Access = Core.Security.FileShare.ReadWrite, ShareTo = userId }
                    }, false, "share");
            });
        }

        private void InitialScope(Action action)
        {
            UpdateScope();

            action?.Invoke();

            DeleteScope();
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

        private void DeleteScope()
        {
            scope.Dispose();
            scope = null;
        }
    }
}
