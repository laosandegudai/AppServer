namespace ASC.Projects.Core.BusinessLogic.Notifications.Data
{
    public class MilestoneNotificationData : BaseNotificationData
    {
        public string NotificationId { get; set; }

        public string Description { get; set; }

        public int ProjectId { get; set; }

        public string ProjectTitle { get; set; }

        public int MilestoneId { get; set; }

        public string MilestoneTitle { get; set; }
    }
}
