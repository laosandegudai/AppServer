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
    /// An interface of repository working with subtasks.
    /// </summary>
    public interface ISubtaskRepository : IRepository<DbProjectSubtask, int>
    {
        /// <summary>
        /// Receives subtask of task with specified id.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>List of subtasks <see cref="DbProjectTask"/> of task with specified id.</returns>
        List<DbProjectSubtask> GetSubtasksOfTask(int taskId);

        /// <summary>
        /// Receives subtasks with specified ids.
        /// </summary>
        /// <param name="ids">Ids of needed subtasks.</param>
        /// <returns>List of subtasks <see cref="DbProjectTask"/> with specified ids.</returns>
        List<DbProjectSubtask> GetByIds(List<int> ids);

        /// <summary>
        /// Receives an updates of subtasks for specific interval.
        /// </summary>
        /// <param name="from">Beginning of interval.</param>
        /// <param name="to">End of interval.</param>
        /// <returns>List of subtasks <see cref="DbProjectTask"/> with updates.</returns>
        List<DbProjectSubtask> GetUpdates(DateTime from, DateTime to);

        /// <summary>
        /// Receives subtasks which person with specified id responsible for, having specific status.
        /// </summary>
        /// <param name="responsibleId">Id of responsible.</param>
        /// <param name="status">Needed status.</param>
        /// <returns>List of subtasks <see cref="DbProjectSubtask"/> which person with specified id responsible for, having specific status.</returns>
        List<DbProjectSubtask> GetSubtasksOfResponsible(Guid responsibleId, TaskStatus? status = null);

        /// <summary>
        /// Receives an amount of subtasks of specific task having one of specified statuses.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <param name="statuses">Needed statuses.</param>
        /// <returns>Amount of subtasks of specific task having one of specified statuses</returns>
        int GetTaskSubtasksCountInStatuses(int taskId, params TaskStatus[] statuses);

        /// <summary>
        /// Closes all subtasks of task.
        /// </summary>
        /// <param name="task">Task, which subtasks should be closed.</param>
        void CloseAllSubtaskOfTask(DbProjectTask task);
    }
}
