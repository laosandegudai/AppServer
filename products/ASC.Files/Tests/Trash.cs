using System;
using System.Threading;
using System.Threading.Tasks;

using ASC.Api.Documents;
using ASC.Web.Files.Services.WCFService;
using ASC.Web.Files.Services.WCFService.FileOperations;

using NUnit.Framework;

namespace ASC.Files.Tests
{
    [TestFixture]
    class Trash :BaseFilesTests
    {
        private FolderWrapper<int> TestFolder { get; set; }
        public FileWrapper<int> TestFile { get; private set; }

        [OneTimeSetUp]
        public async Task SetUpTrash()
        {
            TestFolder = await FilesControllerHelper.CreateFolder(await GlobalFolderHelper.FolderMy, "TestFolder");
            TestFile = await FilesControllerHelper.CreateFile(await GlobalFolderHelper.FolderMy, "TestFile", default);

        }
        [OneTimeTearDown]
        public async Task TearDown()
        {
            await DeleteFile(TestFile.Id);
            await DeleteFolder(TestFolder.Id);
        }

        [TestCaseSource(typeof(DocumentData), nameof(DocumentData.GetCreateFolderItems))]
        [Category("Folder")]
        [Order(1)]
        public void CreateFolderReturnsFolderWrapper(string folderTitle)
        {
            var folderWrapper = Assert.ThrowsAsync<InvalidOperationException>(async () => await FilesControllerHelper.CreateFolder((int)GlobalFolderHelper.FolderTrash, folderTitle));
            Assert.That(folderWrapper.Message == "You don't have enough permission to create");
        }
        [TestCaseSource(typeof(DocumentData), nameof(DocumentData.GetCreateFileItems))]
        [Category("File")]
        [Order(2)]
        public async Task CreateFileReturnsFolderWrapper(string fileTitle)
        {
            var fileWrapper = await FilesControllerHelper.CreateFile((int)GlobalFolderHelper.FolderTrash, fileTitle, default);
            Assert.AreEqual(fileWrapper.FolderId, await GlobalFolderHelper.FolderMy);
        }

        [Test]
        [Category("Folder")]
        [Order(2)]
        public void DeleteFileFromTrash()
        {
            var Empty = FilesControllerHelper.EmptyTrash();
            
            ItemList<FileOperationResult> statuses;

            while (true)
            {
                statuses = FileStorageService.GetTasksStatuses();

                if (statuses.TrueForAll(r => r.Finished))
                    break;
                Thread.Sleep(100);
            }
            Assert.IsTrue(statuses.TrueForAll(r => string.IsNullOrEmpty(r.Error)));
        }
    }
}
