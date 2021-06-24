namespace ASC.Projects.Core.BusinessLogic.Managers.Interfaces
{
    public interface IProjectTaskManager
    {
        /// <summary>
        /// Makes a check about task with specified id existence.
        /// </summary>
        /// <param name="taskId">Id of needed task.</param>
        /// <returns>true - if task exists, otherwise - false.</returns>
        bool Exists(int taskId);
    }
}
