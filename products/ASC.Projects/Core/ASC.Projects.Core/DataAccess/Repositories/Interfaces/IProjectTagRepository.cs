using System.Collections.Generic;
using ASC.Projects.Core.DataAccess.Domain.Entities;

namespace ASC.Projects.Core.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// An interface of repository working with <see cref="DbProjectTag"/> entity.
    /// </summary>
    public interface IProjectTagRepository : IRepository<DbProjectTag, int>
    {
        /// <summary>
        /// Receives title of tag with specified id.
        /// </summary>
        /// <param name="id">Id of needed tag.</param>
        /// <returns>Title of tag with specified id.</returns>
        public string GetTitleById(int id);

        /// <summary>
        /// Creates new tag.
        /// </summary>
        /// <param name="title">Title of new tag.</param>
        /// <returns>Just created tag <see cref="DbProjectTag"/>.</returns>
        public DbProjectTag Create(string title);

        /// <summary>
        /// Receives a list of tags with specified prefix in title.
        /// </summary>
        /// <param name="prefix">Needed prefix.</param>
        /// <returns>List of tags with specified prefix in title.</returns>
        public List<DbProjectTag> GetTagsWithPrefix(string prefix);

        /// <summary>
        /// Receives a list of projects which are tagged by tag with specified title.
        /// </summary>
        /// <param name="tagName">Needed tag title.</param>
        /// <returns>A list of project ids tagged by specified tag.</returns>
        public List<int> GetTaggedProjectIds(string tagName);

        /// <summary>
        /// Receives a list of projects which are tagged by tag with specified id.
        /// </summary>
        /// <param name="tagId">Id of needed tag.</param>
        /// <returns>A list of project ids tagged by specified tag.</returns>
        public List<int> GetTaggedProjectIds(int tagId);

        /// <summary>
        /// Receives a list of tags associated with needed project.
        /// </summary>
        /// <param name="projectId">Id of needed project</param>
        /// <returns></returns>
        public List<DbProjectTag> GetProjectTags(int projectId);
    }
}
