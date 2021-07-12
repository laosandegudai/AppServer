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
    /// An interface of business logic manager responsible for project tasks processing.
    /// </summary>
    public interface IProjectTaskManager
    {
        /// <summary>
        /// Receives a full list of existing tasks.
        /// </summary>
        /// <returns>List of tasks <see cref="ProjectTaskData"/> including all existing tasks.</returns>
        List<ProjectTaskData> GetAll();

        /// <summary>
        /// Receives tasks related to specific project.
        /// </summary>
        /// <param name="projectId">Id of project</param>
        /// <param name="status">Status of tasks.</param>
        /// <param name="participant">Id of participant.</param>
        /// <returns></returns>
        List<ProjectTaskData> GetProjectTasks(int projectId, TaskStatus? status = null, Guid? participant = null);

        /// <summary>
        /// Receives tasks, related to milestone with specified id.
        /// </summary>
        /// <param name="milestoneId">Id of needed milestone.</param>
        /// <returns>List of milestone tasks <see cref="ProjectTaskData"/>.</returns>
        List<ProjectTaskData> GetMilestoneTasks(int milestoneId);

        /// <summary>
        /// Receives task with specified id.
        /// </summary>
        /// <param name="id">Id of needed task.</param>
        /// <returns>Task <see cref="ProjectTaskData"/> having specified id./returns>
        ProjectTaskData GetById(int id);

        /// <summary>
        /// Receives a list of tasks, having specified ids.
        /// </summary>
        /// <param name="ids">Ids of needed tasks.</param>
        /// <returns>List of tasks <see cref="ProjectTaskData"/> having specified ids.</returns>
        List<ProjectTaskData> GetByIds(List<int> ids);

        /// <summary>
        /// Makes a check about task with specified id existence.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>true - if task exists, otherwise - false.</returns>
        bool Exists(int taskId);
    }
}
