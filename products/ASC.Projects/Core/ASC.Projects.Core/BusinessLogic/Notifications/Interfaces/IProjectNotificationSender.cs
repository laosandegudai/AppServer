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

using ASC.Notify.Recipients;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Notifications.Data;

namespace ASC.Projects.Core.BusinessLogic.Notifications.Interfaces
{
    /// <summary>
    /// An interface of sender of'Projects' product-related messages.
    /// </summary>
    public interface IProjectNotificationSender
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

        /// <summary>
        /// Sends a letter which contains a notification about milestone removal.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        void SendMilestoneRemovalNotification(MilestoneNotificationData notificationData);

        /// <summary>
        /// Sends a letter which contains a notification about milestone closing.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        void SendMilestoneClosingNotification(MilestoneNotificationData notificationData);

        /// <summary>
        /// Sends a letter which contains a notification about milestone resuming.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        void SendMilestoneResumedNotification(MilestoneNotificationData notificationData);

        /// <summary>
        /// Sends a letter which contains a notification about milestone creation.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        void SendMilestoneCreatedNotification(MilestoneNotificationData notificationData);

        /// <summary>
        /// Sends a letter which contains a notification about milestone responsible person change.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        void SendMilestoneResponsibleChangedNotification(MilestoneNotificationData notificationData);

        /// <summary>
        /// Sends a letter which contains a notification about milestone update.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        void SendMilestoneUpdatedNotification(MilestoneNotificationData notificationData);

        /// <summary>
        /// Sends a letter which contains a notification about subtask creation.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        void SendSubtaskCreatedNotification(SubtaskNotificationData notificationData);

        /// <summary>
        /// Sends a letter which contains a notification about subtask modification.
        /// </summary>
        /// <param name="notificationData">Data which is needed for notification sending.</param>
        void SendSubtaskModifiedNotification(SubtaskNotificationData notificationData);
    }
}
