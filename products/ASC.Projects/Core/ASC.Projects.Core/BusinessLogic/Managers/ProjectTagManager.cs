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
using System.Linq;
using ASC.Core.Common.Utils;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    /// <summary>
    /// An manager working with project tags.
    /// </summary>
    public class ProjectTagManager : IProjectTagManager
    {
        private readonly IProjectTagRepository _projectTagRepository;

        private readonly IProjectRepository _projectRepository;

        private readonly IMapper _mapper;

        public ProjectTagManager(IProjectTagRepository projectTagRepository,
            IProjectRepository projectRepository,
            IMapper mapper)
        {
            _projectTagRepository = projectTagRepository.NotNull(nameof(projectTagRepository));
            _projectRepository = projectRepository.NotNull(nameof(projectRepository));
            _mapper = mapper.NotNull(nameof(mapper));
        }

        /// <summary>
        /// Creates new tag.
        /// </summary>
        /// <param name="tag">New tag data.</param>
        /// <returns>Just created tag data <see cref="ProjectTagData"/>.</returns>
        public ProjectTagData CreateTag(ProjectTagData tag)
        {
            var newTag = _mapper.Map<ProjectTagData, DbProjectTag>(tag);

            var createdTag = _projectTagRepository.Create(newTag);

            var result = _mapper.Map<DbProjectTag, ProjectTagData>(createdTag);

            return result;
        }

        /// <summary>
        /// Receives a full list of tags.
        /// </summary>
        /// <returns>List of existing tags data <see cref="ProjectData"/>.</returns>
        public List<ProjectTagData> GetTags()
        {
            var tags = _projectTagRepository
                .GetAll()
                .Select(pt => _mapper.Map<DbProjectTag, ProjectTagData>(pt))
                .ToList();

            return tags;
        }

        /// <summary>
        /// Receives a list of tags with specified prefix in title.
        /// </summary>
        /// <param name="prefix">Needed prefix.</param>
        /// <returns>List of tags <see cref="ProjectTagData"/> with specified prefix in title.</returns>
        public List<ProjectTagData> GetTagsWithPrefix(string prefix)
        {
            var tags = _projectTagRepository
                .GetTagsWithPrefix(prefix)
                .Select(pt => _mapper.Map<DbProjectTag, ProjectTagData>(pt))
                .ToList();

            return tags;
        }

        // <summary>
        /// Receives a tag title by id of tag.
        /// </summary>
        /// <param name="id">Needed tag id.</param>
        /// <returns>Title of tag with specified id.</returns>
        public string GetTagTitleById(int id)
        {
            var result = _projectTagRepository.GetTitleById(id);

            return result;
        }

        /// <summary>
        /// Receives an ids of projects tagged by specified tag title.
        /// </summary>
        /// <param name="tagName">Needed tag title.</param>
        /// <returns>ids of projects <see cref="int"/> tagged by specified tag.</returns>
        public List<int> GetTaggedProjectIds(string tagName)
        {
            var result = _projectTagRepository.GetTaggedProjectIds(tagName);

            return result;
        }

        /// <summary>
        /// Receives an ids of projects tagged by tag with specified id.
        /// </summary>
        /// <param name="tagName">Needed tag id.</param>
        /// <returns>ids of projects <see cref="int"/> tagged by tag with specified id.</returns>
        public List<int> GetTaggedProjectIds(int tagId)
        {
            var result = _projectTagRepository.GetTaggedProjectIds(tagId);

            return result;
        }

        /// <summary>
        /// Receives a list of projects tagged by tag with specified title.
        /// </summary>
        /// <param name="tagName">Needed tag title.</param>
        /// <returns>A list of projects tagged by tag with specified title</returns>
        public List<ProjectData> GetTaggedProjects(string tagName)
        {
            var taggedProjectIds = GetTaggedProjectIds(tagName);

            if (taggedProjectIds?.Any() == false)
            {
                return null;
            }

            var projects = _projectRepository.GetProjectByIds(taggedProjectIds)
                .Select(p => _mapper.Map<DbProject, ProjectData>(p))
                .ToList();

            return projects;
        }

        /// <summary>
        /// Receives a list of tags related to project with specific id.
        /// </summary>
        /// <param name="projectId">Needed project id.</param>
        /// <returns>A list of tags related to project with specific id.</returns>
        public List<ProjectTagData> GetProjectTags(int projectId)
        {
            var result = _projectTagRepository.GetProjectTags(projectId)
                .Select(pt => _mapper.Map<DbProjectTag, ProjectTagData>(pt))
                .ToList();

            return result;
        }
    }
}
