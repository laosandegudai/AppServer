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
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// An interface of business logic manager responsible for project subtasks processing.
    /// </summary>
    public interface ISubtaskManager
    {
        /// <summary>
        /// Receives amount of subtask of task with specified Id and having one of specified statuses.
        /// </summary>
        /// <param name="taskId">Id of the task.</param>
        /// <param name="statuses">Needed statuses.</param>
        /// <returns>Amount of subtasks related to task with specified Id and having one of specified statuses.</returns>
        int GetTaskSubtasksCount(int taskId, params TaskStatus[] statuses);

        /// <summary>
        /// Receives amount of subtasks, related to task with specified Id.
        /// </summary>
        /// <param name="taskId">Id of the task.</param>
        /// <returns>Amount of subtasks related to task with specified Id.</returns>
        int GetTaskSubtasksCount(int taskId);

        /// <summary>
        /// Receives subtask having specified Id.
        /// </summary>
        /// <param name="id">Id of subtask.</param>
        /// <returns>Subtask <see cref="ProjectSubtaskData"/> with specified Id.</returns>
        ProjectSubtaskData GetById(int id);

        /// <summary>
        /// Changes status of subtask.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="subtask"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        ProjectSubtaskData ChangeStatus(ProjectTaskData task, ProjectSubtaskData subtask, TaskStatus newStatus);

        /// <summary>
        /// Creates or updates subtask.
        /// </summary>
        /// <param name="subtask">Subtask data for creation or update.</param>
        /// <param name="task">Parent task data.</param>
        /// <returns>Just updated subtask <see cref="ProjectSubtaskData"/>.</returns>
        ProjectSubtaskData SaveOrUpdate(ProjectSubtaskData subtask, ProjectTaskData task);

        /// <summary>
        /// Creates a copy of subtask.
        /// </summary>
        /// <param name="source">Donor subtask.</param>
        /// <param name="task">Task data.</param>
        /// <param name="team">Team data.</param>
        /// <returns>A copy of provided donor subtask <see cref="ProjectSubtaskData"/>.</returns>
        ProjectSubtaskData Copy(ProjectSubtaskData source, ProjectTaskData task, List<ParticipantData> team);

        /// <summary>
        /// Deletes subtask.
        /// </summary>
        /// <param name="subtask">Subtask data.</param>
        /// <param name="task">Related task data.</param>
        void Delete(ProjectSubtaskData subtask, ProjectTaskData task);
    }
}
