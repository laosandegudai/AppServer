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
    /// Repository working with 'Milestone' entity.
    /// </summary>
    public class MilestoneRepository : BaseTenantRepository<DbMilestone, int>, IMilestoneRepository
    {
        public MilestoneRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        /// <summary>
        /// Receives a milestones related to specific project.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <returns>List of milestones <see cref="DbMilestone"/> related to project with specified Id.</returns>
        public List<DbMilestone> GetProjectMilestones(int projectId)
        {
            var milestones = GetAll()
                .Where(m => m.ProjectId == projectId)
                .ToList();

            return milestones;
        }

        // ToDo: implement this later.
        public List<DbMilestone> GetByFilter()
        {
            throw new NotImplementedException();
        }

        // ToDo: implement this later.
        public int GetByFilterCount()
        {
            throw new NotImplementedException();
        }

        // ToDo: implement this later.
        public List<Tuple<Guid, int, int>> GetByFilterCountForReport()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives a milestones related to specific project and having a specified status.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <param name="milestoneStatus">Needed status of milestones.</param>
        /// <returns>
        /// List of milestones <see cref="DbMilestone"/> related to project with specified Id and having specified status.
        /// </returns>
        public List<DbMilestone> GetProjectMilestonesWithStatus(int projectId, MilestoneStatus milestoneStatus)
        {
            var milestones = GetAll()
                .Where(m => m.ProjectId == projectId && m.Status == milestoneStatus)
                .ToList();

            return milestones;
        }

        /// <summary>
        /// Receives needed amount of upcoming milestones with specified offset.
        /// </summary>
        /// <param name="offset">An offset (which amount of items to skip is needed).</param>
        /// <param name="max">Maximal amount of items to get.</param>
        /// <param name="projectIds">Ids of needed projects.</param>
        /// <returns>A list with needed amount of upcoming milestones <see cref="DbMilestone"/> with specified offset.</returns>
        public List<DbMilestone> GetUpcomingMilestones(int offset, int max, params int[] projectIds)
        {
            var currentDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local).Date;

            var milestonesQuery = GetAll()
                .Include(m => m.Project)
                .Where(m => m.Project.Status == ProjectStatus.Open
                    && m.Deadline >= currentDate
                    && m.Status == MilestoneStatus.Open);

            if (projectIds?.Any() == true)
            {
                var neededProjectIdsAmount = max > default(int)
                    ? max
                    : projectIds.Length;

                var neededProjectIds = projectIds
                    .Take(neededProjectIdsAmount)
                    .ToList();

                milestonesQuery = milestonesQuery
                    .Where(m => neededProjectIds.Contains(m.ProjectId));
            }

            var result = milestonesQuery
                .Skip(offset)
                .Take(max)
                .OrderBy(m => m.Deadline)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives needed amount of overdue milestones with specified offset.
        /// </summary>
        /// <param name="offset">An offset (which amount of items to skip is needed).</param>
        /// <param name="max">Maximal amount of items to get.</param>
        /// <returns>A list with needed amount of overdue milestones <see cref="DbMilestone"/> with specified offset.</returns>
        public List<DbMilestone> GetOverdueMilestones(int offset, int max)
        {
            var now = TenantUtil.DateTimeNow(TimeZoneInfo.Local);
            var yesterday = now.Date.AddDays(-1);

            var result = GetAll()
                .Include(m => m.Project)
                .Where(m => m.Project.Status == ProjectStatus.Open
                            && m.Status != MilestoneStatus.Closed
                            && m.Deadline <= yesterday)
                .Skip(offset)
                .Take(max)
                .OrderBy(m => m.Deadline)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives milestones with specified deadline.
        /// </summary>
        /// <param name="deadline">Needed deadline of milestone.</param>
        /// <returns>A list of milestones <see cref="DbMilestone"/> with specified deadline.</returns>
        public List<DbMilestone> GetMilestonesByDeadline(DateTime deadline)
        {
            var result = GetAll()
                .Where(m => m.Deadline == deadline.Date)
                .ToList();

            return result;
        }
        
        /// <summary>
        /// Receives milestones with specified Ids.
        /// </summary>
        /// <param name="milestoneIds">Ids of needed milestones.</param>
        /// <returns>A list of needed milestones <see cref="DbMilestone"/>.</returns>
        public List<DbMilestone> GetMilestonesByIds(List<int> milestoneIds)
        {
            var result = GetAll()
                .Where(m => milestoneIds.Contains(m.Id))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives milestones for user notifications about deadline.
        /// </summary>
        /// <param name="deadline">Needed deadline.</param>
        /// <returns>A list of milestones <see cref="DbMilestone"/> for user notifications about deadline.</returns>
        public List<DbMilestone> GetInfoForReminder(DateTime deadline)
        {
            var deadlineDate = deadline.Date;

            var result = GetAll()
                .Include(m => m.Project)
                .Where(m => m.Deadline > deadlineDate.AddDays(-1)
                    && m.Deadline < deadlineDate.AddDays(1)
                    && m.Status == MilestoneStatus.Open
                    && m.Project.Status == ProjectStatus.Open
                    && m.IsNotify)
                .ToList();

            return result;
        }

        /// <summary>
        /// Updates an existing milestone.
        /// </summary>
        /// <param name="updatedItem">Updating milestone data.</param>
        /// <returns>Just updated milestone.</returns>
        public override DbMilestone Update(DbMilestone updatedItem)
        {
            var milestone = GetById(updatedItem.Id);

            if (milestone.Deadline.Kind != DateTimeKind.Local)
            {
                milestone.Deadline = TenantUtil.DateTimeFromUtc(TimeZoneInfo.Local, updatedItem.Deadline);
            }

            milestone.ProjectId = updatedItem.ProjectId;
            milestone.Title = updatedItem.Title;
            milestone.CreatorId = updatedItem.CreatorId;
            milestone.CreationDate = updatedItem.CreationDate;
            milestone.LastModificationDate = updatedItem.LastModificationDate;
            milestone.LastEditorId = updatedItem.LastEditorId;
            milestone.Deadline = updatedItem.Deadline;
            milestone.Status = updatedItem.Status;
            milestone.IsNotify = updatedItem.IsNotify;
            milestone.IsKey = updatedItem.IsKey;
            milestone.Description = updatedItem.Description;
            milestone.StatusChangeDate = updatedItem.StatusChangeDate;
            milestone.ResponsibleId = updatedItem.ResponsibleId;

            var result = base.Update(updatedItem);

            return result;
        }

        /// <summary>
        /// Receives maximal date of milestone modification.
        /// </summary>
        /// <returns>Maximal date of milestone modification.</returns>
        public DateTime? GetLastModificationDate()
        {
            var result = GetAll()
                .Max(m => m.LastModificationDate);

            return result;
        }

        /// <summary>
        /// Receives tasks assigned to milestone with specified Id.
        /// </summary>
        /// <param name="milestoneId">Id of needed milestone.</param>
        /// <returns>List of tasks <see cref="TaskData"/> assigned to milestone with specified Id.</returns>
        public List<DbProjectTask> GetMilestoneTasks(int milestoneId)
        {
            var result = GetAll()
                .Include(m => m.Tasks)
                .FirstOrDefault(m => m.Id == milestoneId)
                ?.Tasks;

            return result;
        }
    }
}
