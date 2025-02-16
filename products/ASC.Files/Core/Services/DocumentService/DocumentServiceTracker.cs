/*
 *
 * (c) Copyright Ascensio System Limited 2010-2018
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Web;

using ASC.Common;
using ASC.Common.Logging;
using ASC.Core;
using ASC.Core.Common;
using ASC.Core.Users;
using ASC.Files.Core;
using ASC.Files.Core.Resources;
using ASC.Files.Core.Services.NotifyService;
using ASC.MessagingSystem;
using ASC.Security.Cryptography;
using ASC.Web.Core.Files;
using ASC.Web.Core.Users;
using ASC.Web.Files.Classes;
using ASC.Web.Files.Core;
using ASC.Web.Files.Helpers;
using ASC.Web.Files.ThirdPartyApp;
using ASC.Web.Files.Utils;

using Microsoft.Extensions.Options;

using static ASC.Web.Files.Services.DocumentService.DocumentServiceTracker;

using CommandMethod = ASC.Web.Core.Files.DocumentService.CommandMethod;

namespace ASC.Web.Files.Services.DocumentService
{
    public class DocumentServiceTracker
    {
        #region Class

        public enum TrackerStatus
        {
            NotFound = 0,
            Editing = 1,
            MustSave = 2,
            Corrupted = 3,
            Closed = 4,
            MailMerge = 5,
            ForceSave = 6,
            CorruptedForceSave = 7,
        }

        [DebuggerDisplay("{Status} - {Key}")]
        public class TrackerData
        {
            public List<Action> Actions { get; set; }
            public string ChangesUrl { get; set; }
            public ForceSaveInitiator ForceSaveType { get; set; }
            public object History { get; set; }
            public string Key { get; set; }
            public MailMergeData MailMerge { get; set; }
            public TrackerStatus Status { get; set; }
            public string Token { get; set; }
            public string Url { get; set; }
            public List<string> Users { get; set; }
            public string UserData { get; set; }
            public bool Encrypted { get; set; }

            [DebuggerDisplay("{Type} - {UserId}")]
            public class Action
            {
                public string Type;
                public string UserId;
            }

            public enum ForceSaveInitiator
            {
                Command = 0,
                User = 1,
                Timer = 2
            }
        }

        public enum MailMergeType
        {
            Html = 0,
            AttachDocx = 1,
            AttachPdf = 2,
        }

        [DebuggerDisplay("{From}")]
        public class MailMergeData
        {
            public int RecordCount { get; set; }
            public int RecordErrorCount { get; set; }
            public int RecordIndex { get; set; }

            public string From { get; set; }
            public string Subject { get; set; }
            public string To { get; set; }
            public MailMergeType Type { get; set; }

            public string Title { get; set; } //attach
            public string Message { get; set; } //attach
        }

        [Serializable]
        public class TrackResponse
        {
            public int Error
            {
                set { }
                get
                {
                    return string.IsNullOrEmpty(Message)
                               ? 0 //error:0 - sended
                               : 1; //error:1 - some error
                }
            }

            public string Message { get; set; }

            public static string Serialize(TrackResponse response)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                return JsonSerializer.Serialize(response, options);
            }
        }

        #endregion
    }

    [Scope]
    public class DocumentServiceTrackerHelper
    {
        private SecurityContext SecurityContext { get; }
        private UserManager UserManager { get; }
        private TenantManager TenantManager { get; }
        private FilesLinkUtility FilesLinkUtility { get; }
        private EmailValidationKeyProvider EmailValidationKeyProvider { get; }
        private BaseCommonLinkUtility BaseCommonLinkUtility { get; }
        private SocketManager SocketManager { get; }
        private GlobalStore GlobalStore { get; }
        private DisplayUserSettingsHelper DisplayUserSettingsHelper { get; }
        private IDaoFactory DaoFactory { get; }
        private DocumentServiceHelper DocumentServiceHelper { get; }
        private EntryManager EntryManager { get; }
        private FileShareLink FileShareLink { get; }
        private FilesMessageService FilesMessageService { get; }
        private DocumentServiceConnector DocumentServiceConnector { get; }
        private NotifyClient NotifyClient { get; }
        private MailMergeTaskRunner MailMergeTaskRunner { get; }
        private FileTrackerHelper FileTracker { get; }
        public ILog Logger { get; }

        public DocumentServiceTrackerHelper(
            SecurityContext securityContext,
            UserManager userManager,
            TenantManager tenantManager,
            FilesLinkUtility filesLinkUtility,
            EmailValidationKeyProvider emailValidationKeyProvider,
            BaseCommonLinkUtility baseCommonLinkUtility,
            SocketManager socketManager,
            GlobalStore globalStore,
            DisplayUserSettingsHelper displayUserSettingsHelper,
            IDaoFactory daoFactory,
            IOptionsMonitor<ILog> options,
            DocumentServiceHelper documentServiceHelper,
            EntryManager entryManager,
            FileShareLink fileShareLink,
            FilesMessageService filesMessageService,
            DocumentServiceConnector documentServiceConnector,
            NotifyClient notifyClient,
            MailMergeTaskRunner mailMergeTaskRunner,
            FileTrackerHelper fileTracker)
        {
            SecurityContext = securityContext;
            UserManager = userManager;
            TenantManager = tenantManager;
            FilesLinkUtility = filesLinkUtility;
            EmailValidationKeyProvider = emailValidationKeyProvider;
            BaseCommonLinkUtility = baseCommonLinkUtility;
            SocketManager = socketManager;
            GlobalStore = globalStore;
            DisplayUserSettingsHelper = displayUserSettingsHelper;
            DaoFactory = daoFactory;
            DocumentServiceHelper = documentServiceHelper;
            EntryManager = entryManager;
            FileShareLink = fileShareLink;
            FilesMessageService = filesMessageService;
            DocumentServiceConnector = documentServiceConnector;
            NotifyClient = notifyClient;
            MailMergeTaskRunner = mailMergeTaskRunner;
            FileTracker = fileTracker;
            Logger = options.CurrentValue;
        }

        public string GetCallbackUrl<T>(T fileId)
        {
            var callbackUrl = BaseCommonLinkUtility.GetFullAbsolutePath(FilesLinkUtility.FileHandlerPath
                                                                    + "?" + FilesLinkUtility.Action + "=track"
                                                                    + "&" + FilesLinkUtility.FileId + "=" + HttpUtility.UrlEncode(fileId.ToString())
                                                                    + "&" + FilesLinkUtility.AuthKey + "=" + EmailValidationKeyProvider.GetEmailKey(fileId.ToString()));
            callbackUrl = DocumentServiceConnector.ReplaceCommunityAdress(callbackUrl);
            return callbackUrl;
        }

        public bool StartTrack<T>(T fileId, string docKeyForTrack)
        {
            var callbackUrl = GetCallbackUrl(fileId);
            return DocumentServiceConnector.Command(CommandMethod.Info, docKeyForTrack, fileId, callbackUrl);
        }

        public TrackResponse ProcessData<T>(T fileId, TrackerData fileData)
        {
            switch (fileData.Status)
            {
                case TrackerStatus.NotFound:
                case TrackerStatus.Closed:
                    FileTracker.Remove(fileId);
                    SocketManager.FilesChangeEditors(fileId, true);
                    break;

                case TrackerStatus.Editing:
                    ProcessEdit(fileId, fileData);
                    break;

                case TrackerStatus.MustSave:
                case TrackerStatus.Corrupted:
                case TrackerStatus.ForceSave:
                case TrackerStatus.CorruptedForceSave:
                    return ProcessSave(fileId, fileData);

                case TrackerStatus.MailMerge:
                    return ProcessMailMerge(fileId, fileData);
            }
            return null;
        }

        private void ProcessEdit<T>(T fileId, TrackerData fileData)
        {
            if (ThirdPartySelector.GetAppByFileId(fileId.ToString()) != null)
            {
                return;
            }

            var users = FileTracker.GetEditingBy(fileId);
            var usersDrop = new List<string>();

            string docKey;
            var app = ThirdPartySelector.GetAppByFileId(fileId.ToString());
            if (app == null)
            {
                File<T> fileStable;
                fileStable = DaoFactory.GetFileDao<T>().GetFileStable(fileId);

                docKey = DocumentServiceHelper.GetDocKey(fileStable);
            }
            else
            {
                docKey = fileData.Key;
            }

            if (!fileData.Key.Equals(docKey))
            {
                Logger.InfoFormat("DocService editing file {0} ({1}) with key {2} for {3}", fileId, docKey, fileData.Key, string.Join(", ", fileData.Users));
                usersDrop = fileData.Users;
            }
            else
            {
                foreach (var user in fileData.Users)
                {
                    if (!Guid.TryParse(user, out var userId))
                    {
                        Logger.Info("DocService userId is not Guid: " + user);
                        continue;
                    }
                    users.Remove(userId);

                    try
                    {
                        var doc = FileShareLink.CreateKey(fileId);
                        EntryManager.TrackEditing(fileId, userId, userId, doc);
                    }
                    catch (Exception e)
                    {
                        Logger.DebugFormat("Drop command: fileId '{0}' docKey '{1}' for user {2} : {3}", fileId, fileData.Key, user, e.Message);
                        usersDrop.Add(userId.ToString());
                    }
                }
            }

            if (usersDrop.Any())
            {
                if (!DocumentServiceHelper.DropUser(fileData.Key, usersDrop.ToArray(), fileId))
                {
                    Logger.Error("DocService drop failed for users " + string.Join(",", usersDrop));
                }
            }

            foreach (var removeUserId in users)
            {
                FileTracker.Remove(fileId, userId: removeUserId);
            }
            SocketManager.FilesChangeEditors(fileId);
        }

        private TrackResponse ProcessSave<T>(T fileId, TrackerData fileData)
        {
            var comments = new List<string>();
            if (fileData.Status == TrackerStatus.Corrupted
                || fileData.Status == TrackerStatus.CorruptedForceSave)
                comments.Add(FilesCommonResource.ErrorMassage_SaveCorrupted);

            var forcesave = fileData.Status == TrackerStatus.ForceSave || fileData.Status == TrackerStatus.CorruptedForceSave;

            if (fileData.Users == null || fileData.Users.Count == 0 || !Guid.TryParse(fileData.Users[0], out var userId))
            {
                userId = Guid.Empty;
            }

            var app = ThirdPartySelector.GetAppByFileId(fileId.ToString());
            if (app == null)
            {
                File<T> fileStable;
                fileStable = DaoFactory.GetFileDao<T>().GetFileStable(fileId);

                var docKey = DocumentServiceHelper.GetDocKey(fileStable);
                if (!fileData.Key.Equals(docKey))
                {
                    Logger.ErrorFormat("DocService saving file {0} ({1}) with key {2}", fileId, docKey, fileData.Key);

                    StoringFileAfterError(fileId, userId.ToString(), DocumentServiceConnector.ReplaceDocumentAdress(fileData.Url));
                    return new TrackResponse { Message = "Expected key " + docKey };
                }
            }

            UserInfo user = null;
            try
            {
                SecurityContext.AuthenticateMeWithoutCookie(userId);

                user = UserManager.GetUsers(userId);
                var culture = string.IsNullOrEmpty(user.CultureName) ? TenantManager.GetCurrentTenant().GetCulture() : CultureInfo.GetCultureInfo(user.CultureName);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch (Exception ex)
            {
                Logger.Info("DocService save error: anonymous author - " + userId, ex);
                if (!userId.Equals(ASC.Core.Configuration.Constants.Guest.ID))
                {
                    comments.Add(FilesCommonResource.ErrorMassage_SaveAnonymous);
                }
            }

            File<T> file = null;
            var saveMessage = "Not saved";

            if (string.IsNullOrEmpty(fileData.Url))
            {
                try
                {
                    comments.Add(FilesCommonResource.ErrorMassage_SaveUrlLost);

                    file = EntryManager.CompleteVersionFile(fileId, 0, false, false);

                    DaoFactory.GetFileDao<T>().UpdateComment(file.ID, file.Version, string.Join("; ", comments));

                    file = null;
                    Logger.ErrorFormat("DocService save error. Empty url. File id: '{0}'. UserId: {1}. DocKey '{2}'", fileId, userId, fileData.Key);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("DocService save error. Version update. File id: '{0}'. UserId: {1}. DocKey '{2}'", fileId, userId, fileData.Key), ex);
                }
            }
            else
            {
                if (fileData.Encrypted)
                {
                    comments.Add(FilesCommonResource.CommentEditEncrypt);
                }

                var forcesaveType = ForcesaveType.None;
                if (forcesave)
                {
                    switch (fileData.ForceSaveType)
                    {
                        case TrackerData.ForceSaveInitiator.Command:
                            forcesaveType = ForcesaveType.Command;
                            break;
                        case TrackerData.ForceSaveInitiator.Timer:
                            forcesaveType = ForcesaveType.Timer;
                            break;
                        case TrackerData.ForceSaveInitiator.User:
                            forcesaveType = ForcesaveType.User;
                            break;
                    }
                    comments.Add(fileData.ForceSaveType == TrackerData.ForceSaveInitiator.User
                                     ? FilesCommonResource.CommentForcesave
                                     : FilesCommonResource.CommentAutosave);
                }

                try
                {
                    file = EntryManager.SaveEditing(fileId, null, DocumentServiceConnector.ReplaceDocumentAdress(fileData.Url), null, string.Empty, string.Join("; ", comments), false, fileData.Encrypted, forcesaveType);
                    saveMessage = fileData.Status == TrackerStatus.MustSave || fileData.Status == TrackerStatus.ForceSave ? null : "Status " + fileData.Status;
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("DocService save error. File id: '{0}'. UserId: {1}. DocKey '{2}'. DownloadUri: {3}", fileId, userId, fileData.Key, fileData.Url), ex);
                    saveMessage = ex.Message;

                    StoringFileAfterError(fileId, userId.ToString(), DocumentServiceConnector.ReplaceDocumentAdress(fileData.Url));
                }
            }

            if (!forcesave)
                FileTracker.Remove(fileId);

            if (file != null)
            {
                if (user != null)
                    FilesMessageService.Send(file, MessageInitiator.DocsService, MessageAction.UserFileUpdated, user.DisplayUserName(false, DisplayUserSettingsHelper), file.Title);

                if (!forcesave)
                    SaveHistory(file, (fileData.History ?? "").ToString(), DocumentServiceConnector.ReplaceDocumentAdress(fileData.ChangesUrl));
            }

            SocketManager.FilesChangeEditors(fileId, !forcesave);

            var result = new TrackResponse { Message = saveMessage };
            return result;
        }

        private TrackResponse ProcessMailMerge<T>(T fileId, TrackerData fileData)
        {
            if (fileData.Users == null || fileData.Users.Count == 0 || !Guid.TryParse(fileData.Users[0], out var userId))
            {
                userId = FileTracker.GetEditingBy(fileId).FirstOrDefault();
            }

            string saveMessage;

            try
            {
                SecurityContext.AuthenticateMeWithoutCookie(userId);

                var user = UserManager.GetUsers(userId);
                var culture = string.IsNullOrEmpty(user.CultureName) ? TenantManager.GetCurrentTenant().GetCulture() : CultureInfo.GetCultureInfo(user.CultureName);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                if (string.IsNullOrEmpty(fileData.Url)) throw new ArgumentException("emptry url");

                if (fileData.MailMerge == null) throw new ArgumentException("MailMerge is null");

                var message = fileData.MailMerge.Message;
                Stream attach = null;
                switch (fileData.MailMerge.Type)
                {
                    case MailMergeType.AttachDocx:
                    case MailMergeType.AttachPdf:
                        var downloadRequest = (HttpWebRequest)WebRequest.Create(DocumentServiceConnector.ReplaceDocumentAdress(fileData.Url));

                        // hack. http://ubuntuforums.org/showthread.php?t=1841740
                        if (WorkContext.IsMono)
                        {
                            ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true;
                        }

                        using (var downloadStream = new ResponseStream(downloadRequest.GetResponse()))
                        {
                            const int bufferSize = 2048;
                            var buffer = new byte[bufferSize];
                            int readed;
                            attach = new MemoryStream();
                            while ((readed = downloadStream.Read(buffer, 0, bufferSize)) > 0)
                            {
                                attach.Write(buffer, 0, readed);
                            }
                            attach.Position = 0;
                        }

                        if (string.IsNullOrEmpty(fileData.MailMerge.Title))
                        {
                            fileData.MailMerge.Title = "Attach";
                        }

                        var attachExt = fileData.MailMerge.Type == MailMergeType.AttachDocx ? ".docx" : ".pdf";
                        var curExt = FileUtility.GetFileExtension(fileData.MailMerge.Title);
                        if (curExt != attachExt)
                        {
                            fileData.MailMerge.Title += attachExt;
                        }

                        break;

                    case MailMergeType.Html:
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(DocumentServiceConnector.ReplaceDocumentAdress(fileData.Url));

                        // hack. http://ubuntuforums.org/showthread.php?t=1841740
                        if (WorkContext.IsMono)
                        {
                            ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true;
                        }

                        using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                        using (var stream = httpWebResponse.GetResponseStream())
                            if (stream != null)
                                using (var reader = new StreamReader(stream, Encoding.GetEncoding(Encoding.UTF8.WebName)))
                                {
                                    message = reader.ReadToEnd();
                                }
                        break;
                }

                using (var mailMergeTask =
                    new MailMergeTask
                    {
                        From = fileData.MailMerge.From,
                        Subject = fileData.MailMerge.Subject,
                        To = fileData.MailMerge.To,
                        Message = message,
                        AttachTitle = fileData.MailMerge.Title,
                        Attach = attach
                    })
                {
                    var response = MailMergeTaskRunner.Run(mailMergeTask);
                    Logger.InfoFormat("DocService mailMerge {0}/{1} send: {2}",
                                             fileData.MailMerge.RecordIndex + 1, fileData.MailMerge.RecordCount, response);
                }
                saveMessage = null;
            }
            catch (Exception ex)
            {
                Logger.Error(
                    string.Format("DocService mailMerge{0} error: userId - {1}, url - {2}",
                                  (fileData.MailMerge == null ? "" : " " + fileData.MailMerge.RecordIndex + "/" + fileData.MailMerge.RecordCount),
                                  userId, fileData.Url),
                    ex);
                saveMessage = ex.Message;
            }

            if (fileData.MailMerge != null &&
                fileData.MailMerge.RecordIndex == fileData.MailMerge.RecordCount - 1)
            {
                var errorCount = fileData.MailMerge.RecordErrorCount;
                if (!string.IsNullOrEmpty(saveMessage)) errorCount++;

                NotifyClient.SendMailMergeEnd(userId, fileData.MailMerge.RecordCount, errorCount);
            }

            return new TrackResponse { Message = saveMessage };
        }


        private void StoringFileAfterError<T>(T fileId, string userId, string downloadUri)
        {
            if (string.IsNullOrEmpty(downloadUri)) return;

            try
            {
                var fileName = Global.ReplaceInvalidCharsAndTruncate(fileId + FileUtility.GetFileExtension(downloadUri));
                var path = string.Format(@"save_crash\{0}\{1}_{2}",
                                         DateTime.UtcNow.ToString("yyyy_MM_dd"),
                                         userId,
                                         fileName);

                var store = GlobalStore.GetStore();
                var req = (HttpWebRequest)WebRequest.Create(downloadUri);

                // hack. http://ubuntuforums.org/showthread.php?t=1841740
                if (WorkContext.IsMono)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true;
                }

                using (var fileStream = new ResponseStream(req.GetResponse()))
                {
                    store.Save(FileConstant.StorageDomainTmp, path, fileStream);
                }
                Logger.DebugFormat("DocService storing to {0}", path);
            }
            catch (Exception ex)
            {
                Logger.Error("DocService Error on save file to temp store", ex);
            }
        }

        private void SaveHistory<T>(File<T> file, string changes, string differenceUrl)
        {
            if (file == null) return;
            if (file.ProviderEntry) return;
            if (string.IsNullOrEmpty(changes) || string.IsNullOrEmpty(differenceUrl)) return;

            try
            {
                var fileDao = DaoFactory.GetFileDao<T>();
                var req = (HttpWebRequest)WebRequest.Create(differenceUrl);

                // hack. http://ubuntuforums.org/showthread.php?t=1841740
                if (WorkContext.IsMono)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true;
                }

                using var differenceStream = new ResponseStream(req.GetResponse());
                fileDao.SaveEditHistory(file, changes, differenceStream);
            }
            catch (Exception ex)
            {
                Logger.Error("DocService save history error", ex);
            }
        }
    }
}
