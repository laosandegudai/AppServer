#region License agreement statement

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

#endregion License agreement statement

using System;
using System.Collections.Generic;
using System.Linq;
using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Core.Tenants;
using ASC.Notify.Model;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.BusinessLogic.Notifications.Data;
using ASC.Projects.Core.BusinessLogic.Notifications.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    public class SubtaskManager : ProjectEntityManager, ISubtaskManager
    {
        private readonly ISubtaskRepository _subtaskRepository;

        private readonly IMapper _mapper;

        private readonly IProjectNotificationSender _projectNotificationSender;

        private readonly SecurityContext _securityContext;


        public SubtaskManager(INotifySource notifySource,
            INotifyAction notifyAction,
            ISubtaskRepository subtaskRepository,
            IMapper mapper,
            IProjectNotificationSender projectNotificationSender,
            SecurityContext securityContext,
            bool disableNotifications) : base(notifySource, notifyAction, securityContext, disableNotifications)
        {
            _subtaskRepository = subtaskRepository.NotNull(nameof(subtaskRepository));
            _mapper = mapper.NotNull(nameof(mapper));
            _projectNotificationSender = projectNotificationSender.NotNull(nameof(projectNotificationSender));
        }


        public int GetTaskSubtasksCount(int taskId, params TaskStatus[] statuses)
        {
            var result = _subtaskRepository.GetTaskSubtasksCountInStatuses(taskId, statuses);

            return result;
        }

        public int GetTaskSubtasksCount(int taskId, params System.Threading.Tasks.TaskStatus[] statuses)
        {
            throw new NotImplementedException();
        }

        public int GetTaskSubtasksCount(int taskId)
        {
            var result = _subtaskRepository.GetTaskSubtasksCountInStatuses(taskId);

            return result;
        }

        public SubtaskData GetById(int id)
        {
            var subtask = _subtaskRepository.GetById(id);

            var result = _mapper.Map<DbProjectSubtask, SubtaskData>(subtask);

            return result;
        }

        public SubtaskData ChangeStatus(TaskData task, SubtaskData subtask, System.Threading.Tasks.TaskStatus newStatus)
        {
            throw new NotImplementedException();
        }

        public SubtaskData SaveOrUpdate(SubtaskData subtask, TaskData task)
        {
            subtask.NotNull(nameof(subtask));
            subtask.NotNull(nameof(task));

            if (task.Status == TaskStatus.Closed)
            {
                throw new InvalidOperationException("Task cannot be closed");
            }

            DbProjectSubtask item;
            var isNew = subtask.Id == default;
            var oldResponsible = Guid.Empty;

            subtask.LastEditorId = _securityContext.CurrentAccount.ID;
            subtask.LastModificationDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local);

            if (isNew)
            {
                subtask.CreatorId ??= _securityContext.CurrentAccount.ID;
                subtask.CreationDate ??= TenantUtil.DateTimeNow(TimeZoneInfo.Local);

                var entity = _mapper.Map<SubtaskData, DbProjectSubtask>(subtask);

                item = _subtaskRepository.Update(entity);

                var notificationData = new SubtaskNotificationData
                {
                    ResponsibleId = entity.ResponsibleId,
                    InitiatorId = _securityContext.CurrentAccount.ID.ToString(),
                    ProjectTitle = entity.RootTask.Project.Title,
                    ProjectId = entity.RootTask.ProjectId,
                    SubtaskId = item.Id,
                    SubtaskTitle = entity.Title,
                    TaskId = entity.RootTaskId,
                    TaskTitle = entity.RootTask.Title
                };

                _projectNotificationSender.SendSubtaskCreatedNotification(notificationData);
            }
            else
            {
                var existingSubtask = GetById(subtask.Id);

                if (existingSubtask == null)
                {
                    throw new InvalidOperationException($"A subtask with ID = {subtask.Id} does not exists");
                }

                oldResponsible = existingSubtask.ResponsibleId;

                var entity = _mapper.Map<SubtaskData, DbProjectSubtask>(subtask);

                item = _subtaskRepository.Update(entity);

                var notificationData = new SubtaskNotificationData
                {
                    ResponsibleId = entity.ResponsibleId,
                    InitiatorId = _securityContext.CurrentAccount.ID.ToString(),
                    ProjectTitle = entity.RootTask.Project.Title,
                    ProjectId = entity.RootTask.ProjectId,
                    SubtaskId = entity.Id,
                    SubtaskTitle = entity.Title,
                    TaskId = entity.RootTaskId,
                    TaskTitle = entity.RootTask.Title
                };

                _projectNotificationSender.SendSubtaskModifiedNotification(notificationData);
            }

            var result = _mapper.Map<DbProjectSubtask, SubtaskData>(item);

            return result;
        }

        public SubtaskData Copy(SubtaskData source, TaskData task, List<ParticipantData> team)
        {
            var subtask = new SubtaskData
            {
                CreatorId = _securityContext.CurrentAccount.ID,
                CreationDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local),
                RootTaskId = source.RootTaskId,
                Title = source.Title,
                Status = source.Status
            };

            if (team.Any(r => r.Id == source.ResponsibleId))
            {
                subtask.ResponsibleId = source.ResponsibleId;
            }

            var result = SaveOrUpdate(subtask, task);

            return result;
        }

        public void Delete(SubtaskData subtask, TaskData task)
        {
            throw new NotImplementedException();
        }
    }
}
