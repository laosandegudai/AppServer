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
using System.Linq;
using ASC.Core;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// A repository working with <see cref="DbProjectTag"/> entity.
    /// </summary>
    internal class ProjectTagRepository : BaseTenantRepository<DbProjectTag, int>, IProjectTagRepository
    {
        #region .ctor

        public ProjectTagRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }


        #endregion .ctor

        /// <summary>
        /// Receives title of tag with specified id.
        /// </summary>
        /// <param name="id">Id of needed tag.</param>
        /// <returns>Title of tag with specified id.</returns>
        public string GetTitleById(int id)
        {
            var tag = GetById(id);

            if (tag == null)
            {
                throw new ArgumentException($"Project tag with ID = {id} does not exists");
            }

            var title = tag.Title;

            return title;
        }

        /// <summary>
        /// Creates new tag.
        /// </summary>
        /// <param name="title">Title of new tag.</param>
        /// <returns>Just created tag <see cref="DbProjectTag"/>.</returns>
        public DbProjectTag Create(string title)
        {
            var newTag = new DbProjectTag
            {
                Title = title,
                LastModificationDate = DateTime.UtcNow
            };

            base.Create(newTag);

            return newTag;
        }

        /// <summary>
        /// Receives a list of tags with specified prefix in title.
        /// </summary>
        /// <param name="prefix">Needed prefix.</param>
        /// <returns>List of tags with specified prefix in title.</returns>
        public List<DbProjectTag> GetTagsWithPrefix(string prefix)
        {
            var result = base.GetAll()
                .Where(pt => pt.Title.StartsWith(prefix))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives a list of projects which are tagged by tag with specified title.
        /// </summary>
        /// <param name="tagName">Needed tag title.</param>
        /// <returns>A list of project ids tagged by specified tag.</returns>
        public List<int> GetTaggedProjectIds(string tagName)
        {
            var result = base.GetAll()
                .Include(pt => pt.TaggedProjects)
                .Where(p => p.Title.Equals(tagName, StringComparison.InvariantCultureIgnoreCase))
                .SelectMany(pt => pt.TaggedProjects)
                .Select(p => p.Id)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives a list of projects which are tagged by tag with specified id.
        /// </summary>
        /// <param name="tagId">Id of needed tag.</param>
        /// <returns>A list of project ids tagged by specified tag.</returns>
        public List<int> GetTaggedProjectIds(int tagId)
        {
            var result = base.GetAll()
                .Include(pt => pt.TaggedProjects)
                .Where(pt => pt.Id == tagId)
                .SelectMany(pt => pt.TaggedProjects)
                .Select(p => p.Id)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives a list of tags associated with needed project.
        /// </summary>
        /// <param name="projectId">Id of needed project</param>
        /// <returns></returns>
        public List<DbProjectTag> GetProjectTags(int projectId)
        {
            var tags = GetAll()
                .Include(pt => pt.TaggedProjects)
                .ThenInclude(pt => pt.Tags)
                .SelectMany(pt => pt.TaggedProjects)
                .Where(tp => tp.Id == projectId)
                .SelectMany(pt => pt.Tags)
                .ToList();

            return tags;
        }
    }
}
