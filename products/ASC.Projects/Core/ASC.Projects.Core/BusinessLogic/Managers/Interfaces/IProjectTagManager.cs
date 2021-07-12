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

namespace ASC.Projects.Core.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// An interface of business logic manager responsible for project tags processing.
    /// </summary>
    public interface IProjectTagManager
    {
        /// <summary>
        /// Creates new tag.
        /// </summary>
        /// <param name="tag">New tag data.</param>
        /// <returns>Just created tag data <see cref="ProjectTagData"/>.</returns>
        ProjectTagData CreateTag(ProjectTagData tag);

        /// <summary>
        /// Receives a full list of tags.
        /// </summary>
        /// <returns>List of existing tags data <see cref="ProjectData"/>.</returns>
        List<ProjectTagData> GetTags();

        /// <summary>
        /// Receives a list of tags with specified prefix in title.
        /// </summary>
        /// <param name="prefix">Needed prefix.</param>
        /// <returns>List of tags <see cref="ProjectTagData"/> with specified prefix in title.</returns>
        List<ProjectTagData> GetTagsWithPrefix(string prefix);

        /// <summary>
        /// Receives a tag title by id of tag.
        /// </summary>
        /// <param name="id">Needed tag id.</param>
        /// <returns>Title of tag with specified id.</returns>
        string GetTagTitleById(int id);

        /// <summary>
        /// Receives an ids of projects tagged by specified tag title.
        /// </summary>
        /// <param name="tagName">Needed tag title.</param>
        /// <returns>ids of projects <see cref="int"/> tagged by specified tag.</returns>
        List<int> GetTaggedProjectIds(string tagName);

        /// <summary>
        /// Receives an ids of projects tagged by tag with specified id.
        /// </summary>
        /// <param name="tagName">Needed tag id.</param>
        /// <returns>ids of projects <see cref="int"/> tagged by tag with specified id.</returns>
        List<int> GetTaggedProjectIds(int tagId);

        /// <summary>
        /// Receives a list of projects tagged by tag with specified title.
        /// </summary>
        /// <param name="tagName">Needed tag title.</param>
        /// <returns>A list of projects tagged by tag with specified title</returns>
        List<ProjectData> GetTaggedProjects(string tagName);

        /// <summary>
        /// Receives a list of tags related to project with specific id.
        /// </summary>
        /// <param name="projectId">Needed project id.</param>
        /// <returns>A list of tags related to project with specific id.</returns>
        List<ProjectTagData> GetProjectTags(int projectId);
    }
}
