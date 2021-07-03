#region License agreement statement

/*
 *
 * (c) Copyright Ascensio System Limited 2010-2018
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/

#endregion License agreement statement

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
