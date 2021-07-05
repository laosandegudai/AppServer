using ASC.Notify.Recipients;

namespace ASC.Projects.Core.BusinessLogic.Notifications.Data
{
    public class BaseNotificationData
    {
        public string InitiatorId { get; set; }

        public IRecipient[] Recipients { get; set; }
    }
}
