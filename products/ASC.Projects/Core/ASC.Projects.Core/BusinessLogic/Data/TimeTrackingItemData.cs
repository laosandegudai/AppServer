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
using ASC.Common.Mapping;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Data
{
    /// <summary>
    /// Represents a business logic-level time tracking item.
    /// </summary>
    public class TimeTrackingItemData : BaseData<int>, IMapFrom<DbTimeTrackingItem>
    {
        /// <summary>
        /// Id of task, which this item logged for.
        /// </summary>
        public int RelatedTaskId { get; set; }

        /// <summary>
        /// Task, which this item logged for.
        /// </summary>
        public ProjectTaskData RelatedTask { get; set; }

        /// <summary>
        /// Log date.
        /// </summary>
        public DateTime TrackingDate { get; set; }

        /// <summary>
        /// Amount of logged hours.
        /// </summary>
        public decimal Hours { get; set; }

        /// <summary>
        /// Id of person, who logged this item.
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// Note for item.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Status of payment for this item.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// Date when status of this item was changed.
        /// </summary>
        public DateTime StatusChangeDate { get; set; }

        /// <summary>
        /// Date when this item was been created,
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Id of person, who created this item.
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Person, who created this item.
        /// </summary>
        public EmployeeData Creator { get; set; }

        /// <summary>
        /// Determines ability of this item edition.
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Determines ability of this item payment status edition.
        /// </summary>
        public bool CanEditPaymentStatus { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DbTimeTrackingItem, TimeTrackingItemData>()
                .ReverseMap();
        }
    }
}
