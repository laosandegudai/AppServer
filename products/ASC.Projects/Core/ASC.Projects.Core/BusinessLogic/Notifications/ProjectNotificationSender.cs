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

using System;
using System.Collections;
using System.Globalization;
using System.Linq;

using ASC.Core.Common.Extensions;
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
    public class ProjectNotificationSender : IProjectNotificationSender
    {
        private readonly INotifyClient _notifyClient;

        private readonly IInitiatorInterceptorBuilder _initiatorInterceptorBuilder;

        private readonly ReplyToTagProvider _replyToTagProvider;

        public ProjectNotificationSender(INotifyClient notifyClient,
            IInitiatorInterceptorBuilder initiatorInterceptorBuilder,
            ReplyToTagProvider replyToTagProvider)
        {
            _notifyClient = notifyClient.NotNull(nameof(notifyClient));
            _initiatorInterceptorBuilder = initiatorInterceptorBuilder.NotNull(nameof(initiatorInterceptorBuilder));
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

        /// <summary>
        /// Sends a letter which contains a notification about milestone removal.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        public void SendMilestoneRemovalNotification(MilestoneNotificationData notificationData)
        {
            DoWithInterceptor(notificationData, () =>
            {
                _notifyClient.SendNoticeToAsync(ProjectsNotificationConstants.Event_MilestoneDeleted,
                    notificationData.NotificationId,
                    notificationData.Recipients,
                    true,
                    new TagValue(ProjectsNotificationConstants.Tag_ProjectID, notificationData.ProjectId),
                    new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, notificationData.ProjectTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_EntityTitle, notificationData.MilestoneTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_EntityID, notificationData.MilestoneId),
                    new TagValue(ProjectsNotificationConstants.Tag_AdditionalData, notificationData.Description),
                    new AdditionalSenderTag("push.sender"),

                    new TagValue(PushConstants.PushItemTagName, new PushItem(PushItemType.Milestone,
                        notificationData.MilestoneId.ToString(CultureInfo.InvariantCulture),
                        notificationData.MilestoneTitle)),

                    new TagValue(PushConstants.PushParentItemTagName,
                        new PushItem(PushItemType.Project,
                            notificationData.ProjectId.ToString(CultureInfo.InvariantCulture),
                            notificationData.ProjectTitle)),

                    new TagValue(PushConstants.PushModuleTagName, PushModule.Projects),
                    new TagValue(PushConstants.PushActionTagName, PushAction.Deleted));
            });
        }

        /// <summary>
        /// Sends a letter which contains a notification about milestone closing.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        public void SendMilestoneClosingNotification(MilestoneNotificationData notificationData)
        {
            DoWithInterceptor(notificationData,
                () =>
                {
                    _notifyClient.SendNoticeToAsync(
                        ProjectsNotificationConstants.Event_MilestoneClosed,
                        notificationData.NotificationId,
                        notificationData.Recipients,
                        true,
                        new TagValue(ProjectsNotificationConstants.Tag_ProjectID, notificationData.ProjectId),
                        new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, notificationData.ProjectTitle),
                        new TagValue(ProjectsNotificationConstants.Tag_EntityTitle, notificationData.MilestoneTitle),
                        new TagValue(ProjectsNotificationConstants.Tag_EntityID, notificationData.MilestoneId),
                        new TagValue(ProjectsNotificationConstants.Tag_AdditionalData, notificationData.Description),
                        new AdditionalSenderTag("push.sender"),

                        new TagValue(PushConstants.PushItemTagName,
                            new PushItem(PushItemType.Milestone,
                                notificationData.MilestoneId.ToString(CultureInfo.InvariantCulture),
                                notificationData.MilestoneTitle)),

                        new TagValue(PushConstants.PushParentItemTagName,
                            new PushItem(PushItemType.Project,
                                notificationData.ProjectId.ToString(CultureInfo.InvariantCulture),
                                notificationData.ProjectTitle)),

                        new TagValue(PushConstants.PushModuleTagName, PushModule.Projects),
                        new TagValue(PushConstants.PushActionTagName, PushAction.Closed));
                }, "milestone closed");
        }

        /// <summary>
        /// Sends a letter which contains a notification about milestone resuming.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        public void SendMilestoneResumedNotification(MilestoneNotificationData notificationData)
        {
            DoWithInterceptor(notificationData, () =>
            {
                _notifyClient.SendNoticeToAsync(
                    ProjectsNotificationConstants.Event_MilestoneResumed,
                    notificationData.NotificationId,
                    notificationData.Recipients,
                    true,
                    new TagValue(ProjectsNotificationConstants.Tag_ProjectID, notificationData.ProjectId),
                    new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, notificationData.ProjectTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_EntityTitle, notificationData.MilestoneTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_EntityID, notificationData.MilestoneId),
                    new TagValue(ProjectsNotificationConstants.Tag_AdditionalData, notificationData.Description),
                    new AdditionalSenderTag("push.sender"),

                    new TagValue(PushConstants.PushItemTagName,
                        new PushItem(PushItemType.Milestone,
                            notificationData.MilestoneId.ToString(CultureInfo.InvariantCulture),
                            notificationData.MilestoneTitle)),

                    new TagValue(PushConstants.PushParentItemTagName,
                        new PushItem(PushItemType.Project,
                            notificationData.ProjectId.ToString(CultureInfo.InvariantCulture),
                            notificationData.ProjectTitle)),

                    new TagValue(PushConstants.PushModuleTagName, PushModule.Projects),
                    new TagValue(PushConstants.PushActionTagName, PushAction.Resumed));
            });
        }

        /// <summary>
        /// Sends a letter which contains a notification about milestone creation.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        public void SendMilestoneCreatedNotification(MilestoneNotificationData notificationData)
        {
            DoWithInterceptor(notificationData, () =>
            {
                _notifyClient.SendNoticeToAsync(ProjectsNotificationConstants.Event_MilestoneCreated,
                    notificationData.NotificationId,
                    notificationData.Recipients,
                    true,
                    new TagValue(ProjectsNotificationConstants.Tag_ProjectID, notificationData.ProjectId),
                    new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, notificationData.ProjectTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_EntityTitle, notificationData.MilestoneTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_EntityID, notificationData.MilestoneId),

                    new TagValue(ProjectsNotificationConstants.Tag_AdditionalData,
                        new Hashtable
                        {
                            {
                                "MilestoneDescription", notificationData.Description
                            }
                        }),

                    new AdditionalSenderTag("push.sender"),

                    new TagValue(PushConstants.PushItemTagName,
                        new PushItem(PushItemType.Milestone,
                            notificationData.MilestoneId.ToString(CultureInfo.InvariantCulture),
                            notificationData.MilestoneTitle)),

                    new TagValue(PushConstants.PushParentItemTagName,
                        new PushItem(PushItemType.Project,
                            notificationData.ProjectId.ToString(CultureInfo.InvariantCulture),
                            notificationData.ProjectTitle)),

                    new TagValue(PushConstants.PushModuleTagName, PushModule.Projects),
                    new TagValue(PushConstants.PushActionTagName, PushAction.Created));
            });
        }

        /// <summary>
        /// Sends a letter which contains a notification about milestone responsible person change.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        public void SendMilestoneResponsibleChangedNotification(MilestoneNotificationData notificationData)
        {
            notificationData.NotNull(nameof(notificationData));

            if (notificationData.Recipients?.Any() == false)
            {
                return;
            }

            _notifyClient.SendNoticeToAsync(
                ProjectsNotificationConstants.Event_ResponsibleForMilestone,
                notificationData.NotificationId,
                notificationData.Recipients,
                true,
                new TagValue(ProjectsNotificationConstants.Tag_ProjectID, notificationData.ProjectId),
                new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, notificationData.ProjectTitle),
                new TagValue(ProjectsNotificationConstants.Tag_EntityTitle, notificationData.MilestoneTitle),
                new TagValue(ProjectsNotificationConstants.Tag_EntityID, notificationData.MilestoneId),
                new TagValue(ProjectsNotificationConstants.Tag_AdditionalData, new Hashtable { { "MilestoneDescription", notificationData.Description } }),
                _replyToTagProvider.Comment("project.milestone", notificationData.MilestoneId.ToString(CultureInfo.InvariantCulture)),
                new AdditionalSenderTag("push.sender"),
                new TagValue(PushConstants.PushItemTagName, new PushItem(PushItemType.Milestone, notificationData.MilestoneId.ToString(CultureInfo.InvariantCulture), notificationData.MilestoneId.ToString())),
                new TagValue(PushConstants.PushParentItemTagName, new PushItem(PushItemType.Project, notificationData.ProjectId.ToString(CultureInfo.InvariantCulture), notificationData.ProjectTitle)),
                new TagValue(PushConstants.PushModuleTagName, PushModule.Projects),
                new TagValue(PushConstants.PushActionTagName, PushAction.Assigned));
        }

        /// <summary>
        /// Sends a letter which contains a notification about milestone update.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        public void SendMilestoneUpdatedNotification(MilestoneNotificationData notificationData)
        {
            notificationData.NotNull(nameof(notificationData));

            if (notificationData.Recipients?.Any() == false)
            {
                return;
            }

            _notifyClient.SendNoticeToAsync(ProjectsNotificationConstants.Event_MilestoneEdited,
                notificationData.NotificationId,
                notificationData.Recipients,
                true,
                new TagValue(ProjectsNotificationConstants.Tag_ProjectID, notificationData.ProjectId),
                new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, notificationData.ProjectTitle),
                new TagValue(ProjectsNotificationConstants.Tag_EntityTitle, notificationData.MilestoneTitle),
                new TagValue(ProjectsNotificationConstants.Tag_EntityID, notificationData.MilestoneId));
        }

        /// <summary>
        /// Sends a letter which contains a notification about subtask creation.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        public void SendSubtaskCreatedNotification(SubtaskNotificationData notificationData)
        {
            notificationData.NotNull(nameof(notificationData));

            DoWithInterceptor(notificationData, () =>
                {
                    _notifyClient.SendNoticeToAsync(ProjectsNotificationConstants.Event_SubTaskCreated,
                        notificationData.NotificationId,
                        notificationData.Recipients,
                        true,
                        new TagValue(ProjectsNotificationConstants.Tag_ProjectID, notificationData.ProjectId),
                        new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, notificationData.ProjectTitle),
                        new TagValue(ProjectsNotificationConstants.Tag_EntityTitle, notificationData.TaskTitle),
                        new TagValue(ProjectsNotificationConstants.Tag_SubEntityTitle, notificationData.SubtaskTitle),
                        new TagValue(ProjectsNotificationConstants.Tag_EntityID, notificationData.TaskId),
                        new TagValue(ProjectsNotificationConstants.Tag_Responsible, notificationData.ResponsibleId),
                        _replyToTagProvider.Comment("project.task", notificationData.TaskId.ToString(CultureInfo.InvariantCulture)),
                        new AdditionalSenderTag("push.sender"),
                        new TagValue(PushConstants.PushItemTagName, new PushItem(PushItemType.Subtask, notificationData.SubtaskId.ToString(CultureInfo.InvariantCulture), notificationData.SubtaskTitle)),
                        new TagValue(PushConstants.PushParentItemTagName, new PushItem(PushItemType.Task, notificationData.TaskId.ToString(CultureInfo.InvariantCulture), notificationData.TaskTitle)),
                        new TagValue(PushConstants.PushModuleTagName, PushModule.Projects),
                        new TagValue(PushConstants.PushActionTagName, PushAction.Created));
                });
        }

        /// <summary>
        /// Sends a letter which contains a notification about subtask modification.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        public void SendSubtaskModifiedNotification(SubtaskNotificationData notificationData)
        {
            notificationData.NotNull(nameof(notificationData));

            DoWithInterceptor(notificationData, () =>
            {
                _notifyClient.SendNoticeToAsync(ProjectsNotificationConstants.Event_SubTaskEdited,
                    notificationData.NotificationId,
                    notificationData.Recipients,
                    true,
                    new TagValue(ProjectsNotificationConstants.Tag_ProjectID, notificationData.ProjectId),
                    new TagValue(ProjectsNotificationConstants.Tag_ProjectTitle, notificationData.ProjectTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_EntityTitle, notificationData.TaskTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_SubEntityTitle, notificationData.SubtaskTitle),
                    new TagValue(ProjectsNotificationConstants.Tag_EntityID, notificationData.TaskId),
                    new TagValue(ProjectsNotificationConstants.Tag_Responsible, notificationData.ResponsibleId),
                    _replyToTagProvider.Comment("project.task", notificationData.TaskId.ToString(CultureInfo.InvariantCulture)));
            });
        }

        private void DoWithInterceptor(BaseNotificationData notificationData,
            Action action,
            string recipientEventName = null)
        {
            action.NotNull(nameof(action));
            notificationData.NotNull(nameof(notificationData));

            if (!recipientEventName.IsNullOrWhiteSpace())
            {
                _notifyClient.BeginSingleRecipientEvent(recipientEventName);
            }

            var interceptor = _initiatorInterceptorBuilder.Build(notificationData.InitiatorId);

            _notifyClient.AddInterceptor(interceptor);

            try
            {
                action.Invoke();
            }
            catch
            {
                // ToDo: logging?
            }
            finally
            {
                _notifyClient.RemoveInterceptor(interceptor.Name);

                if (!recipientEventName.IsNullOrWhiteSpace())
                {
                    _notifyClient.EndSingleRecipientEvent(recipientEventName);
                }
            }
        }
    }
}
