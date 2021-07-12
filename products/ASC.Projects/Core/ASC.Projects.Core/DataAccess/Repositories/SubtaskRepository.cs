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
using ASC.Core.Tenants;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// Repository working with <see cref="DbProjectSubtask"/> entity.
    /// </summary>
    internal class SubtaskRepository : BaseTenantRepository<DbProjectSubtask, int>, ISubtaskRepository
    {
        #region .ctor

        public SubtaskRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }


        #endregion .ctor

        /// <summary>
        /// Receives subtask of task with specified id.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>List of subtasks <see cref="DbProjectTask"/> of task with specified id.</returns>
        public List<DbProjectSubtask> GetSubtasksOfTask(int taskId)
        {
            var result = GetAll()
                .Where(st => st.RootTaskId == taskId)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives subtasks with specified ids.
        /// </summary>
        /// <param name="ids">Ids of needed subtasks.</param>
        /// <returns>List of subtasks <see cref="DbProjectTask"/> with specified ids.</returns>
        public List<DbProjectSubtask> GetByIds(List<int> ids)
        {
            var result = GetAll()
                .Where(st => ids.Contains(st.Id))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives an updates of subtasks for specific interval.
        /// </summary>
        /// <param name="from">Beginning of interval.</param>
        /// <param name="to">End of interval.</param>
        /// <returns>List of subtasks <see cref="DbProjectTask"/> with updates.</returns>
        public List<DbProjectSubtask> GetUpdates(DateTime from, DateTime to)
        {
            var result = GetAll()
                .Where(st => (st.CreationDate > from && st.CreationDate < to)
                    || (st.LastModificationDate > from && st.CreationDate <  to)
                    || (st.StatusChangeDate > from && st.StatusChangeDate < to))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives subtasks which person with specified id responsible for, having specific status.
        /// </summary>
        /// <param name="responsibleId">Id of responsible.</param>
        /// <param name="status">Needed status.</param>
        /// <returns>List of subtasks <see cref="DbProjectSubtask"/> which person with specified id responsible for, having specific status.</returns>
        public List<DbProjectSubtask> GetSubtasksOfResponsible(Guid responsibleId, TaskStatus? status = null)
        {
            var predicate = GetAll()
                .Where(st => st.ResponsibleId == responsibleId);

            if (status.HasValue)
            {
                predicate = predicate
                    .Where(t => t.Status == status);
            }

            var result = predicate.ToList();

            return result;
        }

        /// <summary>
        /// Receives an amount of subtasks of specific task having one of specified statuses.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <param name="statuses">Needed statuses.</param>
        /// <returns>Amount of subtasks of specific task having one of specified statuses</returns>
        public int GetTaskSubtasksCountInStatuses(int taskId, params TaskStatus[] statuses)
        {
            var predicate = GetAll()
                .Where(st => st.RootTaskId == taskId);

            if (statuses?.Any() == true)
            {
                predicate = predicate
                    .Where(st => statuses.Contains(st.Status));
            }

            var result = predicate
                .Count();

            return result;
        }

        /// <summary>
        /// Closes all subtasks of task.
        /// </summary>
        /// <param name="task">Task, which subtasks should be closed.</param>
        public void SetClosedStatusForSubtasks(DbProjectTask task)
        {
            task.Subtasks
                .Where(st => st.RootTaskId == task.Id && st.Status == TaskStatus.Open)
                .ToList()
                .ForEach(st =>
                {
                    st.Status = TaskStatus.Closed;
                    st.LastModificationDate = TenantUtil.DateTimeToUtc(TimeZoneInfo.Local, DateTime.Now);
                    st.StatusChangeDate = TenantUtil.DateTimeToUtc(TimeZoneInfo.Local, DateTime.Now);
                });

            DbContext.SaveChanges();
        }

        /// <summary>
        /// Updates an existing subtask.
        /// </summary>
        /// <param name="updatedItem">Updating subtask.</param>
        /// <returns>Just updated subtask.</returns>
        public override DbProjectSubtask Update(DbProjectSubtask updatedItem)
        {
            var updatingItem = GetById(updatedItem.Id);

            updatingItem.RootTaskId = updatedItem.RootTaskId;
            updatingItem.Title = updatedItem.Title;
            updatingItem.ResponsibleId = updatedItem.ResponsibleId;
            updatingItem.Status = updatedItem.Status;
            updatingItem.CreatorId = updatedItem.CreatorId;
            updatingItem.CreationDate = updatedItem.CreationDate;
            updatingItem.LastModificationDate = updatedItem.LastModificationDate;
            updatingItem.LastEditorId = updatedItem.LastEditorId;
            updatingItem.StatusChangeDate = updatedItem.StatusChangeDate;

            var result = base.Update(updatedItem);

            return result;
        }
    }
}
