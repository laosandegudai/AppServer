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
using System.Linq;
using System.Text.Json;

using ASC.Common;
using ASC.Common.Threading;
using ASC.Core.Tenants;
using ASC.Files.Core.Resources;
using ASC.Web.Files.Core.Compress;

using Microsoft.Extensions.Primitives;

namespace ASC.Web.Files.Services.WCFService.FileOperations
{
    [Singletone(Additional = typeof(FileOperationsManagerHelperExtention))]
    public class FileOperationsManager
    {
        private readonly DistributedTaskQueue tasks;

        private TempStream TempStream { get; }
        private IServiceProvider ServiceProvider { get; }

        public FileOperationsManager(TempStream tempStream, DistributedTaskQueueOptionsManager distributedTaskQueueOptionsManager, IServiceProvider serviceProvider)
        {
            tasks = distributedTaskQueueOptionsManager.Get<FileOperation>();
            TempStream = tempStream;
            ServiceProvider = serviceProvider;
        }

        public List<FileOperationResult> GetOperationResults(Guid userId)
        {
            var operations = tasks.GetTasks();
            var processlist = Process.GetProcesses();

            //TODO: replace with distributed cache
            if (processlist.Any())
            {
                foreach (var o in operations.Where(o => processlist.All(p => p.Id != o.InstanceId)))
                {
                    o.SetProperty(FileOperation.PROGRESS, 100);
                    tasks.RemoveTask(o.Id);
                }
            }

            operations = operations.Where(t => t.GetProperty<Guid>(FileOperation.OWNER) == userId);
            foreach (var o in operations.Where(o => o.Status > DistributedTaskStatus.Running))
            {
                o.SetProperty(FileOperation.PROGRESS, 100);
                tasks.RemoveTask(o.Id);
            }

            var results = operations
                .Where(o => o.GetProperty<bool>(FileOperation.HOLD) || o.GetProperty<int>(FileOperation.PROGRESS) != 100)
                .Select(o => new FileOperationResult
                {
                    Id = o.Id,
                    OperationType = o.GetProperty<FileOperationType>(FileOperation.OPERATION_TYPE),
                    Source = o.GetProperty<string>(FileOperation.SOURCE),
                    Progress = o.GetProperty<int>(FileOperation.PROGRESS),
                    Processed = o.GetProperty<int>(FileOperation.PROCESSED).ToString(),
                    Result = o.GetProperty<string>(FileOperation.RESULT),
                    Error = o.GetProperty<string>(FileOperation.ERROR),
                    Finished = o.GetProperty<bool>(FileOperation.FINISHED),
                })
                .ToList();

            return new List<FileOperationResult>(results);
        }

        public List<FileOperationResult> CancelOperations(Guid userId)
        {
            var operations = tasks.GetTasks()
                .Where(t => t.GetProperty<Guid>(FileOperation.OWNER) == userId);

            foreach (var o in operations)
            {
                tasks.CancelTask(o.Id);
            }

            return GetOperationResults(userId);
        }


        public List<FileOperationResult> MarkAsRead(Guid userId, Tenant tenant, List<JsonElement> folderIds, List<JsonElement> fileIds)
        {
            var op1 = new FileMarkAsReadOperation<int>(ServiceProvider, new FileMarkAsReadOperationData<int>(GetIntIds(folderIds), GetIntIds(fileIds), tenant));
            var op2 = new FileMarkAsReadOperation<string>(ServiceProvider, new FileMarkAsReadOperationData<string>(GetStringIds(folderIds), GetStringIds(fileIds), tenant));
            var op = new FileMarkAsReadOperation(ServiceProvider, op2, op1);
            return QueueTask(userId, op);
        }

        public List<FileOperationResult> Download(Guid userId, Tenant tenant, Dictionary<JsonElement, string> folders, Dictionary<JsonElement, string> files, IDictionary<string, StringValues> headers)
        {
            var operations = tasks.GetTasks()
                .Where(t => t.GetProperty<Guid>(FileOperation.OWNER) == userId)
                .Where(t => t.GetProperty<FileOperationType>(FileOperation.OPERATION_TYPE) == FileOperationType.Download);

            if (operations.Any(o => o.Status <= DistributedTaskStatus.Running))
            {
                throw new InvalidOperationException(FilesCommonResource.ErrorMassage_ManyDownloads);
            }

            var op1 = new FileDownloadOperation<int>(ServiceProvider, new FileDownloadOperationData<int>(GetIntIds(folders), GetIntIds(files), tenant, headers));
            var op2 = new FileDownloadOperation<string>(ServiceProvider, new FileDownloadOperationData<string>(GetStringIds(folders), GetStringIds(files), tenant, headers));
            var op = new FileDownloadOperation(ServiceProvider, TempStream, op2, op1);

            return QueueTask(userId, op);
        }

