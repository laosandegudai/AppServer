using ASC.Notify.Recipients;
using ASC.Projects.Core.BusinessLogic.Data;

namespace ASC.Projects.Core.BusinessLogic.Notifications.Interfaces
{
    /// <summary>
    /// An interface of sender of'Projects' product-related messages.
    /// </summary>
    public interface IProjectsNotificationSender
    {
        /// <summary>
        /// Sends a letter which contains an invitation to project team.
        /// </summary>
        /// <param name="recipient">Recipient of letter.</param>
        /// <param name="project">Project related data.</param>
        void SendProjectTeamInvitation(IRecipient recipient, ProjectData project);

        /// <summary>
        /// Sends a letter which contains a notification about removing from project team.
        /// </summary>
        /// <param name="recipient">Recipient of letter.</param>
        /// <param name="project">Project related data.</param>
        void SendRemovingFromProjectTeam(IRecipient recipient, ProjectData project);
    }
}
