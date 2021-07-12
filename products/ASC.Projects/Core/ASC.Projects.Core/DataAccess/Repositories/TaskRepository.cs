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
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// A repository working with <see cref="DbProjectTask"/> entity.
    /// </summary>
    internal class TaskRepository : BaseTenantRepository<DbProjectTask, int>, ITaskRepository
    {
        #region .ctor

        public TaskRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        #endregion .ctor

        public List<DbProjectTask> GetProjectTasks(int projectId,
            TaskStatus? status = null,
            Guid? participantId = null)
        {
            var predicate = GetAll()
                .Include(t => t.Milestone)
                .Include(t => t.Project)
                .Include(t => t.Subtasks)
                .Where(t => t.ProjectId == projectId);

            if (status.HasValue)
            {
                predicate = predicate
                    .Where(t => t.Status == status);
            }

            var result = predicate
                .OrderByDescending(t => t.SortOrder)
                .ThenBy(t => t.Milestone.Status)
                .ThenBy(t => t.Milestone.Deadline)
                .ThenBy(t => t.Milestone.Id)
                .ThenBy(t => t.Status)
                .ThenBy(t => t.Priority)
                .ThenBy(t => t.CreationDate)
                .ToList();

            return result;
        }

        // ToDo: implement this later.
        //public List<Task> GetByFilter(TaskFilter filter, bool isAdmin, bool checkAccess)

        // ToDo: implement this later.
        //public TaskFilterCountOperationResult GetByFilterCount(TaskFilter filter, bool isAdmin, bool checkAccess)

        // ToDo: implement this later.
        //public IEnumerable<TaskFilterCountOperationResult> GetByFilterCountForStatistic(TaskFilter filter, bool isAdmin, bool checkAccess)

        // ToDo: implement this later.
        //public List<Tuple<Guid, int, int>> GetByFilterCountForReport(TaskFilter filter, bool isAdmin, bool checkAccess)

        // ToDo: implement this later.
        public List<DbProjectTask> GetByResponsible(Guid responsibleId, TaskStatus? status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives subtasks of tasks having specified ids.
        /// </summary>
        /// <param name="taskIds">Ids of needed tasks.</param>
        /// <returns>List of task with subtasks.</returns>
        public List<DbProjectTask> GetSubtasksOfTasks(List<int> taskIds)
        {
            var result = GetAll()
                .Include(t => t.Subtasks)
                .Where(t => taskIds.Contains(t.Id))
                .ToList();

            return result;
        }

        public List<DbProjectTask> GetMilestoneTasks(int milestoneId)
        {
            var result = GetAll()
                .Where(t => t.MilestoneId == milestoneId)
                .OrderByDescending(t => t.SortOrder)
                .ThenBy(t => t.Status)
                .ThenByDescending(t => t.Priority)
                .ThenByDescending(t => t.CreationDate)
                .ToList();

            return result;
        }

        public List<DbProjectTask> GetByIds(List<int> ids)
        {
            var result = GetAll()
                .Where(t => ids.Contains(t.Id))
                .ToList();

            return result;
        }

        public List<DbProjectTask> GetTasksForReminder(DateTime deadline)
        {
            var deadlineDate = deadline.Date;
            var yesterday = deadline.AddDays(-1);
            var tomorrow = deadline.AddDays(1);

            var result = GetAll()
                .Include(t => t.Project)
                .Where(t => t.Deadline >= yesterday
                    && t.Deadline <= yesterday
                    && t.Status != TaskStatus.Closed
                    && t.Project.Status == ProjectStatus.Open)
                .ToList();

            return result;
        }

        public override DbProjectTask Update(DbProjectTask updatingItem)
        {
            var existingItem = GetById(updatingItem.Id);

            if (existingItem == null)
            {
                throw new InvalidOperationException($"Task with ID = {updatingItem.Id} does not exists");
            }

            existingItem.ProjectId = updatingItem.ProjectId;
            existingItem.Title = updatingItem.Title;
            existingItem.LastModificationDate = updatingItem.LastModificationDate;
            existingItem.LastEditorId = updatingItem.LastEditorId;
            existingItem.Description = updatingItem.Description;
            existingItem.Priority = updatingItem.Priority;
            existingItem.Status = updatingItem.Status;
            existingItem.MilestoneId = updatingItem.MilestoneId;
            existingItem.SortOrder = updatingItem.SortOrder;
            existingItem.Deadline = updatingItem.Deadline;
            existingItem.StatusChangeDate = updatingItem.StatusChangeDate;
            existingItem.StartDate = updatingItem.StartDate;
            existingItem.Progress = updatingItem.Progress;
            existingItem.ResponsibleId = updatingItem.ResponsibleId;

            var result = base.Update(existingItem);

            return result;
        }
    }
}
