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
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// An interface of repository working with <see cref="DbMilestone"/> entity.
    /// </summary>
    public interface IMilestoneRepository : IRepository<DbMilestone, int>
    {
        /// <summary>
        /// Receives a milestones related to specific project.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <returns>List of milestones <see cref="DbMilestone"/> related to project with specified Id.</returns>
        List<DbMilestone> GetProjectMilestones(int projectId);

        /// <summary>
        /// Receives a milestones related to specific project and having a specified status.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <param name="milestoneStatus">Needed status of milestones.</param>
        /// <returns>
        /// List of milestones <see cref="DbMilestone"/> related to project with specified Id and having specified status.
        /// </returns>
        List<DbMilestone> GetProjectMilestonesWithStatus(int projectId, MilestoneStatus milestoneStatus);

        /// <summary>
        /// Receives needed amount of upcoming milestones with specified offset.
        /// </summary>
        /// <param name="offset">An offset (which amount of items to skip is needed).</param>
        /// <param name="max">Maximal amount of items to get.</param>
        /// <param name="projectIds">Ids of needed projects.</param>
        /// <returns>A list with needed amount of upcoming milestones <see cref="DbMilestone"/> with specified offset.</returns>
        List<DbMilestone> GetUpcomingMilestones(int offset, int max, params int[] projectIds);

        /// <summary>
        /// Receives needed amount of overdue milestones with specified offset.
        /// </summary>
        /// <param name="offset">An offset (which amount of items to skip is needed).</param>
        /// <param name="max">Maximal amount of items to get.</param>
        /// <returns>A list with needed amount of overdue milestones <see cref="DbMilestone"/> with specified offset.</returns>
        List<DbMilestone> GetOverdueMilestones(int offset, int max);

        /// <summary>
        /// Receives milestones with specified deadline.
        /// </summary>
        /// <param name="deadline">Needed deadline of milestone.</param>
        /// <returns>A list of milestones <see cref="DbMilestone"/> with specified deadline.</returns>
        List<DbMilestone> GetMilestonesByDeadline(DateTime deadline);

        /// <summary>
        /// Receives tasks assigned to milestone with specified Id.
        /// </summary>
        /// <param name="milestoneId">Id of needed milestone.</param>
        /// <returns>List of tasks <see cref="DbProjectTask"/> assigned to milestone with specified Id.</returns>
        List<DbProjectTask> GetMilestoneTasks(int milestoneId);

        /// <summary>
        /// Receives milestones with specified Ids.
        /// </summary>
        /// <param name="milestoneIds">Ids of needed milestones.</param>
        /// <returns>A list of needed milestones <see cref="DbMilestone"/>.</returns>
        List<DbMilestone> GetMilestonesByIds(List<int> milestoneIds);

        /// <summary>
        /// Receives milestones for user notifications about deadline.
        /// </summary>
        /// <param name="deadline">Needed deadline.</param>
        /// <returns>A list of milestones <see cref="DbMilestone"/> for user notifications about deadline.</returns>
        List<DbMilestone> GetInfoForReminder(DateTime deadline);

        /// <summary>
        /// Receives maximal date of milestone modification.
        /// </summary>
        /// <returns>Maximal date of milestone modification.</returns>
        DateTime? GetLastModificationDate();
    }
}
