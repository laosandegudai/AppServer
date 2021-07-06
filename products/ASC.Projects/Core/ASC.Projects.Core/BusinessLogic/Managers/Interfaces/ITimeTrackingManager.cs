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
    /// An interface of manager working with Time Tracking items.
    /// </summary>
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
