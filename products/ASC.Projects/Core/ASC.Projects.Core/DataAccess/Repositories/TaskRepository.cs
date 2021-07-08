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
using System.Text;
using Amazon.S3.Model;
using ASC.Core;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    internal class TaskRepository : BaseTenantRepository<DbProjectTask, int>, ITaskRepository
    {
        public TaskRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager)
        {
        }

        public List<DbProjectTask> GetProjectTasks(int projectId,
            TaskStatus? status = null,
            Guid? participantId = null)
        {
            var predicate = GetAll()
                .Include(t => t.Milestone)
                .Where(t => t.ProjectId == projectId);

            if (status.HasValue)
            {
                predicate = predicate
                    .Where(t => t.Status == status.Value);
            }

            if (participantId.HasValue)
            {
                predicate = predicate
                    .Where(t => t.Par)
            }
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

        public void AddTaskLink(DbProjectTaskLink link)
        {
            DbContext.Set<DbProjectTaskLink>()
                .Add(link);

            DbContext.SaveChanges();
        }
    }
}
