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
using ASC.Projects.Core.BusinessLogic.Security.Data;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Data
{
    /// <summary>
    /// Represents a business logic-level project.
    /// </summary>
    public class ProjectData : BaseData<int>, IMapFrom<DbProject>
    {
        /// <summary>
        /// Title of project.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Id of person who has created project.
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// Date when project has been created.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Id of person who modified project lastly.
        /// </summary>
        public Guid? LastEditorId { get; set; }

        /// <summary>
        /// Date when project was been modified lastly.
        /// </summary>
        public DateTime? LastModificationDate { get; set; }

        /// <summary>
        /// Description of project.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Status of project.
        /// </summary>
        public ProjectStatus Status { get; set; }

        /// <summary>
        /// Date when status of project has been changed lastly.
        /// </summary>
        public DateTime StatusChangedDate { get; set; }

        /// <summary>
        /// Id of person, who is responsible fo this project.
        /// </summary>
        public Guid ResponsibleId { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Determines project as private.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Amount of milestones in project.
        /// </summary>
        public int MilestoneCount { get; set; }

        /// <summary>
        /// Amount of project tasks.
        /// </summary>
        public int TaskCount { get; set; }

        /// <summary>
        /// Amount of project discussions.
        /// </summary>

        public int DiscussionCount { get; set; }

        /// <summary>
        /// Amount of project documents.
        /// </summary>
        public int DocumentsCount { get; set; }

        /// <summary>
        /// Amount of project participants.
        /// </summary>
        public int ParticipantCount { get; set; }

        /// <summary>
        /// Unique Id of project.
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        /// Security info.
        /// </summary>
        public ProjectSecurityInfo Security { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DbProject, ProjectData>()
                .ReverseMap();
        }
    }
}
