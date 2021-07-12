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

using System.Collections.Generic;
using System.Linq;
using ASC.Core;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// A repository working with <see cref="DbProjectTaskLink"/> entity.
    /// </summary>
    internal class TaskLinkRepository: BaseTenantRepository<DbProjectTaskLink, int>, ITaskLinkRepository
    {
        #region .ctor

        public TaskLinkRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        #endregion .ctor

        /// <summary>
        /// Receives links of task with specified id.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>Links of task with specified id.</returns>
        public List<DbProjectTaskLink> GetTaskLinks(int taskId)
        {
            var result = GetAll()
                .Where(tl => tl.TaskId == taskId)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives links of tasks having specified ids.
        /// </summary>
        /// <param name="taskIds">Ids of needed tasks.</param>
        /// <returns>Links of tasks having specified ids.</returns>
        public List<DbProjectTaskLink> GetByTaskIds(List<int> taskIds)
        {
            var result = GetAll()
                .Where(tl => taskIds.Contains(tl.TaskId) || taskIds.Contains(tl.ParentId))
                .ToList();

            return result;
        }

        /// <summary>
        /// Determines an existence of task link.
        /// </summary>
        /// <param name="parentTaskId">Id of parent task link.</param>
        /// <param name="dependenceTaskId">Id of dependence task link.</param>
        /// <returns>true if task link exists, otherwise - false.</returns>
        public bool Exists(int parentTaskId, int dependenceTaskId)
        {
            var count = GetAll()
                .Count(tl => (tl.TaskId == dependenceTaskId && tl.ParentId == parentTaskId)
                    || (tl.TaskId == parentTaskId && tl.ParentId == dependenceTaskId));

            var result = count != default;

            return result;
        }
    }
}