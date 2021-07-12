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
    /// <summary>
    /// Business logic manager responsible for project subtasks processing.
    /// </summary>
    public class SubtaskManager : ProjectEntityManager, ISubtaskManager
    {
        #region Fields and .ctor

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
            _securityContext = securityContext.NotNull(nameof(securityContext));
        }

        #endregion Fields and .ctor

        /// <summary>
        /// Receives amount of subtask of task with specified Id and having one of specified statuses.
        /// </summary>
        /// <param name="taskId">Id of the task.</param>
        /// <param name="statuses">Needed statuses.</param>
        /// <returns>Amount of subtasks related to task with specified Id and having one of specified statuses.</returns>
        public int GetTaskSubtasksCount(int taskId, params TaskStatus[] statuses)
        {
            var result = _subtaskRepository.GetTaskSubtasksCountInStatuses(taskId, statuses);

            return result;
        }

        /// <summary>
        /// Receives amount of subtasks, related to task with specified Id.
        /// </summary>
        /// <param name="taskId">Id of the task.</param>
        /// <returns>Amount of subtasks related to task with specified Id.</returns>
        public int GetTaskSubtasksCount(int taskId)
        {
            var result = _subtaskRepository.GetTaskSubtasksCountInStatuses(taskId);

            return result;
        }

        /// <summary>
        /// Receives subtask having specified Id.
        /// </summary>
        /// <param name="id">Id of subtask.</param>
        /// <returns>Subtask <see cref="ProjectSubtaskData"/> with specified Id.</returns>
        public ProjectSubtaskData GetById(int id)
        {
            var subtask = _subtaskRepository.GetById(id);

            var result = _mapper.Map<DbProjectSubtask, ProjectSubtaskData>(subtask);

            return result;
        }

        /// <summary>
        /// Changes status of subtask.
        /// </summary>
        /// <param name="task">Task for status change</param>
        /// <param name="subtask">Subtask.</param>
        /// <param name="newStatus">New status for task.</param>
        /// <returns></returns>
        public ProjectSubtaskData ChangeStatus(ProjectTaskData task, ProjectSubtaskData subtask, TaskStatus newStatus)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates or updates subtask.
        /// </summary>
        /// <param name="subtask">Subtask data for creation or update.</param>
        /// <param name="task">Parent task data.</param>
        /// <returns>Just updated subtask <see cref="ProjectSubtaskData"/>.</returns>
        public ProjectSubtaskData SaveOrUpdate(ProjectSubtaskData subtask, ProjectTaskData task)
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

                var entity = _mapper.Map<ProjectSubtaskData, DbProjectSubtask>(subtask);

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

                var entity = _mapper.Map<ProjectSubtaskData, DbProjectSubtask>(subtask);

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

            var result = _mapper.Map<DbProjectSubtask, ProjectSubtaskData>(item);

            return result;
        }

        /// <summary>
        /// Creates a copy of subtask.
        /// </summary>
        /// <param name="source">Donor subtask.</param>
        /// <param name="task">Task data.</param>
        /// <param name="team">Team data.</param>
        /// <returns>A copy of provided donor subtask <see cref="ProjectSubtaskData"/>.</returns>
        public ProjectSubtaskData Copy(ProjectSubtaskData source, ProjectTaskData task, List<ParticipantData> team)
        {
            var subtask = new ProjectSubtaskData
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

        /// <summary>
        /// Deletes subtask.
        /// </summary>
        /// <param name="subtask">Subtask data.</param>
        /// <param name="task">Related task data.</param>
        public void Delete(ProjectSubtaskData subtask, ProjectTaskData task)
        {
            throw new NotImplementedException();
        }
    }
}
