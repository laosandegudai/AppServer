using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Managers.Interfaces
{
    public interface ITimeTrackingManager
    {
        /// <summary>
        /// Returns a fully-filled Time Tracking items that satisfied a provided filter.
        /// </summary>
        /// <param name="filter">Filter data.</param>
        /// <returns>A list of items <see cref="List{TimeTrackingItemData}"/> that satisfied a provided filter.</returns>
        List<TimeTrackingItemData> GetLoggedTimeByFilter(TimeTrackingItemFilterData filter);

        /// <summary>
        /// Returns amount of items that satisfied a provided filter.
        /// </summary>
        /// <param name="filter">Filter data.</param>
        /// <returns>Amount of items what satisfied a provided filter.</returns>
        decimal GetTotalCountByFilter(TimeTrackingItemFilterData filter);

        /// <summary>
        /// Receives a list of Time Tracking items related to task.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>A list of Time tracking items <see cref="List{TimeTrackingItemData}"/> related to task with specified Id.</returns>
        List<TimeTrackingItemData> GetTaskTimeTrackingItems(int taskId);

        /// <summary>
        /// Creates a new Time Tracking item with logged time data.
        /// </summary>
        /// <param name="loggedItemData">Logged time data.</param>
        /// <returns>Just created Time tracking item data <see cref="TimeTrackingItemData"/>.</returns>
        TimeTrackingItemData LogTime(TimeTrackingItemData loggedItemData);

        /// <summary>
        /// Updates an existing Time Tracking item.
        /// </summary>
        /// <param name="updatedItemData">Updated logged time data.</param>
        /// <returns>Just updated Time Tracking item data <see cref="TimeTrackingItemData"/>.</returns>
        TimeTrackingItemData UpdateLoggedTime(TimeTrackingItemData updatedItemData);

        /// <summary>
        /// Updates a payment status of existing Time Tracking item.
        /// </summary>
        /// <param name="itemId">Id of needed time tracking item.</param>
        /// <param name="newStatus">A new status of updating item <see cref="TimeTrackingItemData"/>.</param>
        /// <returns>A Time Tracking item <see cref="TimeTrackingItemData"/> with just updated payment status.</returns>
        TimeTrackingItemData ChangePaymentStatus(int itemId, PaymentStatus newStatus);

        /// <summary>
        /// Removes needed Time Tracking items.
        /// </summary>
        /// <param name="itemIds">Removal items Ids.</param>
        /// <returns>List of just removed Time Tracking items <see cref="List{TimeTrackingItemData}"/>.</returns>
        List<TimeTrackingItemData> RemoveLoggedTimes(List<int> itemIds);

        /// <summary>
        /// Makes a check about Time Tracking item existence.
        /// </summary>
        /// <param name="timeTrackingItemId">Id of needed Time Tracking item.</param>
        /// <returns>True - if Time Tracking item exists, otherwise - false.</returns>
        bool Exists(int timeTrackingItemId);
    }
}
