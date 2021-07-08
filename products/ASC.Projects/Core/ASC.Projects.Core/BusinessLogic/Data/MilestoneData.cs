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
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Data
{
    /// <summary>
    /// Represents a milestone.
    /// </summary>
    public class MilestoneData
    {
        /// <summary>
        /// Id of milestone.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of milestone.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of milestone.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// HTML title of milestone.
        /// </summary>
        public string HtmlTitle { get; set; }

        /// <summary>
        /// Id of project, which this milestone is part of.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Project, which this milestone is part of.
        /// </summary>
        public ProjectData Project { get; set; }

        /// <summary>
        /// Id of user who created this milestone.
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Date when this milestone was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Date when this milestone was edited lastly.
        /// </summary>
        public DateTime LastModificationDate { get; set; }

        /// <summary>
        /// Id of employee who edited this milestone lastly.
        /// </summary>
        public Guid LastEditorId { get; set; }

        /// <summary>
        /// Id of employee who is responsible for this milestone.
        /// </summary>
        public Guid ResponsibleId { get; set; }

        /// <summary>
        /// Employee who is responsible for milestone.
        /// </summary>
        public EmployeeData Responsible { get; set; }

        /// <summary>
        /// Status of milestone.
        /// </summary>
        public MilestoneStatus Status { get; set; }

        /// <summary>
        /// Determines notification needs.
        /// </summary>
        public bool IsNotify { get; set; }

        /// <summary>
        /// Determines this milestone as key.
        /// </summary>
        public bool IsKey { get; set; }

        /// <summary>
        /// Deadline of milestone.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Count of active tasks in milestone.
        /// </summary>
        public int ActiveTaskCount { get; set; }

        /// <summary>
        /// Count of closed tasks in milestone.
        /// </summary>
        public int ClosedTaskCount { get; set; }

        /// <summary>
        /// Date when status of milestone was changed.
        /// </summary>
        public DateTime StatusChangeDate { get; set; }

        public string NotificationId { get; set; }
    }
}
