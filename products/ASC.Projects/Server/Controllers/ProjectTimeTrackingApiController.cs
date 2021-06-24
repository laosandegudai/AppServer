using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using ASC.Common;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ASC.Core.Common.Utils;
using ASC.Projects.Configuration;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Validators;
using ASC.Projects.ViewModels;
using ASC.Web.Api.Routing;
using Microsoft.AspNetCore.Http;
using SecurityContext = ASC.Core.SecurityContext;

namespace ASC.Projects.Controllers
{
    //[Scope]
    [DefaultRoute]
    [ApiController]
    public class ProjectTimeTrackingApiController : ProjectApiControllerBase
    {
        private readonly IProjectManager _projectManager;

        private readonly IProjectTaskManager _projectTaskManager;

        private readonly ITimeTrackingManager _timeTrackingManager;

        private readonly IMapper _mapper;

        public ProjectTimeTrackingApiController(
            IProjectManager projectManager,
            IProjectTaskManager projectTaskManager,
            ITimeTrackingManager timeTrackingManager,
            IMapper mapper,
            ProductEntryPoint productEntryPoint,
            SecurityContext securityContext) : base(productEntryPoint, securityContext)
        {
            _projectManager = projectManager.NotNull(nameof(projectManager));
            _projectTaskManager = projectTaskManager.NotNull(nameof(projectTaskManager));
            _timeTrackingManager = timeTrackingManager.NotNull(nameof(timeTrackingManager));
            _mapper = mapper.NotNull(nameof(mapper));
        }

        ///<summary>
        /// Returns the list with the detailed information about all Time tracking items
        /// that satisfies the filter parameters specified in the request.
        ///</summary>
        ///<short>
        ///Get time spent by filter
        ///</short>
        ///<category>Time</category>
        ///<param name="projectId" optional="true">Id of needed project.</param>
        ///<param name="tagId" optional="true">Id of needed project.</param>
        ///<param name="departmentId" optional="true">Id of needed department.</param>
        ///<param name="participantId" optional="true">Id of needed participant.</param>
        ///<param name="creationIntervalFrom" optional="true">Minimal value of creation time.</param>
        ///<param name="creationIntervalTo" optional="true">Maximal value of creation time.</param>
        ///<param name="lastId">Id of needed last Time Tracking item.</param>
        ///<param name="myProjectsAreNeeded">Determines if we need to get time from 'My projects'.</param>
        ///<param name="myMilestonesAreNeeded">Determines if we need to get time from 'My Milestones'.</param>
        ///<param name="milestoneId" optional="true">Milestone ID</param>
        ///<param name="status" optional="true">Needed payment status of items.</param>
        ///<returns>List of time spent</returns>
        ///<exception cref="ItemNotFoundException"></exception>
        [Read(@"time/filter")]
        public List<TimeTrackingItemViewModel> GetLoggedTimeByFilter(int? projectId,
            bool myProjectsAreNeeded,
            int? milestoneId,
            bool myMilestonesAreNeeded,
            int tagId,
            Guid departmentId,
            Guid participantId,
            DateTime creationIntervalFrom,
            DateTime creationIntervalTo,
            int lastId,
            PaymentStatus? status)
        {
            var filterData = new TimeTrackingItemFilterData
            {
                DepartmentId = departmentId,
                ParticipantId = participantId,
                CreationIntervalFrom = creationIntervalFrom,
                CreationIntervalTo = creationIntervalTo,
                TagId = tagId,
                LastId = lastId,
                AreMyMilestonesNeeded = myMilestonesAreNeeded,
                AreMyProjectsNeeded = myProjectsAreNeeded,
                MilestoneId = milestoneId,
                PaymentStatus = status,
                ProjectId = projectId
            };

            return new List<TimeTrackingItemViewModel>();
        }

