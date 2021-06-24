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
        public ProjectTagRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

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
                .Where(pt => pt.ProjectId == projectId)
                .ToList();

            return tags;
        }
    }
}
