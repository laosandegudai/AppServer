using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ASC.Api.Documents;

using NUnit.Framework;

namespace ASC.Files.Tests
{
    [TestFixture]
    class Favorites : BaseFilesTests
    {
        private FolderWrapper<int> TestFolder { get; set; }
        public FileWrapper<int> TestFile { get; private set; }

        public IEnumerable<int> folderIds;
        public IEnumerable<int> fileIds;

        [OneTimeSetUp]
        public async Task SetUpFavorites()
        {
            OneTimeSetUp();
            TestFolder = await FilesControllerHelper.CreateFolder(await GlobalFolderHelper.FolderMy, "TestFolder");
            TestFile = await FilesControllerHelper.CreateFile(await GlobalFolderHelper.FolderMy, "TestFile", default);
            folderIds = new List<int> { TestFolder.Id };
            fileIds = new List<int> { TestFile.Id };
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await DeleteFolder(TestFolder.Id);
            await DeleteFile(TestFile.Id);
        }

        [TestCaseSource(typeof(DocumentData), nameof(DocumentData.GetCreateFolderItems))]
        [Category("Folder")]
        [Order(1)]
        public void CreateFolderReturnsFolderWrapper(string folderTitle)
        {
            var folderWrapper = Assert.ThrowsAsync<InvalidOperationException>(async () => await FilesControllerHelper.CreateFolder(await GlobalFolderHelper.FolderFavorites, folderTitle));
            Assert.That(folderWrapper.Message == "You don't have enough permission to create");
        }

        [TestCaseSource(typeof(DocumentData), nameof(DocumentData.GetCreateFileItems))]
        [Category("File")]
        [Order(1)]
        public async Task CreateFileReturnsFolderWrapper(string fileTitle)
        {
            var fileWrapper = await FilesControllerHelper.CreateFile(await GlobalFolderHelper.FolderShare, fileTitle, default);
            Assert.AreEqual(fileWrapper.FolderId, await GlobalFolderHelper.FolderMy);
        }

        [Test]
        [Category("Favorite")]
        [Order(2)]
        public async Task GetFavoriteFolderToFolderWrapper()
        {
            var favorite = await FileStorageService.AddToFavorites(folderIds, fileIds);

            Assert.IsNotNull(favorite);
        }
        [Test]
        [Category("Favorite")]
        [Order(3)]
        public async Task DeleteFavoriteFolderToFolderWrapper()
        {
            var favorite = await FileStorageService.DeleteFavorites(folderIds, fileIds);
            Assert.IsNotNull(favorite);
        }
    }
}
