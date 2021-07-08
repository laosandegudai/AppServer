using System;

namespace ASC.Projects.Core.BusinessLogic.Notifications.Data
{
    public class SubtaskNotificationData : BaseNotificationData
    {
        public string NotificationId { get; set; }

        public int ProjectId { get; set; }

        public string ProjectTitle { get; set; }

        public int TaskId { get; set; }

        public string TaskTitle { get; set; }

        public int SubtaskId { get; set; }

        public string SubtaskTitle { get; set; }

        public Guid ResponsibleId { get; set; }
    }
}
