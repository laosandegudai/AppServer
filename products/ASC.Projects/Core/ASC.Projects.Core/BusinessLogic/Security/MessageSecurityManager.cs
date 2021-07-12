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
using System.Linq;
using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Security
{
    public class MessageSecurityManager : SecurityTemplateManager<MessageData>
    {
        #region Fields and .ctor

        private readonly ProjectSecurityManager _projectSecurityManager;

        private readonly SecurityContext _securityContext;

        private readonly IMessageManager _messageManager;

        public MessageSecurityManager(CommonSecurityManager commonSecurityManager,
            ProjectSecurityManager projectSecurityManager,
            SecurityContext securityContext,
            IMessageManager messageEngine) : base(commonSecurityManager)
        {
            _projectSecurityManager = projectSecurityManager.NotNull(nameof(projectSecurityManager));
            _securityContext = securityContext.NotNull(nameof(securityContext));
            _messageManager = messageEngine.NotNull(nameof(messageEngine));
        }

        #endregion Fields and .ctor

        public override bool CanCreateEntities(ProjectData project)
        {
            if (!base.CanCreateEntities(project))
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(project))
            {
                return true;
            }

            var result = CommonSecurityManager.IsInTeam(project) && CanReadEntities(project);

            return result;
        }

        public override bool CanReadEntities(ProjectData project, Guid userId)
        {
            var result = base.CanReadEntities(project, userId)
                && CommonSecurityManager.GetTeamSecurity(project, userId, ProjectTeamSecurity.Messages);

            return result;
        }

        public override bool CanReadEntity(MessageData entity, Guid userId)
        {
            if (entity == null || !_projectSecurityManager.CanReadEntity(entity.Project, userId))
            {
                return false;
            }

            var result = CanReadEntities(entity.Project, userId);

            return result;
        }

        public override bool CanUpdateEntity(MessageData message)
        {
            if (!base.CanUpdateEntity(message) || !CanReadEntity(message))
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(message.Project))
            {
                return true;
            }

            var result = CommonSecurityManager.IsInTeam(message.Project) && message.CreatorId == CommonSecurityManager.CurrentUserId;

            return result;
        }

        public override bool CanDeleteEntity(MessageData message)
        {
            var result = CanUpdateEntity(message);

            return result;
        }

        public override bool CanCreateComment(MessageData message)
        {
            var result = CanReadEntity(message)
                   && (message == null || message.Status == MessageStatus.Open)
                   && CommonSecurityManager.IsProjectsEnabled()
                   && _securityContext.IsAuthenticated
                   && !CommonSecurityManager.CurrentUserIsOutsider;

            return result;
        }

        public override bool CanGoToFeed(MessageData message, Guid userId)
        {
            if (message == null || !CommonSecurityManager.IsProjectsEnabled(userId))
            {
                return false;
            }

            if (message.CreatorId == userId)
            {
                return true;
            }

            if (!CommonSecurityManager.IsInTeam(message.Project, userId, false)
                && !CommonSecurityManager.IsFollow(message.Project, userId))
            {
                return false;
            }

            var isSubscriber = _messageManager.GetSubscribers(message)
                .Any(r => new Guid(r.ID)
                    .Equals(userId));
            
            var result = isSubscriber && CommonSecurityManager.GetTeamSecurityForParticipants(message.Project, userId, ProjectTeamSecurity.Messages);

            return result;
        }

        public override bool CanEditComment(MessageData message, CommentData comment)
        {
            var result = message.Status == MessageStatus.Open && _projectSecurityManager.CanEditComment(message.Project, comment);

            return result;
        }

        public override bool CanEditFiles(MessageData entity)
        {
            if (!CommonSecurityManager.IsProjectsEnabled()
                || entity.Status == MessageStatus.Archived
                || entity.Project.Status == ProjectStatus.Closed)
            {
                return false;
            }

            var result = CommonSecurityManager.IsProjectManager(entity.Project) || CommonSecurityManager.IsInTeam(entity.Project);

            return result;
        }
    }
}
