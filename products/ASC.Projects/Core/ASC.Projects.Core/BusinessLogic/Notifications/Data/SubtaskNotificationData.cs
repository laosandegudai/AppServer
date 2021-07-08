using System;

namespace ASC.Projects.Core.BusinessLogic.Notifications.Data
{
    /// <summary>
    /// Represents a data, which is needed for subtasks notifications sending.
    /// </summary>
    public class SubtaskNotificationData : BaseNotificationData
    {
        /// <summary>
        /// Id of notification.
        /// </summary>
        public string NotificationId { get; set; }

        /// <summary>
        /// Id of project, which this subtask is related for.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Title of project, which this subtask is related for.
        /// </summary>
        public string ProjectTitle { get; set; }

        /// <summary>
        /// Id of parent task, which this subtask is related for.
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Title of task, which this subtask is related for.
        /// </summary>
        public string TaskTitle { get; set; }

        /// <summary>
        /// Id of subtask.
        /// </summary>
        public int SubtaskId { get; set; }

        /// <summary>
        /// Title of subtask.
        /// </summary>
        public string SubtaskTitle { get; set; }

        /// <summary>
        /// Id of person, who is responsible for the task.
        /// </summary>
        public Guid ResponsibleId { get; set; }
    }
}
