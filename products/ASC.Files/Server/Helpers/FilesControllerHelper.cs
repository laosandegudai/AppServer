using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using ASC.Api.Core;
using ASC.Api.Documents;
using ASC.Api.Utils;
using ASC.Common;
using ASC.Common.Logging;
using ASC.Common.Web;
using ASC.Core;
using ASC.Core.Common.Settings;
using ASC.FederatedLogin.Helpers;
using ASC.Files.Core;
using ASC.Files.Model;
using ASC.Web.Core.Files;
using ASC.Web.Files.Classes;
using ASC.Web.Files.Core.Entries;
using ASC.Web.Files.Services.DocumentService;
using ASC.Web.Files.Services.WCFService;
using ASC.Web.Files.Utils;
using ASC.Web.Studio.Core;

using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

using static ASC.Api.Documents.FilesController;

using FileShare = ASC.Files.Core.Security.FileShare;
using MimeMapping = ASC.Common.Web.MimeMapping;
using SortedByType = ASC.Files.Core.SortedByType;

namespace ASC.Files.Helpers
{
    [Scope]
    public class FilesControllerHelper<T>
    {
        private readonly ApiContext ApiContext;
        private readonly FileStorageService<T> FileStorageService;

        private FileWrapperHelper FileWrapperHelper { get; }
        private FilesSettingsHelper FilesSettingsHelper { get; }
        private FilesLinkUtility FilesLinkUtility { get; }
        private FileUploader FileUploader { get; }
        private DocumentServiceHelper DocumentServiceHelper { get; }
        private TenantManager TenantManager { get; }
        private SecurityContext SecurityContext { get; }
        private FolderWrapperHelper FolderWrapperHelper { get; }
        private FileOperationWraperHelper FileOperationWraperHelper { get; }
        private FileShareWrapperHelper FileShareWrapperHelper { get; }
        private FileShareParamsHelper FileShareParamsHelper { get; }
        private EntryManager EntryManager { get; }
        private FolderContentWrapperHelper FolderContentWrapperHelper { get; }
        private ChunkedUploadSessionHelper ChunkedUploadSessionHelper { get; }
        private DocumentServiceTrackerHelper DocumentServiceTracker { get; }
        private SettingsManager SettingsManager { get; }
        private EncryptionKeyPairHelper EncryptionKeyPairHelper { get; }
        private ILog Logger { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileStorageService"></param>
        public FilesControllerHelper(
            ApiContext context,
            FileStorageService<T> fileStorageService,
            FileWrapperHelper fileWrapperHelper,
            FilesSettingsHelper filesSettingsHelper,
            FilesLinkUtility filesLinkUtility,
            FileUploader fileUploader,
            DocumentServiceHelper documentServiceHelper,
            TenantManager tenantManager,
            SecurityContext securityContext,
            FolderWrapperHelper folderWrapperHelper,
            FileOperationWraperHelper fileOperationWraperHelper,
            FileShareWrapperHelper fileShareWrapperHelper,
            FileShareParamsHelper fileShareParamsHelper,
            EntryManager entryManager,
            FolderContentWrapperHelper folderContentWrapperHelper,
            ChunkedUploadSessionHelper chunkedUploadSessionHelper,
            DocumentServiceTrackerHelper documentServiceTracker,
            IOptionsMonitor<ILog> optionMonitor,
            SettingsManager settingsManager,
            EncryptionKeyPairHelper encryptionKeyPairHelper)
        {
            ApiContext = context;
            FileStorageService = fileStorageService;
            FileWrapperHelper = fileWrapperHelper;
            FilesSettingsHelper = filesSettingsHelper;
            FilesLinkUtility = filesLinkUtility;
            FileUploader = fileUploader;
            DocumentServiceHelper = documentServiceHelper;
            TenantManager = tenantManager;
            SecurityContext = securityContext;
            FolderWrapperHelper = folderWrapperHelper;
            FileOperationWraperHelper = fileOperationWraperHelper;
            FileShareWrapperHelper = fileShareWrapperHelper;
            FileShareParamsHelper = fileShareParamsHelper;
            EntryManager = entryManager;
            FolderContentWrapperHelper = folderContentWrapperHelper;
            ChunkedUploadSessionHelper = chunkedUploadSessionHelper;
            DocumentServiceTracker = documentServiceTracker;
            SettingsManager = settingsManager;
            EncryptionKeyPairHelper = encryptionKeyPairHelper;
            Logger = optionMonitor.Get("ASC.Files");
        }

        public async Task<FolderContentWrapper<T>> GetFolder(T folderId, Guid userIdOrGroupId, FilterType filterType, bool withSubFolders)
        {
            return await ToFolderContentWrapper(folderId, userIdOrGroupId, filterType, withSubFolders).NotFoundIfNull();
        }

        public async Task<List<FileWrapper<T>>> UploadFile(T folderId, UploadModel uploadModel)
        {
            if (uploadModel.StoreOriginalFileFlag.HasValue)
            {
                FilesSettingsHelper.StoreOriginalFiles = uploadModel.StoreOriginalFileFlag.Value;
            }

            if (uploadModel.Files != null && uploadModel.Files.Any())
            {
                if (uploadModel.Files.Count() == 1)
                {
                    //Only one file. return it
                    var postedFile = uploadModel.Files.First();
                    return new List<FileWrapper<T>>
                    {
                        await InsertFile(folderId, postedFile.OpenReadStream(), postedFile.FileName, uploadModel.CreateNewIfExist, uploadModel.KeepConvertStatus)
                    };
                }
                //For case with multiple files
                return (await Task.WhenAll(uploadModel.Files.Select(postedFile => InsertFile(folderId, postedFile.OpenReadStream(), postedFile.FileName, uploadModel.CreateNewIfExist, uploadModel.KeepConvertStatus)))).ToList();
            }
            if (uploadModel.File != null)
            {
                var fileName = "file" + MimeMapping.GetExtention(uploadModel.ContentType.MediaType);
                if (uploadModel.ContentDisposition != null)
                {
                    fileName = uploadModel.ContentDisposition.FileName;
                }

                return new List<FileWrapper<T>>
                {
                    await InsertFile(folderId, uploadModel.File, fileName, uploadModel.CreateNewIfExist, uploadModel.KeepConvertStatus)
                };
            }
            throw new InvalidOperationException("No input files");
        }

        public async Task<FileWrapper<T>> InsertFile(T folderId, Stream file, string title, bool? createNewIfExist, bool keepConvertStatus = false)
        {
            try
            {
                var resultFile = await FileUploader.Exec(folderId, title, file.Length, file, createNewIfExist ?? !FilesSettingsHelper.UpdateIfExist, !keepConvertStatus);
                return await FileWrapperHelper.Get(resultFile);
            }
            catch (FileNotFoundException e)
            {
                throw new ItemNotFoundException("File not found", e);
            }
            catch (DirectoryNotFoundException e)
            {
                throw new ItemNotFoundException("Folder not found", e);
            }
        }

        public async Task<FileWrapper<T>> UpdateFileStream(Stream file, T fileId, bool encrypted = false, bool forcesave = false)
        {
            try
            {
                var resultFile = await FileStorageService.UpdateFileStream(fileId, file, encrypted, forcesave);
                return await FileWrapperHelper.Get(resultFile);
            }
            catch (FileNotFoundException e)
            {
                throw new ItemNotFoundException("File not found", e);
            }
        }

        public async Task<FileWrapper<T>> SaveEditing(T fileId, string fileExtension, string downloadUri, Stream stream, string doc, bool forcesave)
        {
            return await FileWrapperHelper.Get(await FileStorageService.SaveEditing(fileId, fileExtension, downloadUri, stream, doc, forcesave));
        }

        public async Task<string> StartEdit(T fileId, bool editingAlone, string doc)
        {
            return await FileStorageService.StartEdit(fileId, editingAlone, doc);
        }

        public async Task<KeyValuePair<bool, string>> TrackEditFile(T fileId, Guid tabId, string docKeyForTrack, string doc, bool isFinish)
        {
            return await FileStorageService.TrackEditFile(fileId, tabId, docKeyForTrack, doc, isFinish);
        }

        public async Task<Configuration<T>> OpenEdit(T fileId, int version, string doc)
        {
            var (_, configuration) = await DocumentServiceHelper.GetParams(fileId, version, doc, true, true, true);
            configuration.EditorType = EditorType.External;
            configuration.EditorConfig.CallbackUrl = DocumentServiceTracker.GetCallbackUrl(configuration.Document.Info.File.ID.ToString());

            if (configuration.Document.Info.File.RootFolderType == FolderType.Privacy && PrivacyRoomSettings.GetEnabled(SettingsManager))
            {
                var keyPair = EncryptionKeyPairHelper.GetKeyPair();
                if (keyPair != null)
                {
                    configuration.EditorConfig.EncryptionKeys = new EncryptionKeysConfig
                    {
                        PrivateKeyEnc = keyPair.PrivateKeyEnc,
                        PublicKey = keyPair.PublicKey,
                    };
                }
            }


            configuration.Token = DocumentServiceHelper.GetSignature(configuration);
            return configuration;
        }

        public async Task<object> CreateUploadSession(T folderId, string fileName, long fileSize, string relativePath, bool encrypted)
        {
            var file = await FileUploader.VerifyChunkedUpload(folderId, fileName, fileSize, FilesSettingsHelper.UpdateIfExist, relativePath);

            if (FilesLinkUtility.IsLocalFileUploader)
            {
                var session = await FileUploader.InitiateUpload(file.FolderID, (file.ID ?? default), file.Title, file.ContentLength, encrypted);

                var responseObject = await ChunkedUploadSessionHelper.ToResponseObject(session, true);
                return new
                {
                    success = true,
                    data = responseObject
                };
            }

            var createSessionUrl = FilesLinkUtility.GetInitiateUploadSessionUrl(TenantManager.GetCurrentTenant().TenantId, file.FolderID, file.ID, file.Title, file.ContentLength, encrypted, SecurityContext);
            var request = (HttpWebRequest)WebRequest.Create(createSessionUrl);
            request.Method = "POST";
            request.ContentLength = 0;

            // hack for uploader.onlyoffice.com in api requests
            var rewriterHeader = ApiContext.HttpContextAccessor.HttpContext.Request.Headers[HttpRequestExtensions.UrlRewriterHeader];
            if (!string.IsNullOrEmpty(rewriterHeader))
            {
                request.Headers[HttpRequestExtensions.UrlRewriterHeader] = rewriterHeader;
            }

            // hack. http://ubuntuforums.org/showthread.php?t=1841740
            if (WorkContext.IsMono)
            {
                ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true;
            }

            using var response = request.GetResponse();
            using var responseStream = response.GetResponseStream();
            using var streamReader = new StreamReader(responseStream);
            return JObject.Parse(streamReader.ReadToEnd()); //result is json string
        }

        public async Task<FileWrapper<T>> CreateTextFile(T folderId, string title, string content)
        {
            if (title == null) throw new ArgumentNullException("title");
            //Try detect content
            var extension = ".txt";
            if (!string.IsNullOrEmpty(content))
            {
                if (Regex.IsMatch(content, @"<([^\s>]*)(\s[^<]*)>"))
                {
                    extension = ".html";
                }
            }
            return await CreateFile(folderId, title, content, extension);
        }

        private async Task<FileWrapper<T>> CreateFile(T folderId, string title, string content, string extension)
        {
            using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var file = await FileUploader.Exec(folderId,
                              title.EndsWith(extension, StringComparison.OrdinalIgnoreCase) ? title : (title + extension),
                              memStream.Length, memStream);
            return await FileWrapperHelper.Get(file);
        }

        public async Task<FileWrapper<T>> CreateHtmlFile(T folderId, string title, string content)
        {
            if (title == null) throw new ArgumentNullException("title");
            return await CreateFile(folderId, title, content, ".html");
        }

        public async Task<FolderWrapper<T>> CreateFolder(T folderId, string title)
        {
            var folder = await FileStorageService.CreateNewFolder(folderId, title);
            return await FolderWrapperHelper.Get(folder);
        }

        public async Task<FileWrapper<T>> CreateFile(T folderId, string title, T templateId)
        {
            var file = await FileStorageService.CreateNewFile(new FileModel<T> { ParentId = folderId, Title = title, TemplateId = templateId });
            return await FileWrapperHelper.Get(file);
        }

        public async Task<FolderWrapper<T>> RenameFolder(T folderId, string title)
        {
            var folder = await FileStorageService.FolderRename(folderId, title);
            return await FolderWrapperHelper.Get(folder);
        }

        public async Task<FolderWrapper<T>> GetFolderInfo(T folderId)
        {
            var folder = await FileStorageService.GetFolder(folderId).NotFoundIfNull("Folder not found");

            return await FolderWrapperHelper.Get(folder);
        }

        public async Task<IEnumerable<FileEntryWrapper>> GetFolderPath(T folderId)
        {
            var result = new List<FileEntryWrapper>();
            var breadCrumbs = await EntryManager.GetBreadCrumbs(folderId);

            foreach (var b in breadCrumbs)
            {
                result.Add(await GetFileEntryWrapper(b));
            }

            return result;
        }

        public async Task<FileWrapper<T>> GetFileInfo(T fileId, int version = -1)
        {
            var file = await FileStorageService.GetFile(fileId, version).NotFoundIfNull("File not found");
            return await FileWrapperHelper.Get(file);
        }

        public async Task<FileEntryWrapper> AddToRecent(T fileId, int version = -1)
        {
            var file = await FileStorageService.GetFile(fileId, version).NotFoundIfNull("File not found");
            EntryManager.MarkAsRecent(file);
            return await FileWrapperHelper.Get(file);
        }

        public async Task<List<FileEntryWrapper>> GetNewItems(T folderId)
        {
            return (await Task.WhenAll(
                (await FileStorageService.GetNewItems(folderId))
                .Select(GetFileEntryWrapper)))
                .ToList();
        }

        public async Task<FileWrapper<T>> UpdateFile(T fileId, string title, int lastVersion)
        {
            if (!string.IsNullOrEmpty(title))
                await FileStorageService.FileRename(fileId, title);

            if (lastVersion > 0)
                await FileStorageService.UpdateToVersion(fileId, lastVersion);

            return await GetFileInfo(fileId);
        }

        public async Task<IEnumerable<FileOperationWraper>> DeleteFile(T fileId, bool deleteAfter, bool immediately)
        {
            return await Task.WhenAll(FileStorageService.DeleteFile("delete", fileId, false, deleteAfter, immediately)
                .Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<ConversationResult<T>>> StartConversion(T fileId)
        {
            return await CheckConversion(fileId, true);
        }

        public async Task<IEnumerable<ConversationResult<T>>> CheckConversion(T fileId, bool start)
        {
            var data = (await FileStorageService.CheckConversion(new ItemList<ItemList<string>>
            {
                new ItemList<string> { fileId.ToString(), "0", start.ToString() }
            }));

            return await Task.WhenAll(data
            .Select(async r =>
            {
                var o = new ConversationResult<T>
                {
                    Id = r.Id,
                    Error = r.Error,
                    OperationType = r.OperationType,
                    Processed = r.Processed,
                    Progress = r.Progress,
                    Source = r.Source,
                };
                if (!string.IsNullOrEmpty(r.Result))
                {
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            AllowTrailingCommas = true,
                            PropertyNameCaseInsensitive = true
                        };
                        var jResult = JsonSerializer.Deserialize<FileJsonSerializerData<T>>(r.Result, options);
                        o.File = await GetFileInfo(jResult.Id, jResult.Version);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e);
                    }
                }
                return o;
            }));
        }

