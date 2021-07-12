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
using ASC.Core.Common.Utils;
using ASC.Projects.Configuration;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.ViewModels;
using ASC.Web.Api.Routing;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Projects.Controllers
{
    public class MilestoneApiController : BaseApiController
    {
        #region Fields and .ctor

        private readonly IMilestoneManager _milestoneManager;

        public MilestoneApiController(ProductEntryPoint productEntryPoint,
            SecurityContext securityContext,
            IMilestoneManager milestoneManager,
            IMapper mapper) : base(productEntryPoint, securityContext, mapper)
        {
            _milestoneManager = milestoneManager.NotNull(nameof(milestoneManager));
        }

        #endregion Fields and .ctor
        
        ///<summary>
        /// Receives the list of all upcoming milestones within all portal projects.
        ///</summary>
        ///<returns>List of milestones <see cref="MilestoneViewModel"/>.</returns>
        [Read(@"milestone")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MilestoneViewModel>))]
        public IActionResult GetMilestones()
        {
            var result = _milestoneManager
                .GetProjectUpcomingMilestones((int)Count)
                .Select(m => Mapper.Map<MilestoneData, MilestoneViewModel>(m))
                .ToList();

            return Ok(result);
        }

        ///<summary>
        /// Receives the list of all milestones matching the filter with the parameters specified in the request.
        ///</summary>
        ///<param name="projectId">Id of project.</param>
        ///<param name="tag">Project tag.</param>
        ///<param name="status">Determines ability of milestone be open or closed.</param>
        ///<param name="deadlineStart">Minimal value of task deadline.</param>
        ///<param name="deadlineStop">Maximal value of task deadline.</param>
        ///<param name="taskResponsible">Id of responsible for the task in milestone.</param>
        ///<param name="lastId">Id of last milestone</param>
        ///<param name="myProjects">Determines need of my Projects milestones receiving.</param>
        ///<param name="milestoneResponsible">Id of responsible for the milestone.</param>
        ///<returns>List of all milestones matching the filter with the parameters specified in the request.</returns>
        [Read(@"milestone/filter")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MilestoneViewModel>))]
        public IActionResult GetMilestonesByFilter(
            int projectId,
            int tag,
            MilestoneStatus? status,
            DateTime deadlineStart,
            DateTime deadlineStop,
            Guid? taskResponsible,
            int lastId,
            bool myProjects,
            Guid milestoneResponsible)
        {
            // ToDo: implement this later.
            return NotFound();
        }

        ///<summary>
        /// Receives the list of all overdue milestones in the portal projects.
        ///</summary>
        ///<returns>List of an overdue milestones <see cref="MilestoneViewModel"/>.</returns>
        [Read(@"milestone/late")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MilestoneViewModel>))]
        public IActionResult GetLateMilestones()
        {
            var result = _milestoneManager
                .GetLateMilestones((int)Count)
                .Select(m => Mapper.Map<MilestoneData, MilestoneViewModel>(m))
                .ToList();


            return Ok(result);
        }

        ///<summary>
        /// Returns the list of all milestones due on the date specified in the request.
        ///</summary>
        /// <param name="deadline">Needed deadline.</param>
        ///<returns>List of milestones <see cref="MilestoneViewModel"/> with specified deadline.</returns>
        [Read(@"milestone/{year}/{month}/{day}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MilestoneViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetMilestonesWithDeadline(DateTime deadline)
        {
            if (deadline == DateTime.MinValue || deadline == DateTime.MaxValue)
            {
                return BadRequest("Incorrect deadline date");
            }

            var milestonesWithDeadline = _milestoneManager
                .GetMilestonesWithDeadline(deadline)
                .Select(m => Mapper.Map<MilestoneData, MilestoneViewModel>(m))
                .ToList();

            return Ok(milestonesWithDeadline);
        }
        
        ///<summary>
        /// Returns the list of all milestones due in the month specified in the request
        ///</summary>
        ///<param name="year">Deadline year</param>
        ///<param name="month">Deadline month</param>
        ///<returns>List of milestones</returns>
        [Read(@"milestone/{year}/{month}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MilestoneViewModel>))]
        public IActionResult GetMilestonesByDeadLineMonth(int year, int month)
        {
            var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var result = _milestoneManager
                .GetMilestonesWithDeadline(date)
                .Select(m => Mapper.Map<MilestoneData, MilestoneViewModel>(m))
                .ToList();

            return Ok(result);
        }

        ///<summary>
        /// Returns the list with the detailed information about the milestone with the ID specified in the request
        ///</summary>
        ///<param name="id">Milestone ID</param>
        ///<returns>Milestone</returns>
        [Read(@"milestone/{id:[0-9]+}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MilestoneViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMilestoneById(int id)
        {
            if (id <= default(int))
            {
                return BadRequest("Milestone ID must be a positive number.");
            }

            var milestone = _milestoneManager.GetById(id);

            if (milestone == null)
            {
                return NotFound($"A milestone with ID = {id} does not exists.");
            }

            var result = Mapper.Map<MilestoneData, MilestoneViewModel>(milestone);

            return Ok(result);
        }

        ///<summary>
        /// Returns the list of all tasks within the milestone with the ID specified in the request.
        ///</summary>
        ///<param name="milestoneId">If of needed milestone.</param>
        ///<returns>List of tasks <see cref="TaskViewModel"/> assigned to milestone.</returns>
        [Read(@"milestone/{id:[0-9]+}/task")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TaskViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetMilestoneTasks(int milestoneId)
        {
            if (milestoneId <= default(int))
            {
                return BadRequest("Id of milestone must be positive.");
            }

            var result = _milestoneManager
                .GetMilestoneTasks(milestoneId)
                .Select(m => Mapper.Map<ProjectTaskData, TaskViewModel>(m))
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Updates an existing milestone.
        /// </summary>
        /// <param name="milestone">Updating milestone data.</param>
        /// <returns>Just updated milestone.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MilestoneViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateMilestone(MilestoneViewModel milestone, bool notifyResponsible)
        {
            if (milestone == null)
            {
                return BadRequest("Milestone data must be provided.");
            }

            var milestoneData = Mapper.Map<MilestoneViewModel, MilestoneData>(milestone);

            var updatedMilestone = _milestoneManager.SaveOrUpdate(milestoneData, notifyResponsible);

            var result = Mapper.Map<MilestoneData, MilestoneViewModel>(updatedMilestone);

            return Ok(result);
        }

        ///<summary>
        /// Updates the status of the milestone with the Id specified in the request.
        ///</summary>
        ///<param name="id">Id of milestone.</param>
        ///<param name="status">New status of milestone.</param>
        ///<returns>Just updated milestone.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MilestoneViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Update(@"milestone/{id:[0-9]+}/status")]
        public IActionResult SetMilestoneStatus(int milestoneId, MilestoneStatus? milestoneStatus)
        {
            if (milestoneId <= default(int))
            {
                return BadRequest("Id of milestone must be positive.");
            }

            if (!milestoneStatus.HasValue)
            {
                return BadRequest($"Parameter {nameof(milestoneStatus)} must be specified.");
            }

            var result = _milestoneManager.SetMilestoneStatus(milestoneId, milestoneStatus.Value);

            // ToDo: implement this later.
            //MessageService.Send(Request, MessageAction.MilestoneUpdatedStatus, MessageTarget.Create(milestone.ID), milestone.Project.Title, milestone.Title, LocalizedEnumConverter.ConvertToString(milestone.Status));

            return Ok(result);
        }

        ///<summary>
        /// Deletes milestones with the IDs specified in the request.
        ///</summary>
        ///<param name="ids">Ids of removal milestones.</param>
        ///<returns>Deleted milestones.</returns>
        [Delete(@"milestone")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MilestoneViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteMilestones(List<int> ids)
        {
            if (ids?.Any() == false)
            {
                return BadRequest("Ids of milestones for removal must be specified");
            }

            if (ids!.Any(id => id <= default(int)))
            {
                return BadRequest("Ids of milestones for removal must be a positive.");
            }

            var result = new List<MilestoneViewModel>();

            foreach (var id in ids)
            {
                var removalMilestone = _milestoneManager.GetById(id);

                if (removalMilestone == null)
                {
                    return BadRequest($"A milestone with ID = {id} does not exists");
                }

                result.Add(Mapper.Map<MilestoneData, MilestoneViewModel>(removalMilestone));

                _milestoneManager.Delete(removalMilestone);

                // ToDo: implement this later.
                // var messageTarget = _messageTarget.Create(id);
                //_MessageService.Send(Request, MessageAction.MilestoneDeleted, MessageTarget.Create(milestone.ID), milestone.Project.Title, milestone.Title);
            }

            return Ok(result);
        }
    }
}
