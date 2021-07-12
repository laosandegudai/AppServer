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

using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Web.Core;

namespace ASC.Projects.Core.BusinessLogic.Security
{
    public sealed class ProjectSecurityManager : SecurityTemplateManager<ProjectData>
    {
        #region Fields and .ctor

        private readonly WebItemSecurity _webItemSecurity;

        private readonly SecurityContext _securityContext;

        public ProjectSecurityManager(CommonSecurityManager securityCommonManager,
            WebItemSecurity webItemSecurity,
            SecurityContext securityContext) : base(securityCommonManager)
        {
            _webItemSecurity = webItemSecurity.NotNull(nameof(webItemSecurity));
            _securityContext = securityContext.NotNull(nameof(securityContext));
        }

        #endregion Fields and .ctor

        public override bool CanReadEntity(ProjectData project, Guid userId)
        {
            if (!CommonSecurityManager.IsProjectsEnabled(userId)
                || project == null
                || (project.IsPrivate && CommonSecurityManager.IsPrivateDisabled))
            {
                return false;
            }

            var result = !project.IsPrivate
                || CommonSecurityManager.IsProjectCreator(project, userId)
                || CommonSecurityManager.IsInTeam(project, userId);

            return result;
        }

        public override bool CanUpdateEntity(ProjectData project)
        {
            var result = base.CanUpdateEntity(project)
                && CommonSecurityManager.IsProjectManager(project)
                || CommonSecurityManager.IsProjectCreator(project);

            return result;
        }

        public override bool CanDeleteEntity(ProjectData project)
        {
            var result = base.CanDeleteEntity(project)
                && CommonSecurityManager.CurrentUserAdministrator
                || CommonSecurityManager.IsProjectCreator(project);

            return result;
        }

        public override bool CanGoToFeed(ProjectData project, Guid userId)
        {
            if (project == null || !CommonSecurityManager.IsProjectsEnabled(userId))
            {
                return false;
            }

            var result = _webItemSecurity.IsProductAdministrator(WebItemManager.ProjectsProductID, userId)
                   || CommonSecurityManager.IsInTeam(project, userId, false)
                   || CommonSecurityManager.IsFollow(project, userId)
                   || CommonSecurityManager.IsProjectCreator(project, userId);

            return result;
        }

        public bool CanEditTeam(ProjectData project)
        {
            return CommonSecurityManager.Can() && (CommonSecurityManager.IsProjectManager(project) || CommonSecurityManager.IsProjectCreator(project));
        }

        public bool CanReadFiles(ProjectData project)
        {
            return CanReadFiles(project, _securityContext.CurrentAccount.ID);
        }

        public bool CanReadFiles(ProjectData project, Guid userId)
        {
            return CommonSecurityManager.IsProjectsEnabled(userId) && CommonSecurityManager.GetTeamSecurity(project, userId, ProjectTeamSecurity.Files);
        }

        public override bool CanEditComment(ProjectData project, CommentData comment)
        {
            if (!CommonSecurityManager.IsProjectsEnabled())
            {
                return false;
            }

            if (project == null || comment == null)
            {
                return false;
            }

            var result = comment.CreatorId == CommonSecurityManager.CurrentUserId
                || CommonSecurityManager.IsProjectManager(project);

            return result;
        }

        public bool CanReadContacts(ProjectData project)
        {
            var result = CommonSecurityManager.IsCrmEnabled()
                && CommonSecurityManager.IsProjectsEnabled()
                && CommonSecurityManager.GetTeamSecurity(project, ProjectTeamSecurity.Contacts);

            return result;
        }

        public bool CanLinkContact(ProjectData project)
        {
            return CommonSecurityManager.IsProjectsEnabled() && CanUpdateEntity(project);
        }
    }
}