        public async Task<IEnumerable<FileOperationWraper>> DeleteFolder(T folderId, bool deleteAfter, bool immediately)
        {
            return await Task.WhenAll(FileStorageService.DeleteFolder("delete", folderId, false, deleteAfter, immediately)
                    .Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<FileEntryWrapper>> MoveOrCopyBatchCheck(BatchModel batchModel)
        {
            var (checkedFiles, checkedFolders) = await FileStorageService.MoveOrCopyFilesCheck(batchModel.FileIds, batchModel.FolderIds, batchModel.DestFolderId);

            var entries = await FileStorageService.GetItems(checkedFiles.OfType<int>().Select(Convert.ToInt32), checkedFiles.OfType<int>().Select(Convert.ToInt32), FilterType.FilesOnly, false, "", "");

            entries.AddRange(await FileStorageService.GetItems(checkedFiles.OfType<string>(), checkedFiles.OfType<string>(), FilterType.FilesOnly, false, "", ""));

            return await Task.WhenAll(entries.Select(GetFileEntryWrapper));
        }

        public async Task<IEnumerable<FileOperationWraper>> MoveBatchItems(BatchModel batchModel)
        {
            return await Task.WhenAll(FileStorageService.MoveOrCopyItems(batchModel.FolderIds, batchModel.FileIds, batchModel.DestFolderId, batchModel.ConflictResolveType, false, batchModel.DeleteAfter)
                .Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<FileOperationWraper>> CopyBatchItems(BatchModel batchModel)
        {
            return await Task.WhenAll(FileStorageService.MoveOrCopyItems(batchModel.FolderIds, batchModel.FileIds, batchModel.DestFolderId, batchModel.ConflictResolveType, true, batchModel.DeleteAfter)
                .Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<FileOperationWraper>> MarkAsRead(BaseBatchModel<JsonElement> model)
        {
            return await Task.WhenAll(FileStorageService.MarkAsRead(model.FolderIds, model.FileIds).Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<FileOperationWraper>> TerminateTasks()
        {
            return await Task.WhenAll(FileStorageService.TerminateTasks().Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<FileOperationWraper>> GetOperationStatuses()
        {
            return await Task.WhenAll(FileStorageService.GetTasksStatuses().Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<FileOperationWraper>> BulkDownload(DownloadModel model)
        {
            var folders = new Dictionary<JsonElement, string>();
            var files = new Dictionary<JsonElement, string>();

            foreach (var fileId in model.FileConvertIds.Where(fileId => !files.ContainsKey(fileId.Key)))
            {
                files.Add(fileId.Key, fileId.Value);
            }

            foreach (var fileId in model.FileIds.Where(fileId => !files.ContainsKey(fileId)))
            {
                files.Add(fileId, string.Empty);
            }

            foreach (var folderId in model.FolderIds.Where(folderId => !folders.ContainsKey(folderId)))
            {
                folders.Add(folderId, string.Empty);
            }

            return await Task.WhenAll(FileStorageService.BulkDownload(folders, files).Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<FileOperationWraper>> EmptyTrash()
        {
            return await Task.WhenAll((await FileStorageService.EmptyTrash()).Select(FileOperationWraperHelper.Get));
        }

        public async Task<IEnumerable<FileWrapper<T>>> GetFileVersionInfo(T fileId)
        {
            var files = await FileStorageService.GetFileHistory(fileId);
            return await Task.WhenAll(files.Select(r => FileWrapperHelper.Get(r)));
        }

        public async Task<IEnumerable<FileWrapper<T>>> ChangeHistory(T fileId, int version, bool continueVersion)
        {
            var history = (await FileStorageService.CompleteVersion(fileId, version, continueVersion)).Value;
            return await Task.WhenAll(history.Select(r => FileWrapperHelper.Get(r)));
        }

        public async Task<FileWrapper<T>> LockFile(T fileId, bool lockFile)
        {
            var result = await FileStorageService.LockFile(fileId, lockFile);
            return await FileWrapperHelper.Get(result);
        }

        public async Task<string> UpdateComment(T fileId, int version, string comment)
        {
            return await FileStorageService.UpdateComment(fileId, version, comment);
        }

        public async Task<IEnumerable<FileShareWrapper>> GetFileSecurityInfo(T fileId)
        {
            return await GetSecurityInfo(new List<T> { fileId }, new List<T> { });
        }

        public async Task<IEnumerable<FileShareWrapper>> GetFolderSecurityInfo(T folderId)
        {
            return await GetSecurityInfo(new List<T> { }, new List<T> { folderId });
        }

        public async Task<IEnumerable<FileShareWrapper>> GetSecurityInfo(IEnumerable<T> fileIds, IEnumerable<T> folderIds)
        {
            var fileShares = await FileStorageService.GetSharedInfo(fileIds, folderIds);
            return fileShares.Select(FileShareWrapperHelper.Get);
        }

        public async Task<IEnumerable<FileShareWrapper>> SetFileSecurityInfo(T fileId, IEnumerable<FileShareParams> share, bool notify, string sharingMessage)
        {
            return await SetSecurityInfo(new List<T> { fileId }, new List<T>(), share, notify, sharingMessage);
        }

        public async Task<IEnumerable<FileShareWrapper>> SetFolderSecurityInfo(T folderId, IEnumerable<FileShareParams> share, bool notify, string sharingMessage)
        {
            return await SetSecurityInfo(new List<T>(), new List<T> { folderId }, share, notify, sharingMessage);
        }

        public async Task<IEnumerable<FileShareWrapper>> SetSecurityInfo(IEnumerable<T> fileIds, IEnumerable<T> folderIds, IEnumerable<FileShareParams> share, bool notify, string sharingMessage)
        {
            if (share != null && share.Any())
            {
                var list = new ItemList<AceWrapper>(share.Select(FileShareParamsHelper.ToAceObject));
                var aceCollection = new AceCollection<T>
                {
                    Files = fileIds,
                    Folders = folderIds,
                    Aces = list,
                    Message = sharingMessage
                };
                await FileStorageService.SetAceObject(aceCollection, notify);
            }

            return await GetSecurityInfo(fileIds, folderIds);
        }

        public async Task<bool> RemoveSecurityInfo(List<T> fileIds, List<T> folderIds)
        {
            await FileStorageService.RemoveAce(fileIds, folderIds);

            return true;
        }

        public async Task<string> GenerateSharedLink(T fileId, FileShare share)
        {
            var file = GetFileInfo(fileId);

            var sharedInfo = (await FileStorageService.GetSharedInfo(new List<T> { fileId }, new List<T> { })).Find(r => r.SubjectId == FileConstant.ShareLinkId);
            if (sharedInfo == null || sharedInfo.Share != share)
            {
                var list = new ItemList<AceWrapper>
                    {
                        new AceWrapper
                            {
                                SubjectId = FileConstant.ShareLinkId,
                                SubjectGroup = true,
                                Share = share
                            }
                    };
                var aceCollection = new AceCollection<T>
                {
                    Files = new List<T> { fileId },
                    Aces = list
                };
                await FileStorageService.SetAceObject(aceCollection, false);
                sharedInfo = (await FileStorageService.GetSharedInfo(new List<T> { fileId }, new List<T> { })).Find(r => r.SubjectId == FileConstant.ShareLinkId);
            }

            return sharedInfo.Link;
        }

        public async Task<bool> SetAceLink(T fileId, FileShare share)
        {
            return await FileStorageService.SetAceLink(fileId, share);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //[Read(@"@search/{query}")]
        //public IEnumerable<FileEntryWrapper> Search(string query)
        //{
        //    var searcher = new SearchHandler();
        //    var files = searcher.SearchFiles(query).Select(r => (FileEntryWrapper)FileWrapperHelper.Get(r));
        //    var folders = searcher.SearchFolders(query).Select(f => (FileEntryWrapper)FolderWrapperHelper.Get(f));

        //    return files.Concat(folders);
        //}


        private async Task<FolderContentWrapper<T>> ToFolderContentWrapper(T folderId, Guid userIdOrGroupId, FilterType filterType, bool withSubFolders)
        {
            if (!Enum.TryParse(ApiContext.SortBy, true, out SortedByType sortBy))
            {
                sortBy = SortedByType.AZ;
            }

            var startIndex = Convert.ToInt32(ApiContext.StartIndex);
            return await FolderContentWrapperHelper.Get(await FileStorageService.GetFolderItems(folderId,
                                                                               startIndex,
                                                                               Convert.ToInt32(ApiContext.Count),
                                                                               filterType,
                                                                               filterType == FilterType.ByUser,
                                                                               userIdOrGroupId.ToString(),
                                                                               ApiContext.FilterValue,
                                                                               false,
                                                                               withSubFolders,
                                                                               new OrderBy(sortBy, !ApiContext.SortDescending)),
                                            startIndex);
        }

        internal async Task<FileEntryWrapper> GetFileEntryWrapper(FileEntry r)
        {
            FileEntryWrapper wrapper = null;
            if (r is Folder<int> fol1)
            {
                wrapper = await FolderWrapperHelper.Get(fol1);
            }
            else if (r is Folder<string> fol2)
            {
                wrapper = await FolderWrapperHelper.Get(fol2);
            }
            else if (r is File<int> file1)
            {
                wrapper = await FileWrapperHelper.Get(file1);
            }
            else if (r is File<string> file2)
            {
                wrapper = await FileWrapperHelper.Get(file2);
            }

            return wrapper;
        }
    }
}
