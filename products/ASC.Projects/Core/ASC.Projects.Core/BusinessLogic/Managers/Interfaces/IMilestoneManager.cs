using System;
using System.Collections.Generic;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// An interface of business logic manager working with milestones.
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
        /// <returns>List of tasks <see cref="TaskData"/> assigned to milestone with specified Id.</returns>
        List<TaskData> GetMilestoneTasks(int milestoneId);

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
