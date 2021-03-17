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
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using ASC.Common;
using ASC.Core.Tenants;
using ASC.Files.Core;
using ASC.Files.Core.Resources;
using ASC.MessagingSystem;
using ASC.Web.Core.Files;
using ASC.Web.Files.Classes;
using ASC.Web.Files.Helpers;
using ASC.Web.Files.Utils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace ASC.Web.Files.Services.WCFService.FileOperations
{
    class FileMoveCopyOperation : ComposeFileOperation<FileMoveCopyOperationData<string>, FileMoveCopyOperationData<int>>
    {
        public FileMoveCopyOperation(Guid userId,
            FileOperation<FileMoveCopyOperationData<string>, string> thirdPartyOperation,
            FileOperation<FileMoveCopyOperationData<int>, int> daoOperation)
            : base(userId, thirdPartyOperation, daoOperation)
        {

        }

        public override FileOperationType OperationType
        {
            get
            {
                return ThirdPartyOperation.OperationType;
            }
        }
    }

    internal class FileMoveCopyOperationData<T> : FileOperationData<T>
    {
        public string ThirdpartyFolderId { get; }
        public int DaoFolderId { get; }
        public bool Copy { get; }
        public FileConflictResolveType ResolveType { get; }
        public IDictionary<string, StringValues> Headers { get; }

        public FileMoveCopyOperationData(IEnumerable<object> folders, IEnumerable<object> files, Tenant tenant, JsonElement toFolderId, bool copy, FileConflictResolveType resolveType, bool holdResult = true, IDictionary<string, StringValues> headers = null)
            : this(folders.OfType<T>(), files.OfType<T>(), tenant, toFolderId, copy, resolveType, holdResult, headers)
        {
        }

        public FileMoveCopyOperationData(IEnumerable<T> folders, IEnumerable<T> files, Tenant tenant, JsonElement toFolderId, bool copy, FileConflictResolveType resolveType, bool holdResult = true, IDictionary<string, StringValues> headers = null)
            : base(folders, files, tenant, holdResult)
        {
            if (toFolderId.ValueKind == JsonValueKind.String)
            {
                ThirdpartyFolderId = toFolderId.GetString();
            }
            else if (toFolderId.ValueKind == JsonValueKind.Number)
            {
                DaoFolderId = toFolderId.GetInt32();
            }

            Copy = copy;
            ResolveType = resolveType;
            Headers = headers;
        }
    }

    class FileMoveCopyOperation<T> : FileOperation<FileMoveCopyOperationData<T>, T>
    {
        private readonly int DaoFolderId;
        private readonly string ThirdpartyFolderId;
        private readonly bool _copy;
        private readonly FileConflictResolveType _resolveType;

        private readonly IDictionary<string, StringValues> _headers;

        public override FileOperationType OperationType
        {
            get { return _copy ? FileOperationType.Copy : FileOperationType.Move; }
        }

        public FileMoveCopyOperation(IServiceProvider serviceProvider, Guid userId, FileMoveCopyOperationData<T> data)
            : base(serviceProvider, userId, data)
        {
            DaoFolderId = data.DaoFolderId;
            ThirdpartyFolderId = data.ThirdpartyFolderId;
            _copy = data.Copy;
            _resolveType = data.ResolveType;

            _headers = data.Headers;
        }

        protected override async Task Do(IServiceScope scope)
        {
            if (DaoFolderId != 0)
            {
                await Do(scope, DaoFolderId);
            }

            if (!string.IsNullOrEmpty(ThirdpartyFolderId))
            {
                await Do(scope, ThirdpartyFolderId);
            }
        }

        private async Task Do<TTo>(IServiceScope scope, TTo tto)
        {
            var fileMarker = scope.ServiceProvider.GetService<FileMarker>();
            var folderDao = scope.ServiceProvider.GetService<IFolderDao<TTo>>();
            var FolderDao = scope.ServiceProvider.GetService<IFolderDao<T>>();

            Status += string.Format("folder_{0}{1}", DaoFolderId, SPLIT_CHAR);

            //TODO: check on each iteration?
            var toFolder = await folderDao.GetFolder(tto);
            if (toFolder == null) return;
            if (!await FilesSecurity.CanCreate(toFolder)) throw new System.Security.SecurityException(FilesCommonResource.ErrorMassage_SecurityException_Create);

            if ((await folderDao.GetParentFolders(toFolder.ID)).Any(parent => Folders.Any(r => r.ToString() == parent.ID.ToString())))
            {
                Error = FilesCommonResource.ErrorMassage_FolderCopyError;
                return;
            }

            if (_copy)
            {
                Folder<T> rootFrom = null;
                if (0 < Folders.Count) rootFrom = await FolderDao.GetRootFolder(Folders[0]);
                if (0 < Files.Count) rootFrom = await FolderDao.GetRootFolderByFile(Files[0]);
                if (rootFrom != null && rootFrom.FolderType == FolderType.TRASH) throw new InvalidOperationException("Can not copy from Trash.");
                if (toFolder.RootFolderType == FolderType.TRASH) throw new InvalidOperationException("Can not copy to Trash.");
            }

            var needToMark = new List<FileEntry<TTo>>();

            needToMark.AddRange(await MoveOrCopyFolders(scope, Folders, toFolder, _copy));
            needToMark.AddRange(await MoveOrCopyFiles(scope, Files, toFolder, _copy));

            needToMark.Distinct().ToList().ForEach(async x => await fileMarker.MarkAsNew(x));
        }

        private async Task<List<FileEntry<TTo>>> MoveOrCopyFolders<TTo>(IServiceScope scope, List<T> folderIds, Folder<TTo> toFolder, bool copy)
        {
            var needToMark = new List<FileEntry<TTo>>();

            if (folderIds.Count == 0) return needToMark;

            var scopeClass = scope.ServiceProvider.GetService<FileMoveCopyOperationScope>();
            var (filesMessageService, fileMarker, _, _, _) = scopeClass;
            var folderDao = scope.ServiceProvider.GetService<IFolderDao<TTo>>();

            var fileDao = scope.ServiceProvider.GetService<IFileDao<T>>();
            var folderDao1 = scope.ServiceProvider.GetService<IFolderDao<T>>();

            var toFolderId = toFolder.ID;
            var isToFolder = Equals(toFolderId, DaoFolderId);


            foreach (var folderId in folderIds)
            {
                CancellationToken.ThrowIfCancellationRequested();

                var folder = await folderDao1.GetFolder(folderId);
                if (folder == null)
                {
                    Error = FilesCommonResource.ErrorMassage_FolderNotFound;
                }
                else if (!await FilesSecurity.CanRead(folder))
                {
                    Error = FilesCommonResource.ErrorMassage_SecurityException_ReadFolder;
                }
                else if (folder.RootFolderType == FolderType.Privacy
                    && (copy || toFolder.RootFolderType != FolderType.Privacy))
                {
                    Error = FilesCommonResource.ErrorMassage_SecurityException_MoveFolder;
                }
                else if (!Equals(folder.FolderID ?? default, toFolderId) || _resolveType == FileConflictResolveType.Duplicate)
                {
                    try
                    {
                        //if destination folder contains folder with same name then merge folders
                        var conflictFolder = folder.RootFolderType == FolderType.Privacy
                            ? null
                            : await folderDao.GetFolder(folder.Title, toFolderId);
                        Folder<TTo> newFolder;

                        if (copy || conflictFolder != null)
                        {
                            if (conflictFolder != null)
                            {
                                newFolder = conflictFolder;

                                if (isToFolder)
                                    needToMark.Add(conflictFolder);
                            }
                            else
                            {
                                newFolder = await folderDao1.CopyFolder(folder.ID, toFolderId, CancellationToken);
                                filesMessageService.Send(newFolder, toFolder, _headers, MessageAction.FolderCopied, newFolder.Title, toFolder.Title);

                                if (isToFolder)
                                    needToMark.Add(newFolder);

                                if (ProcessedFolder(folderId))
                                {
                                    Status += string.Format("folder_{0}{1}", newFolder.ID, SPLIT_CHAR);
                                }
                            }

                            if (folderDao1.UseRecursiveOperation(folder.ID, toFolderId))
                            {
                                await MoveOrCopyFiles(scope, await fileDao.GetFiles(folder.ID), newFolder, copy);
                                await MoveOrCopyFolders(scope, (await folderDao1.GetFolders(folder.ID)).Select(f => f.ID).ToList(), newFolder, copy);

                                if (!copy)
                                {
                                    if (!await FilesSecurity.CanDelete(folder))
                                    {
                                        Error = FilesCommonResource.ErrorMassage_SecurityException_MoveFolder;
                                    }
                                    else if (await folderDao1.IsEmpty(folder.ID))
                                    {
                                        await folderDao1.DeleteFolder(folder.ID);
                                        if (ProcessedFolder(folderId))
                                        {
                                            Status += string.Format("folder_{0}{1}", newFolder.ID, SPLIT_CHAR);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (conflictFolder != null)
                                {
                                    TTo newFolderId;
                                    if (copy)
                                    {
                                        newFolder = await folderDao1.CopyFolder(folder.ID, toFolderId, CancellationToken);
                                        newFolderId = newFolder.ID;
                                        filesMessageService.Send(newFolder, toFolder, _headers, MessageAction.FolderCopiedWithOverwriting, newFolder.Title, toFolder.Title);

                                        if (isToFolder)
                                            needToMark.Add(newFolder);

                                        if (ProcessedFolder(folderId))
                                        {
                                            Status += string.Format("folder_{0}{1}", newFolderId, SPLIT_CHAR);
                                        }
                                    }
                                    else if (!await FilesSecurity.CanDelete(folder))
                                    {
                                        Error = FilesCommonResource.ErrorMassage_SecurityException_MoveFolder;
                                    }
                                    else
                                    {
                                        var tmpError = await WithError(scope, fileDao.GetFiles(folder.ID, new OrderBy(SortedByType.AZ, true), FilterType.FilesOnly, false, Guid.Empty, string.Empty, false, true));
                                        if (!string.IsNullOrEmpty(tmpError))
                                        {
                                            Error = tmpError;
                                        }
                                        else
                                        {
                                            await fileMarker.RemoveMarkAsNewForAll(folder);

                                            newFolderId = await folderDao1.MoveFolder(folder.ID, toFolderId, CancellationToken);
                                            newFolder = await folderDao.GetFolder(newFolderId);

                                            if (folder.RootFolderType != FolderType.USER)
                                            {
                                                filesMessageService.Send(folder, toFolder, _headers, MessageAction.FolderMovedWithOverwriting, folder.Title, toFolder.Title);
                                            }
                                            else
                                            {
                                                filesMessageService.Send(newFolder, toFolder, _headers, MessageAction.FolderMovedWithOverwriting, folder.Title, toFolder.Title);
                                            }

                                            if (isToFolder)
                                                needToMark.Add(newFolder);

                                            if (ProcessedFolder(folderId))
                                            {
                                                Status += string.Format("folder_{0}{1}", newFolderId, SPLIT_CHAR);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!await FilesSecurity.CanDelete(folder))
                            {
                                Error = FilesCommonResource.ErrorMassage_SecurityException_MoveFolder;
                            }
                            else
                            {
                                var tmpError = await WithError(scope, fileDao.GetFiles(folder.ID, new OrderBy(SortedByType.AZ, true), FilterType.FilesOnly, false, Guid.Empty, string.Empty, false, true));
                                if (!string.IsNullOrEmpty(tmpError))
                                {
                                    Error = tmpError;
                                }
                                else
                                {
                                    await fileMarker.RemoveMarkAsNewForAll(folder);

                                    var newFolderId = await folderDao1.MoveFolder(folder.ID, toFolderId, CancellationToken);
                                    newFolder = await folderDao.GetFolder(newFolderId);

                                    if (folder.RootFolderType != FolderType.USER)
                                    {
                                        filesMessageService.Send(folder, toFolder, _headers, MessageAction.FolderMoved, folder.Title, toFolder.Title);
                                    }
                                    else
                                    {
                                        filesMessageService.Send(newFolder, toFolder, _headers, MessageAction.FolderMoved, folder.Title, toFolder.Title);
                                    }

                                    if (isToFolder)
                                        needToMark.Add(newFolder);

                                    if (ProcessedFolder(folderId))
                                    {
                                        Status += string.Format("folder_{0}{1}", newFolderId, SPLIT_CHAR);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Error = ex.Message;

                        Logger.Error(Error, ex);
                    }
                }
                ProgressStep(folderDao1.CanCalculateSubitems(folderId) ? default : folderId);
            }

            return needToMark;
        }

        private async Task<List<FileEntry<TTo>>> MoveOrCopyFiles<TTo>(IServiceScope scope, List<T> fileIds, Folder<TTo> toFolder, bool copy)
        {
            var needToMark = new List<FileEntry<TTo>>();

            if (fileIds.Count == 0) return needToMark;

            var scopeClass = scope.ServiceProvider.GetService<FileMoveCopyOperationScope>();
            var (filesMessageService, fileMarker, fileUtility, global, entryManager) = scopeClass;
            var fileDao = scope.ServiceProvider.GetService<IFileDao<TTo>>();
            var fileDao1 = scope.ServiceProvider.GetService<IFileDao<T>>();
            var folderDao = scope.ServiceProvider.GetService<IFolderDao<T>>();
            var fileTracker = scope.ServiceProvider.GetService<FileTrackerHelper>();

            var toFolderId = toFolder.ID;
            foreach (var fileId in fileIds)
            {
                CancellationToken.ThrowIfCancellationRequested();

                var file = await fileDao1.GetFile(fileId);
                if (file == null)
                {
                    Error = FilesCommonResource.ErrorMassage_FileNotFound;
                }
                else if (!await FilesSecurity.CanRead(file))
                {
                    Error = FilesCommonResource.ErrorMassage_SecurityException_ReadFile;
                }
                else if (file.RootFolderType == FolderType.Privacy
                    && (copy || toFolder.RootFolderType != FolderType.Privacy))
                {
                    Error = FilesCommonResource.ErrorMassage_SecurityException_MoveFile;
                }
                else if (global.EnableUploadFilter
                         && !fileUtility.ExtsUploadable.Contains(FileUtility.GetFileExtension(file.Title)))
                {
                    Error = FilesCommonResource.ErrorMassage_NotSupportedFormat;
                }
                else
                {
                    var parentFolder = await folderDao.GetFolder(file.FolderID);
                    try
                    {
                        var conflict = _resolveType == FileConflictResolveType.Duplicate
                            || file.RootFolderType == FolderType.Privacy
                                           ? null
                                           : await fileDao.GetFile(toFolderId, file.Title);
                        if (conflict == null)
                        {
                            File<TTo> newFile = null;
                            if (copy)
                            {
                                try
                                {
                                    newFile = await fileDao1.CopyFile(file.ID, toFolderId); //Stream copy will occur inside dao
                                    filesMessageService.Send(newFile, toFolder, _headers, MessageAction.FileCopied, newFile.Title, parentFolder.Title, toFolder.Title);

                                    if (Equals(newFile.FolderID.ToString(), DaoFolderId))
                                    {
                                        needToMark.Add(newFile);
                                    }

                                    if (ProcessedFile(fileId))
                                    {
                                        Status += string.Format("file_{0}{1}", newFile.ID, SPLIT_CHAR);
                                    }
                                }
                                catch
                                {
                                    if (newFile != null)
                                    {
                                        await fileDao.DeleteFile(newFile.ID);
                                    }
                                    throw;
                                }
                            }
                            else
                            {
                                var tmpError = await WithError(scope, new[] { file }.ToAsyncEnumerable());
                                if (!string.IsNullOrEmpty(tmpError))
                                {
                                    Error = tmpError;
                                }
                                else
                                {
                                    await fileMarker.RemoveMarkAsNewForAll(file);

                                    var newFileId = await fileDao1.MoveFile(file.ID, toFolderId);
                                    newFile = await fileDao.GetFile(newFileId);

                                    if (file.RootFolderType != FolderType.USER)
                                    {
                                        filesMessageService.Send(file, toFolder, _headers, MessageAction.FileMoved, file.Title, parentFolder.Title, toFolder.Title);
                                    }
                                    else
                                    {
                                        filesMessageService.Send(newFile, toFolder, _headers, MessageAction.FileMoved, file.Title, parentFolder.Title, toFolder.Title);
                                    }

                                    if (Equals(toFolderId.ToString(), DaoFolderId))
                                    {
                                        needToMark.Add(newFile);
                                    }

                                    if (ProcessedFile(fileId))
                                    {
                                        Status += string.Format("file_{0}{1}", newFileId, SPLIT_CHAR);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (_resolveType == FileConflictResolveType.Overwrite)
                            {
                                if (!await FilesSecurity.CanEdit(conflict))
                                {
                                    Error = FilesCommonResource.ErrorMassage_SecurityException;
                                }
                                else if (entryManager.FileLockedForMe(conflict.ID))
                                {
                                    Error = FilesCommonResource.ErrorMassage_LockedFile;
                                }
                                else if (fileTracker.IsEditing(conflict.ID))
                                {
                                    Error = FilesCommonResource.ErrorMassage_SecurityException_UpdateEditingFile;
                                }
                                else
                                {
                                    var newFile = conflict;
                                    newFile.Version++;
                                    newFile.VersionGroup++;
                                    newFile.PureTitle = file.PureTitle;
                                    newFile.ConvertedType = file.ConvertedType;
                                    newFile.Comment = FilesCommonResource.CommentOverwrite;
                                    newFile.Encrypted = file.Encrypted;

                                    using (var stream = fileDao1.GetFileStream(file))
                                    {
                                        newFile.ContentLength = stream.CanSeek ? stream.Length : file.ContentLength;

                                        newFile = await fileDao.SaveFile(newFile, stream);
                                    }

                                    needToMark.Add(newFile);

                                    if (copy)
                                    {
                                        filesMessageService.Send(newFile, toFolder, _headers, MessageAction.FileCopiedWithOverwriting, newFile.Title, parentFolder.Title, toFolder.Title);
                                        if (ProcessedFile(fileId))
                                        {
                                            Status += string.Format("file_{0}{1}", newFile.ID, SPLIT_CHAR);
                                        }
                                    }
                                    else
                                    {
                                        if (Equals(file.FolderID.ToString(), toFolderId.ToString()))
                                        {
                                            if (ProcessedFile(fileId))
                                            {
                                                Status += string.Format("file_{0}{1}", newFile.ID, SPLIT_CHAR);
                                            }
                                        }
                                        else
                                        {
                                            var tmpError = await WithError(scope, new[] { file }.ToAsyncEnumerable());
                                            if (!string.IsNullOrEmpty(tmpError))
                                            {
                                                Error = tmpError;
                                            }
                                            else
                                            {
                                                await fileDao1.DeleteFile(file.ID);

                                                if (file.RootFolderType != FolderType.USER)
                                                {
                                                    filesMessageService.Send(file, toFolder, _headers, MessageAction.FileMovedWithOverwriting, file.Title, parentFolder.Title, toFolder.Title);
                                                }
                                                else
                                                {
                                                    filesMessageService.Send(newFile, toFolder, _headers, MessageAction.FileMovedWithOverwriting, file.Title, parentFolder.Title, toFolder.Title);
                                                }

                                                if (ProcessedFile(fileId))
                                                {
                                                    Status += string.Format("file_{0}{1}", newFile.ID, SPLIT_CHAR);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (_resolveType == FileConflictResolveType.Skip)
                            {
                                //nothing
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Error = ex.Message;
                        Logger.Error(Error, ex);
                    }
                }
                ProgressStep(fileId: folderDao.CanCalculateSubitems(fileId) ? default : fileId);
            }

            return needToMark;
        }

        private async Task<string> WithError(IServiceScope scope, IAsyncEnumerable<File<T>> files)
        {
            var entryManager = scope.ServiceProvider.GetService<EntryManager>();
            var fileTracker = scope.ServiceProvider.GetService<FileTrackerHelper>();
            await foreach (var file in files)
            {
                if (!await FilesSecurity.CanDelete(file))
                {
                    return FilesCommonResource.ErrorMassage_SecurityException_MoveFile;
                }
                if (entryManager.FileLockedForMe(file.ID))
                {
                    return FilesCommonResource.ErrorMassage_LockedFile;
                }
                if (fileTracker.IsEditing(file.ID))
                {
                    return FilesCommonResource.ErrorMassage_SecurityException_UpdateEditingFile;
                }
            }

            return null;
        }
    }

    [Scope]
    public class FileMoveCopyOperationScope
    {
        private FilesMessageService FilesMessageService { get; }
        private FileMarker FileMarker { get; }
        private FileUtility FileUtility { get; }
        private Global Global { get; }
        private EntryManager EntryManager { get; }

        public FileMoveCopyOperationScope(FilesMessageService filesMessageService, FileMarker fileMarker, FileUtility fileUtility, Global global, EntryManager entryManager)
        {
            FilesMessageService = filesMessageService;
            FileMarker = fileMarker;
            FileUtility = fileUtility;
            Global = global;
            EntryManager = entryManager;
        }

        public void Deconstruct(out FilesMessageService filesMessageService, out FileMarker fileMarker, out FileUtility fileUtility, out Global global, out EntryManager entryManager)
        {
            filesMessageService = FilesMessageService;
            fileMarker = FileMarker;
            fileUtility = FileUtility;
            global = Global;
            entryManager = EntryManager;
        }
    }
}