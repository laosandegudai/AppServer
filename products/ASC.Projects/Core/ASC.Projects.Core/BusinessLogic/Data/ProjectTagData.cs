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
using ASC.Common.Mapping;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Data
{
    /// <summary>
    /// Represents a business logic-level project tag.
    /// </summary>
    public class ProjectTagData : BaseData<int>, IMapFrom<DbProjectTag>
    {
        /// <summary>
        /// Title of tag.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Id of user who created this tag.
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// Date when this tag was created.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Id of user who edited this tag lastly.
        /// </summary>
        public Guid? LastEditorId { get; set; }

        /// <summary>
        /// Date when this tag was edited lastly.
        /// </summary>
        public DateTime? LastModificationDate { get; set; }

        /// <summary>
        /// Projects which were been tagged by this tag.
        /// </summary>
        public List<ProjectData> TaggedProjects { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DbProjectTag, ProjectTagData>();
        }
    }
}
