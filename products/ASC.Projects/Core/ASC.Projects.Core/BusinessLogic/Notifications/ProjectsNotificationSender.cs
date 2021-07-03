using System.Globalization;
using ASC.Core.Common.Notify;
using ASC.Core.Common.Notify.Push;
using ASC.Core.Common.Utils;
using ASC.Notify;
using ASC.Notify.Patterns;
using ASC.Notify.Recipients;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Notifications.Data;
using ASC.Projects.Core.BusinessLogic.Notifications.Interfaces;

namespace ASC.Projects.Core.BusinessLogic.Notifications
{
    /// <summary>
    /// Sender of 'Projects' product-related messages.
    /// </summary>
    public class ProjectsNotificationSender : IProjectsNotificationSender
    {
        private readonly INotifyClient _notifyClient;

        private readonly ReplyToTagProvider _replyToTagProvider;

        public ProjectsNotificationSender(INotifyClient notifyClient,
            ReplyToTagProvider replyToTagProvider)
        {
            _notifyClient = notifyClient.NotNull(nameof(notifyClient));
            _replyToTagProvider = replyToTagProvider.NotNull(nameof(replyToTagProvider));
        }

        /// <summary>
        /// Sends a letter which contains an invitation to project team.
        /// </summary>
        /// <param name="recipient">Recipient of letter (team invitee).</param>
        /// <param name="project">Project related data.</param>
        public void SendProjectTeamInvitation(IRecipient recipient, ProjectData project)
        {
            _notifyClient.SendNoticeToAsync(
                ProjectsNotificationConstants.Event_InviteToProject,
                project.UniqueId,
                new[] { recipient },
                true,
                new TagValue(ProjectsNotificationConstants.Tag_ProjectID, project.Id),
                new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, project.Title),
                _replyToTagProvider.Message(project.Id),
                new AdditionalSenderTag("push.sender"),
                new TagValue(PushConstants.PushItemTagName,
                    new PushItem(PushItemType.Project, project.Id.ToString(CultureInfo.InvariantCulture), project.Title)),
                new TagValue(PushConstants.PushModuleTagName, PushModule.Projects),
                new TagValue(PushConstants.PushActionTagName, PushAction.InvitedTo));
        }

        /// <summary>
        /// Sends a letter which contains a notification about removing from project team.
        /// </summary>
        /// <param name="recipient">Recipient of letter (removing team member).</param>
        /// <param name="project">Project related data.</param>
        public void SendRemovingFromProjectTeam(IRecipient recipient, ProjectData project)
        {
            if (recipient == null)
            {
                return;
            }

            _notifyClient.SendNoticeToAsync(ProjectsNotificationConstants.Event_RemoveFromProject,
                project.UniqueId,
                new[] { recipient },
                true,
                new TagValue(ProjectsNotificationConstants.Tag_ProjectID, project.Id),
                new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, project.Title),
                _replyToTagProvider.Message(project.Id));
        }
    }
}