        /// <summary>
        /// Returns the list with the detailed information about all the time spent matching the filter parameters specified in the request.
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="myProjectsAreNeeded">Determines if we need to get time from 'my projects'</param>
        /// <param name="milestoneId">Needed Milestone Id.</param>
        /// <param name="myMilestonesAreNeeded">Determines if we need to get time from 'My Milestones'</param>
        /// <param name="tagId">Project tag Id.</param>
        /// <param name="departmentId">Department Id.</param>
        /// <param name="participantId">Participant Id.</param>
        /// <param name="creationIntervalFrom">Minimum value of creation time.</param>
        /// <param name="creationIntervalTo">Maximum value of creation time.</param>
        /// <param name="lastId">Last time spent Id.</param>
        /// <param name="status">Needed payment status of items.</param>
        /// <returns></returns>
        [Read(@"time/filter/total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public List<TimeTrackingItemViewModel> GetTaskTimeByFilter(int? projectId,
            bool myProjectsAreNeeded,
            int? milestoneId,
            bool myMilestonesAreNeeded,
            int? tagId,
            Guid? departmentId,
            Guid? participantId,
            DateTime creationIntervalFrom,
            DateTime creationIntervalTo,
            int lastId,
            PaymentStatus? status)
        {
            var filter = new TimeTrackingItemFilterData
            {
                DepartmentId = departmentId,
                ParticipantId = participantId,
                CreationIntervalFrom = creationIntervalFrom,
                CreationIntervalTo = creationIntervalTo,
                TagId = tagId,
                LastId = lastId,
                AreMyMilestonesNeeded = myMilestonesAreNeeded,
                AreMyProjectsNeeded = myProjectsAreNeeded,
                MilestoneId = milestoneId,
                PaymentStatus = status,
                ProjectId = projectId
            };

            return new List<TimeTrackingItemViewModel>();
        }

        /// <summary>
        /// Returns the list of Time Tracking items logged up for specific task.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>The list of Time Tracking items <see cref="List{TimeTrackingItemViewModel}"/> related to task with specified Id.</returns>
        [HttpGet("task/{taskId}/time")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<TimeTrackingItemViewModel>> GetTaskLoggedItems(int taskId)
        {
            var doesTaskExists = _projectTaskManager.Exists(taskId);

            if (!doesTaskExists)
            {
                return BadRequest($"Task with Id = {taskId} does not exists");
            }

            var loggedItems = _timeTrackingManager
                .GetTaskTimeTrackingItems(taskId)
                .Select(li => _mapper.Map<TimeTrackingItemData, TimeTrackingItemViewModel>(li))
                .ToList();

            return Ok(loggedItems);
        }

        /// <summary>
        /// Creates a new Time Tracking item for specified task.
        /// </summary>
        /// <param name="data">Logged item data.</param>
        /// <returns>Just created Time tracking item.</returns>
        [Create(@"task/{taskId}/time")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TimeTrackingItemViewModel> LogTime(TimeTrackingItemViewModel data)
        {
            var validationResult = new TimeTrackingItemValidator()
                .Validate(data);

            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join(", ", validationResult.Errors));
            }

            var doesTaskExists = _projectTaskManager.Exists(data.RelatedTaskId);

            if (!doesTaskExists)
            {
                return BadRequest($"A task with Id = {data.RelatedTaskId} does not exists");
            }

            var doesProjectExists = _projectManager.Exists(data.RelatedProjectId);

            if (!doesProjectExists)
            {
                return BadRequest($"A project with Id = {data.RelatedProjectId} does not exists");
            }

            var loggedTimeData = _mapper.Map<TimeTrackingItemViewModel, TimeTrackingItemData>(data);

            _timeTrackingManager.LogTime(loggedTimeData);

            var result = _mapper.Map<TimeTrackingItemData, TimeTrackingItemViewModel>(loggedTimeData);

            return result;
        }

        ///<summary>
        /// Updates an existing Time Tracking item for the selected task with the time parameters specified in the model.
        ///</summary>
        /// <param name="data">Updating Time Tracking item data.</param>
        ///<returns>Just updated Time tracking Item</returns>
        [Update(@"time/{timeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TimeTrackingItemViewModel> UpdateLoggedTime(TimeTrackingItemViewModel data)
        {
            var validationResult = new TimeTrackingItemValidator()
                .Validate(data);

            if (!validationResult.IsValid)
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(string.Join(", ", validationResult.Errors));
                }
            }

            var doesTimeTrackingItemExists = _timeTrackingManager
                .Exists(data.Id);

            if (!doesTimeTrackingItemExists)
            {
                return BadRequest($"A Time Tracking item with ID = {data.Id} does not exists");
            }

            var timeTrackingData = _mapper.Map<TimeTrackingItemViewModel, TimeTrackingItemData>(data);

            var updatedItem = _timeTrackingManager.UpdateLoggedTime(timeTrackingData);

            var result = _mapper.Map<TimeTrackingItemData, TimeTrackingItemViewModel>(updatedItem);

            return result;
        }

        ///<summary>
        /// Updates payment status of needed Time Tracking items.
        ///</summary>
        ///<param name="itemIds">Ids of needed Time Tracking items.</param>
        ///<param name="newStatus">New status.</param>
        ///<returns>A list <see cref="List{TimeTrackingItemViewModel}"/> of Time Tracking items with just updated payment status.</returns>
        [Update(@"time/times/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<TimeTrackingItemViewModel>> UpdateLoggedItemsByPaymentStatus(List<int> itemIds, PaymentStatus newStatus)
        {
            if (itemIds.Count == default)
            {
                return BadRequest("A list of updating item ids must be provided.");
            }

            var result = new List<TimeTrackingItemViewModel>();

            foreach (var itemId in itemIds)
            {
                TimeTrackingItemData updatingResult;

                try
                {
                    updatingResult = _timeTrackingManager.ChangePaymentStatus(itemId, newStatus);
                }
                catch (SecurityException)
                {
                    return Forbidden("Access is denied.");
                }

                result.Add(_mapper.Map<TimeTrackingItemData, TimeTrackingItemViewModel>(updatingResult));
            }

            return result;
        }

        /// <summary>
        /// Removes needed Time Tracking Items.
        /// </summary>
        /// <param name="itemIds">Ids of removal Time Tracking items.</param>
        /// <returns>List <see cref="List{TimeTrackingItemViewModel}"/> of removed Time Tracking items.</returns>
        [Delete(@"time/times/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<TimeTrackingItemViewModel>> RemoveTaskLoggedTimes(List<int> itemIds)
        {
            if (itemIds?.Any() == false)
            {
                return BadRequest("A list of removal items Ids must be provided.");
            }

            var result = _timeTrackingManager.RemoveLoggedTimes(itemIds)
                .Select(ttd => _mapper.Map<TimeTrackingItemData, TimeTrackingItemViewModel>(ttd))
                .ToList();

            return result;
        }
    }
}