        public List<FileOperationResult> MoveOrCopy(Guid userId, Tenant tenant, List<JsonElement> folders, List<JsonElement> files, JsonElement destFolderId, bool copy, FileConflictResolveType resolveType, bool holdResult, IDictionary<string, StringValues> headers)
        {
            var op1 = new FileMoveCopyOperation<int>(ServiceProvider, new FileMoveCopyOperationData<int>(GetIntIds(folders), GetIntIds(files), tenant, destFolderId, copy, resolveType, holdResult, headers));
            var op2 = new FileMoveCopyOperation<string>(ServiceProvider, new FileMoveCopyOperationData<string>(GetStringIds(folders), GetStringIds(files), tenant, destFolderId, copy, resolveType, holdResult, headers));
            var op = new FileMoveCopyOperation(ServiceProvider, op2, op1);

            return QueueTask(userId, op);
        }

        public List<FileOperationResult> Delete<T>(Guid userId, Tenant tenant, IEnumerable<T> folders, IEnumerable<T> files, bool ignoreException, bool holdResult, bool immediately, IDictionary<string, StringValues> headers)
        {
            var op = new FileDeleteOperation<T>(ServiceProvider, new FileDeleteOperationData<T>(folders, files, tenant, holdResult, ignoreException, immediately, headers));
            return QueueTask(userId, op);
        }

        public List<FileOperationResult> Delete(Guid userId, Tenant tenant, List<JsonElement> folders, List<JsonElement> files, bool ignoreException, bool holdResult, bool immediately, IDictionary<string, StringValues> headers)
        {
            var op1 = new FileDeleteOperation<int>(ServiceProvider, new FileDeleteOperationData<int>(GetIntIds(folders), GetIntIds(files), tenant, holdResult, ignoreException, immediately, headers));
            var op2 = new FileDeleteOperation<string>(ServiceProvider, new FileDeleteOperationData<string>(GetStringIds(folders), GetStringIds(files), tenant, holdResult, ignoreException, immediately, headers));
            var op = new FileDeleteOperation(ServiceProvider, op2, op1);

            return QueueTask(userId, op);
        }


        private List<FileOperationResult> QueueTask(Guid userId, FileOperation op)
        {
            tasks.QueueTask(op.RunJob, op.GetDistributedTask());
            return GetOperationResults(userId);
        }

        private List<FileOperationResult> QueueTask<T, TId>(Guid userId, FileOperation<T, TId> op) where T : FileOperationData<TId>
        {
            tasks.QueueTask(op.RunJob, op.GetDistributedTask());
            return GetOperationResults(userId);
        }

        public static List<int> GetIntIds(IEnumerable<JsonElement> items)
        {
            var result = items.Where(r => r.ValueKind == JsonValueKind.Number).Select(r => r.GetInt32()).ToList();

            if (result.Count() == items.Count()) return result;

            foreach (var r in GetStringIds(items))
            {
                if (int.TryParse(r, out var i))
                {
                    result.Add(i);
                }
            }

            return result;
        }

        public static List<string> GetStringIds(IEnumerable<JsonElement> items)
        {
            return items.Where(r => r.ValueKind == JsonValueKind.String).Select(r => r.GetString()).ToList();
        }

        public static Dictionary<int, string> GetIntIds(Dictionary<JsonElement, string> items)
        {
            var result = items.Where(r => r.Key.ValueKind == JsonValueKind.Number).ToDictionary(r => r.Key.GetInt32(), r => r.Value);

            if (result.Count == items.Count) return result;

            foreach (var r in GetStringIds(items))
            {
                if (int.TryParse(r.Key, out var i))
                {
                    result.Add(i, r.Value);
                }
            }

            return result;
        }

        public static Dictionary<string, string> GetStringIds(Dictionary<JsonElement, string> items)
        {
            return items.Where(r => r.Key.ValueKind == JsonValueKind.String).ToDictionary(r => r.Key.GetString(), r => r.Value);
        }
    }

    public class FileOperationsManagerHelperExtention
    {
        public static void Register(DIHelper services)
        {
            services.TryAdd<FileDeleteOperationScope>();
            services.TryAdd<FileMarkAsReadOperationScope>();
            services.TryAdd<FileMoveCopyOperationScope>();
            services.TryAdd<FileOperationScope>();
            services.TryAdd<FileDownloadOperationScope>();
            services.TryAdd<CompressToArchive>();
            services.AddDistributedTaskQueueService<FileOperation>(10);
        }
    }
}