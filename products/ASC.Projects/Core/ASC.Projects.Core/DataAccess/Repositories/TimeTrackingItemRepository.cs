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
    /// Repository working with 'Time Tracking Item' entity.
    /// </summary>
    internal class TimeTrackingItemRepository : BaseTenantRepository<DbTimeTrackingItem, int>, ITimeTrackingItemRepository
    {
        public TimeTrackingItemRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        /// <summary>
        /// Receives time tracking items of specific project.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <returns>List of time tracking items <see cref="DbTimeTrackingItem"/>.</returns>
        public List<DbTimeTrackingItem> GetByProject(int projectId)
        {
            var loggedItems = GetAll()
                .Where(tti => tti.ProjectId == projectId)
                .OrderByDescending(tti => tti.TrackingDate)
                .ToList();

            return loggedItems;
        }

        /// <summary>
        /// Receives time tracking items of specific task.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>List of time tracking items <see cref="DbTimeTrackingItem"/>, logged to task with specified Id.</returns>
        public List<DbTimeTrackingItem> GetTaskTimeTrackingItems(int taskId)
        {
            var result = GetAll()
                .Where(t => t.RelativeTaskId == taskId)
                .OrderByDescending(t => t.TrackingDate)
                .ToList();

            return result;
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="updatedItem">Updating item.</param>
        /// <returns>Just updated item.</returns>
        public override DbTimeTrackingItem Update(DbTimeTrackingItem updatedItem)
        {
            var entity = GetById(updatedItem.Id);

            entity.Note = updatedItem.Note;
            entity.TrackingDate = updatedItem.TrackingDate;
            entity.Hours = updatedItem.Hours;
            entity.RelativeTaskId = updatedItem.RelativeTaskId;
            entity.PersonId = updatedItem.PersonId;
            entity.ProjectId = updatedItem.ProjectId;
            entity.CreationDate = updatedItem.CreationDate;
            entity.CreatorId = updatedItem.CreatorId;
            entity.PaymentStatus = updatedItem.PaymentStatus;
            entity.StatusChangeDate = updatedItem.StatusChangeDate;

            base.Update(updatedItem);

            return entity;
        }
    }
}
