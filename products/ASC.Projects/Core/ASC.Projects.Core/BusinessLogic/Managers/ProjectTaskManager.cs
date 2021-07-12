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
using ASC.Core.Common.Utils;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.BusinessLogic.Security;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    /// <summary>
    /// Business logic manager responsible for project tasks processing.
    /// </summary>
    public class ProjectTaskManager : IProjectTaskManager
    {
        #region Fields and .ctor

        private readonly ITaskRepository _taskRepository;

        private readonly IMapper _mapper;

        private readonly SecurityManager _securityManager;

        public ProjectTaskManager(ITaskRepository taskRepository,
            IMapper mapper,
            SecurityManager securityManager)
        {
            _taskRepository = taskRepository.NotNull(nameof(taskRepository));
            _mapper = mapper.NotNull(nameof(mapper));
            _securityManager = securityManager.NotNull(nameof(securityManager));
        }

        #endregion Fields and .ctor

        /// <summary>
        /// Receives a full list of existing tasks.
        /// </summary>
        /// <returns>List of tasks <see cref="ProjectTaskData"/> including all existing tasks.</returns>
        public List<ProjectTaskData> GetAll()
        {
            var result = _taskRepository.GetAll()
                .Select(t => _mapper.Map<DbProjectTask, ProjectTaskData>(t))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives tasks related to specific project.
        /// </summary>
        /// <param name="projectId">Id of project</param>
        /// <param name="status">Status of tasks.</param>
        /// <param name="participant">Id of participant.</param>
        /// <returns></returns>
        public List<ProjectTaskData> GetProjectTasks(int projectId, TaskStatus? status = null, Guid? participant = null)
        {
            var result = _taskRepository
                .GetProjectTasks(projectId, status, participant)
                .Select(t => _mapper.Map<DbProjectTask, ProjectTaskData>(t))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives tasks, related to milestone with specified id.
        /// </summary>
        /// <param name="milestoneId">Id of needed milestone.</param>
        /// <returns>List of milestone tasks <see cref="ProjectTaskData"/>.</returns>
        public List<ProjectTaskData> GetMilestoneTasks(int milestoneId)
        {
            var result = _taskRepository
                .GetMilestoneTasks(milestoneId)
                .Select(mt => _mapper.Map<DbProjectTask, ProjectTaskData>(mt))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives task with specified id.
        /// </summary>
        /// <param name="id">Id of needed task.</param>
        /// <returns>Task <see cref="ProjectTaskData"/> having specified id./returns>
        public ProjectTaskData GetById(int id)
        {
            var task = _taskRepository.GetById(id);

            var result = _mapper.Map<DbProjectTask, ProjectTaskData>(task);

            return result;
        }

        /// <summary>
        /// Receives a list of tasks, having specified ids.
        /// </summary>
        /// <param name="ids">Ids of needed tasks.</param>
        /// <returns>List of tasks <see cref="ProjectTaskData"/> having specified ids.</returns>
        public List<ProjectTaskData> GetByIds(List<int> ids)
        {
            if (ids?.Any() == false)
            {
                return new List<ProjectTaskData>();
            }
            
            var result = _taskRepository
                .GetByIds(ids)
                .Select(t => _mapper.Map<DbProjectTask, ProjectTaskData>(t))
                .ToList();

            return result;
        }

        /// Makes a check about task with specified id existence.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>true - if task exists, otherwise - false.</returns>
        public bool Exists(int taskId)
        {
            taskId.IsPositive(nameof(taskId));

            var doesTaskExists = _taskRepository.Exists(taskId);

            return doesTaskExists;
        }

        /// <summary>
        /// Determines availability of task data reading.
        /// </summary>
        /// <param name="task">Task to read.</param>
        /// <returns>true - if user can read task data, otherwise - false.</returns>
        private bool CanRead(ProjectTaskData task)
        {
            var result = _securityManager.CanRead(task);

            return result;
        }
    }
}
