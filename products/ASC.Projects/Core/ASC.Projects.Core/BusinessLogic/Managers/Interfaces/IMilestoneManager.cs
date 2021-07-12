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
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// An interface of business logic manager responsible for milestones processing.
    /// </summary>
    public interface IMilestoneManager
    {
        /// <summary>
        /// Receives a milestone with specified Id.
        /// </summary>
        /// <param name="id">Id of needed milestone.</param>
        /// <returns>Milestone <see cref="MilestoneData"/> with specified Id.</returns>
        MilestoneData GetById(int id);

        /// <summary>
        /// Receives all existing milestones.
        /// </summary>
        /// <returns>List of all existing milestones <see cref="MilestoneData"/>.</returns>
        List<MilestoneData> GetAll();

        /// <summary>
        /// Receives all existing milestones which are satisfies specified filter.
        /// </summary>
        /// <returns>List of filtered milestones <see cref="MilestoneData"/>.</returns>
        List<MilestoneData> GetByFilter();
        
        /// <summary>
        /// Calculates amount of milestones which are satisfies specified filter.
        /// </summary>
        /// <returns>Amount of filtered milestones <see cref="int"/>.</returns>
        int GetByFilterCount();

        /// <summary>
        /// Calculates amount of milestones which are satisfies specified filter. for report.
        /// </summary>
        /// <returns>Amount of filtered milestones <see cref="int"/>.</returns>
        List<Tuple<Guid, int, int>> GetByFilterCountForReport();

        /// <summary>
        /// Receives a list of milestones, related to project with specified Id.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <returns>List of milestones <see cref="MilestoneData"/> related to project with specified Id.</returns>
        List<MilestoneData> GetProjectMilestones(int projectId);

        /// <summary>
        /// Receives a list of milestones, which are related to project with specified Id and having a specific status.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <param name="milestoneStatus">Status of needed milestones.</param>
        /// <returns>List of milestones, which are related to specific project and having a specific status.</returns>
        List<MilestoneData> GetProjectMilestonesWithStatus(int projectId, MilestoneStatus milestoneStatus);

        /// <summary>
        /// Receives an upcoming milestones.
        /// </summary>
        /// <param name="max">Maximal amount of items at result.</param>
        /// <param name="projectIds">Project ids.</param>
        /// <returns>List of upcoming milestones <see cref="MilestoneData"/> of projects with specified ids.</returns>
        List<MilestoneData> GetProjectUpcomingMilestones(int max, params int[] projectIds);

        /// <summary>
        /// Receives a missed milestones.
        /// </summary>
        /// <param name="max">Maximal amount of items at result.</param>
        /// <returns>List of missed milestones <see cref="MilestoneData"/>.</returns>
        List<MilestoneData> GetLateMilestones(int max);

        /// <summary>
        /// Receives milestones having specified deadline.
        /// </summary>
        /// <param name="deadline">Date of supposed deadline.</param>
        /// <returns>List of milestones <see cref="MilestoneData"/> having specified deadline.</returns>
        List<MilestoneData> GetMilestonesWithDeadline(DateTime deadline);

        /// <summary>
        /// Receives tasks assigned to milestone with specified Id.
        /// </summary>
        /// <param name="milestoneId">Id of needed milestone.</param>
        /// <returns>List of tasks <see cref="ProjectTaskData"/> assigned to milestone with specified Id.</returns>
        List<ProjectTaskData> GetMilestoneTasks(int milestoneId);

        /// <summary>
        /// Creates new milestone or updates an existing milestone.
        /// </summary>
        /// <param name="milestone">Milestone.</param>
        /// <returns>Just created or updated milestone <see cref="MilestoneData"/>.</returns>
        MilestoneData SaveOrUpdate(MilestoneData milestone);

        /// <summary>
        /// Creates new milestone or updates an existing milestone.
        /// </summary>
        /// <param name="milestone">Milestone.</param>
        /// <param name="isNotificationForResponsibleNeeded">Determines need of sending notifications after creation or update.</param>
        /// <returns>Just created or updated milestone <see cref="MilestoneData"/>.</returns>
        MilestoneData SaveOrUpdate(MilestoneData milestone, bool isNotificationForResponsibleNeeded);

        /// <summary>
        /// Changes milestone status.
        /// </summary>
        /// <param name="milestone">Updating milestone Id.</param>
        /// <param name="newStatus">New status for milestone.</param>
        /// <returns>Just updated milestone <see cref="MilestoneData"/>.</returns>
        MilestoneData SetMilestoneStatus(int milestoneId, MilestoneStatus newStatus);

        /// <summary>
        /// Removes an existing milestone.
        /// </summary>
        /// <param name="milestone">Removal milestone.</param>
        void Delete(MilestoneData milestone);
    }
}
