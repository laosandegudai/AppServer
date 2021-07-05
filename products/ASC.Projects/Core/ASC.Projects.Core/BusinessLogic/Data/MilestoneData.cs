using System;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Data
{
    /// <summary>
    /// Represents a milestone.
    /// </summary>
    public class MilestoneData
    {
        /// <summary>
        /// Id of milestone.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of milestone.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of milestone.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// HTML title of milestone.
        /// </summary>
        public string HtmlTitle { get; set; }

        /// <summary>
        /// Id of project, which this milestone is part of.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Project, which this milestone is part of.
        /// </summary>
        public ProjectData Project { get; set; }

        /// <summary>
        /// Id of user who created this milestone.
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Date when this milestone was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Date when this milestone was edited lastly.
        /// </summary>
        public DateTime LastModificationDate { get; set; }

        /// <summary>
        /// Id of employee who edited this milestone lastly.
        /// </summary>
        public Guid LastEditorId { get; set; }

        /// <summary>
        /// Id of employee who is responsible for this milestone.
        /// </summary>
        public Guid ResponsibleId { get; set; }

        /// <summary>
        /// Employee who is responsible for milestone.
        /// </summary>
        public EmployeeData Responsible { get; set; }

        /// <summary>
        /// Status of milestone.
        /// </summary>
        public MilestoneStatus Status { get; set; }

        /// <summary>
        /// Determines notification needs.
        /// </summary>
        public bool IsNotify { get; set; }

        /// <summary>
        /// Determines this milestone as key.
        /// </summary>
        public bool IsKey { get; set; }

        /// <summary>
        /// Deadline of milestone.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Count of active tasks in milestone.
        /// </summary>
        public int ActiveTaskCount { get; set; }

        /// <summary>
        /// Count of closed tasks in milestone.
        /// </summary>
        public int ClosedTaskCount { get; set; }

        /// <summary>
        /// Date when status of milestone was changed.
        /// </summary>
        public DateTime StatusChangeDate { get; set; }

        public string NotificationId { get; set; }
    }
}
