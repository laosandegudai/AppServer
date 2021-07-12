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
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Data
{
    /// <summary>
    /// Represents a business logic-level custom status of task.
    /// </summary>
    public class CustomTaskStatusData : CustomStatusData
    {
        /// <summary>
        /// Type of status.
        /// </summary>
        public new TaskStatus StatusType { get; set; }

        /// <summary>
        /// Determines availability of changes.
        /// </summary>
        public bool CanChangeAvailability => StatusType != TaskStatus.Open;

        public static List<CustomTaskStatusData> GetDefaults()
        {
            return new List<CustomTaskStatusData>
            {
                GetDefault(TaskStatus.Open),
                GetDefault(TaskStatus.Closed)
            };
        }

        public static CustomTaskStatusData GetDefault(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.Open => GetDefault(status, "Open", "inbox.svg"),
                TaskStatus.Closed => GetDefault(status, "Closed", "check_tick.svg"),
                _ => null
            };
        }

        private static CustomTaskStatusData GetDefault(TaskStatus status, string title, string svg, bool available = true)
        {
            return new CustomTaskStatusData
            {
                StatusType = status,
                Id = -(int)status,
                Title = title,
                Image = GetImageBase64Content("/skins/default/images/svg/projects/" + svg),
                ImageType = "image/svg+xml",
                Color = "#83888d",
                IsDefault = true,
                IsAvailable = available
            };
        }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<DbCustomTaskStatus, CustomTaskStatusData>()
                .ReverseMap();
        }
    }
}
