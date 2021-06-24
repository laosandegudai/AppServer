using ASC.Core.Common.Utils;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    public class ProjectTaskManager : IProjectTaskManager
    {
        /// <summary>
        /// An instance of repository working with Project Tasks.
        /// </summary>
        private readonly IProjectTaskRepository _projectTaskRepository;

        public ProjectTaskManager(IProjectTaskRepository projectTaskDao)
        {
            _projectTaskRepository = projectTaskDao.NotNull(nameof(projectTaskDao));
        }

        /// Makes a check about task with specified id existence.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>true - if task exists, otherwise - false.</returns>
        public bool Exists(int taskId)
        {
            taskId.IsPositive(nameof(taskId));

            var doesTaskExists = _projectTaskRepository.Exists(taskId);

            return doesTaskExists;
        }
    }
}
