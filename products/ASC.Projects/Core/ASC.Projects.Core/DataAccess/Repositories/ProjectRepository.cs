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
using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// A repository working with <see cref="DbProject"/> entity.
    /// </summary>
    internal class ProjectRepository : BaseTenantRepository<DbProject, int>, IProjectRepository
    {
        #region  .ctor

        public ProjectRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        #endregion .ctor

        public List<DbProject> GetAll(int max, ProjectStatus? status = null, int? offset = null)
        {
            var predicate = GetAll();

            if (status.HasValue)
            {
                predicate = predicate
                    .Where(p => p.Status == status.Value);
            }

            var result = predicate
                .Skip(offset ?? default)
                .Take(max)
                .OrderByDescending(p => p.Title)
                .ThenBy(p => p.CreationDate)
                .ToList();

            return result;
        }

        public List<DbProject> GetOpenProjectsWithTasks(Guid? participantId = null)
        {
            var predicate = GetAll();

            if (participantId.HasValue)
            {
                // ToDo: implement this later.
            }

            var result = predicate
                .Where(p => p.Status == ProjectStatus.Open)
                .OrderByDescending(p => p.Title)
                .ToList();

            return result;
        }

        public List<DbProject> GetProjectByIds(List<int> ids)
        {
            var projects = GetAll()
                .Where(p => ids.Contains(p.Id))
                .ToList();

            return projects;
        }

        // ToDo: implement this later, add crm_projects support.
        public List<DbProject> GetProjectByContactId(int contactId)
        {
            throw new NotImplementedException();
        }

        // ToDo: implement this later, add crm_projects support.
        public void AddProjectContact(int projectId, int contactId)
        {
            throw new NotImplementedException();
        }

        // ToDo: implement this later, add crm_projects support.
        public void DeleteProjectContact(int projectId, int contactId)
        {
            throw new NotImplementedException();
        }

        // ToDo: implement this later.
        public List<int> GetProjectsTaskCount(List<int> projectIds, TaskStatus? taskStatus, bool isAdmin = false)
        {
            throw new NotImplementedException();
        }

        public int GetMessageCount(int projectId)
        {
            var result = GetAll()
                .Include(p => p.Messages)
                .Where(p => p.Id == projectId)
                .Select(p => p.Messages)
                .Count();

            return result;
        }

        public int GetTotalTimeCount(int projectId)
        {
            var result = GetAll()
                .Include(p => p.LoggedTime)
                .Where(p => p.Id == projectId)
                .Select(p => p.LoggedTime)
                .Count();

            return result;
        }

        public int GetProjectMilestoneCount(int projectId, params MilestoneStatus[] statuses)
        {
            var predicate = GetAll()
                .Include(p => p.Milestones)
                .Where(p => p.Id == projectId);


            if (statuses?.Any() == true)
            {
                var milestonesCount = predicate
                    .SelectMany(p => p.Milestones)
                    .Count(m => statuses.Contains(m.Status));

                return milestonesCount;
            }

            var result = predicate
                .Select(p => p.Milestones)
                .Count();

            return result;
        }

        public override DbProject Update(DbProject updatedItem)
        {
            var entity = GetById(updatedItem.Id);

            if (entity == null)
            {
                throw new InvalidOperationException($"A project with ID = {updatedItem.Id} does not exists.");
            }

            entity.Title = updatedItem.Title;
            entity.Description = updatedItem.Description;
            entity.Status = updatedItem.Status;
            entity.StatusChangedDate = updatedItem.StatusChangedDate;
            entity.ResponsibleId = updatedItem.ResponsibleId;
            entity.IsPrivate = updatedItem.IsPrivate;
            entity.LastEditorId = updatedItem.LastEditorId;
            entity.LastModificationDate = updatedItem.LastModificationDate;

            var result = base.Update(updatedItem);

            return result;
        }

        public DateTime GetMaximalLastModificationDate()
        {
            var maxDate = GetAll()
                .Max(p => p.LastModificationDate);

            var result = TenantUtil.DateTimeFromUtc(TimeZoneInfo.Local, maxDate.GetValueOrDefault());

            return result;
        }

        // ToDo: implement this later.
        public List<DbProject> GetByParticipant(Guid participantId, ProjectStatus status)
        {
            throw new NotImplementedException();
        }

        // ToDo: implement this later.
        public List<DbProject> GetByFilter()
        {
            throw new NotImplementedException();
        }

        // ToDo: implement this later.
        public int GetByFilterCount()
        {
            throw new NotImplementedException();
        }
    }
}
