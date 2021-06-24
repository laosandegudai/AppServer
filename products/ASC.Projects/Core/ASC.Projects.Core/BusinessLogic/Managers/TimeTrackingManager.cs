using System;
using System.Collections.Generic;
using System.Linq;
using ASC.Common.Web;
using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Core.Tenants;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    public class TimeTrackingManager : ITimeTrackingManager
    {
        /// <summary>
        /// An instance of Dao working with Time Tracking items.
        /// </summary>
        private readonly ITimeTrackingItemRepository _timeTrackingItemRepository;

        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// An instance of tenant util.
        /// </summary>
        private readonly TenantUtil _tenantUtil;

        /// <summary>
        /// An instance of security context.
        /// </summary>
        private readonly SecurityContext _securityContext;

        public TimeTrackingManager(ITimeTrackingItemRepository timeTrackingItemRepository,
            IMapper mapper,
            TenantUtil tenantUtil,
            SecurityContext securityContext)
        {
            _timeTrackingItemRepository = timeTrackingItemRepository.NotNull(nameof(timeTrackingItemRepository));
            _mapper = mapper.NotNull(nameof(mapper));
            _tenantUtil = tenantUtil.NotNull(nameof(tenantUtil));
            _securityContext = securityContext.NotNull(nameof(securityContext));
        }

        /// <summary>
        /// Returns a fully-filled Time Tracking items that satisfied a provided filter.
        /// </summary>
        /// <param name="filter">Filter data.</param>
        /// <returns>A list of items <see cref="List{TimeTrackingItemData}"/> that satisfied a provided filter.</returns>
        /// ToDo: implement this later.
        public List<TimeTrackingItemData> GetLoggedTimeByFilter(TimeTrackingItemFilterData filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns amount of items that satisfied a provided filter.
        /// </summary>
        /// <param name="filter">Filter data.</param>
        /// <returns>Amount of items what satisfied a provided filter.</returns>
        /// ToDo: implement this later.
        public decimal GetTotalCountByFilter(TimeTrackingItemFilterData filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives a list of Time Tracking items related to task.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>A list of Time tracking items <see cref="List{TimeTrackingItemData}"/> related to task with specified Id.</returns>
        public List<TimeTrackingItemData> GetTaskTimeTrackingItems(int taskId)
        {
            var timeTrackingItems = _timeTrackingItemRepository
                .GetTaskTimeTrackingItems(taskId)
                .Select(tti => _mapper.Map<DbTimeTrackingItem, TimeTrackingItemData>(tti))
                .ToList();

            return timeTrackingItems;
        }

        /// <summary>
        /// Creates a new Time Tracking item with logged time data.
        /// </summary>
        /// <param name="loggedItemData">Logged time data.</param>
        /// <returns>Just created Time tracking item data.</returns>
        public TimeTrackingItemData LogTime(TimeTrackingItemData loggedItemData)
        {
            loggedItemData.NotNull(nameof(loggedItemData));

            var newItem = _mapper.Map<TimeTrackingItemData, DbTimeTrackingItem>(loggedItemData);

            newItem.CreatorId = _securityContext.CurrentAccount.ID;
            newItem.TrackingDate = _tenantUtil.DateTimeToUtc(newItem.TrackingDate);
            newItem.StatusChangeDate = _tenantUtil.DateTimeToUtc(newItem.StatusChangeDate);

            _timeTrackingItemRepository.Create(newItem);

            // ToDo: implement this later.
            //MessageService.Send(Request, MessageAction.TaskTimeCreated, MessageTarget.Create(ts.ID), task.Project.Title, task.Title, ts.Note);

            var result = _mapper.Map<DbTimeTrackingItem, TimeTrackingItemData>(newItem);

            return result;
        }

        /// <summary>
        /// Updates an existing Time Tracking item.
        /// </summary>
        /// <param name="updatedItemData">Updated logged time data.</param>
        /// <returns>Just updated Time Tracking item data.</returns>
        public TimeTrackingItemData UpdateLoggedTime(TimeTrackingItemData updatedItemData)
        {
            updatedItemData.NotNull(nameof(updatedItemData));

            var entity = _mapper.Map<TimeTrackingItemData, DbTimeTrackingItem>(updatedItemData);

            _timeTrackingItemRepository.Update(entity);

            // ToDo: implement this later.
            //_projectSecurityManager.DemandEdit(updatedItemData);

            // ToDo: implement this later.
            //MessageService.Send(Request, MessageAction.TaskTimeUpdated, MessageTarget.Create(time.ID), time.Task.Project.Title, time.Task.Title, time.Note);

            return updatedItemData;
        }

        /// <summary>
        /// Updates a payment status of existing Time Tracking item.
        /// </summary>
        /// <param name="itemId">Id of needed time tracking item.</param>
        /// <param name="newStatus">A new status of updating item <see cref="TimeTrackingItemData"/>.</param>
        /// <returns>A Time Tracking item <see cref="TimeTrackingItemData"/> with just updated payment status.</returns>
        public TimeTrackingItemData ChangePaymentStatus(int itemId, PaymentStatus newStatus)
        {
            itemId.IsPositive(nameof(itemId));

            var item = _timeTrackingItemRepository.GetById(itemId);

            if (item == null)
            {
                return null;
            }

            // ToDo: implement this later.
            //if (!ProjectSecurity.CanEditPaymentStatus(timeSpend)) throw new SecurityException("Access denied.");
            //ProjectSecurity.DemandEdit(timeSpend);

            if (item.PaymentStatus != newStatus)
            {
                item.PaymentStatus = newStatus;
                item.StatusChangeDate = DateTime.UtcNow;

                _timeTrackingItemRepository.Update(item);
            }

            var result = _mapper.Map<DbTimeTrackingItem, TimeTrackingItemData>(item);

            return result;
        }

        /// <summary>
        /// Removes needed Time Tracking items.
        /// </summary>
        /// <param name="itemIds">Removal items Ids.</param>
        /// <returns>List of just removed Time Tracking items <see cref="List{TimeTrackingItemData}"/>.</returns>

        public List<TimeTrackingItemData> RemoveLoggedTimes(List<int> itemIds)
        {
            var result = new List<TimeTrackingItemData>();

            if (itemIds?.Any() == false)
            {
                return null;
            }

            foreach (var itemId in itemIds!.Distinct())
            {
                var removalItem = _timeTrackingItemRepository.GetById(itemId);

                if (removalItem == null)
                {
                    throw new ItemNotFoundException($"A Time Tracking item with Id = {itemId} does not exists");
                }

                var itemData = _mapper.Map<DbTimeTrackingItem, TimeTrackingItemData>(removalItem);

                // ToDo: implement this later.
                //_projectSecurityManager.DemandDelete(itemData);

                _timeTrackingItemRepository.DeleteById(itemId);

                result.Add(itemData);
            }

            return result;
        }

        /// <summary>
        /// Makes a check about Time Tracking item existence.
        /// </summary>
        /// <param name="timeTrackingItemId">Id of needed Time Tracking item.</param>
        /// <returns>True - if Time Tracking item exists, otherwise - false.</returns>
        public bool Exists(int timeTrackingItemId)
        {
            timeTrackingItemId.IsPositive(nameof(timeTrackingItemId));

            var result = _timeTrackingItemRepository.Exists(timeTrackingItemId);

            return result;
        }
    }
}
