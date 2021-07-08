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
using ASC.Core.Users;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Security.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using ASC.Web.Core;
using ASC.Web.Core.Utility.Settings;

namespace ASC.Projects.Core.BusinessLogic.Security
{
    public class CommonSecurityManager : ICommonSecurityManager
    {
        public Guid CurrentUserId { get; }

        public bool CurrentUserAdministrator { get; }

        private bool CurrentUserIsVisitor { get; set; }

        internal bool CurrentUserIsOutsider { get; set; }

        public bool CurrentUserIsProjectsEnabled { get; }

        public bool CurrentUserIsCRMEnabled { get; }

        public bool IsPrivateDisabled { get; }

        private readonly IProjectRepository _projectRepository;

        private readonly UserManager _userManager;

        private readonly WebItemManager _webItemManager;

        private readonly WebItemSecurity _webItemSecurity;

        public CommonSecurityManager(IProjectRepository projectDao,
            SecurityContext securityContext,
            UserManager userManager,
            WebItemManager webItemManager,
            WebItemSecurity webItemSecurity,
            TenantAccessSettings tenantAccessSettings)
        {
            _projectRepository = projectDao.NotNull(nameof(projectDao));
            _userManager = userManager.NotNull(nameof(userManager));
            _webItemManager = webItemManager.NotNull(nameof(webItemManager));
            _webItemSecurity = webItemSecurity.NotNull(nameof(webItemSecurity));

            CurrentUserId = securityContext.CurrentAccount.ID;
            CurrentUserAdministrator = _userManager.IsUserInGroup(CurrentUserId, Constants.GroupAdmin.ID)
                || _webItemSecurity.IsProductAdministrator(WebItemManager.ProjectsProductID, CurrentUserId);

            CurrentUserIsVisitor = _userManager.GetUsers(CurrentUserId).IsVisitor(userManager);

            CurrentUserIsOutsider = IsOutsider(CurrentUserId);
            IsPrivateDisabled = tenantAccessSettings.Anyone;
            CurrentUserIsProjectsEnabled = IsModuleEnabled(WebItemManager.ProjectsProductID, CurrentUserId);
            CurrentUserIsCRMEnabled = IsModuleEnabled(WebItemManager.CRMProductID, CurrentUserId);
        }

        public bool Can(Guid userId)
        {
            var can = !IsVisitor(userId) && IsProjectsEnabled(userId);

            return can;
        }

        public bool Can()
        {
            var can = Can(CurrentUserId);

            return can;
        }

        public bool IsAdministrator(Guid userId)
        {
            if (userId == CurrentUserId)
            {
                return CurrentUserAdministrator;
            }

            var result = _userManager.IsUserInGroup(userId, Constants.GroupAdmin.ID)
                   || _webItemSecurity.IsProductAdministrator(WebItemManager.ProjectsProductID, userId);

            return result;
        }

        public bool IsProjectsEnabled()
        {
            return IsProjectsEnabled(CurrentUserId);
        }

        public bool IsProjectsEnabled(Guid userId)
        {
            var result = userId == CurrentUserId
                ? CurrentUserIsProjectsEnabled
                : IsModuleEnabled(WebItemManager.ProjectsProductID, userId);

            return result;
        }

        public bool IsCrmEnabled()
        {
            var result = IsCrmEnabled(CurrentUserId);

            return result;
        }

        public bool IsCrmEnabled(Guid userId)
        {
            var result = userId == CurrentUserId
                ? CurrentUserIsCRMEnabled
                : IsModuleEnabled(WebItemManager.CRMProductID, userId);

            return result;
        }

        public bool IsModuleEnabled(Guid module, Guid userId)
        {
            var projects = _webItemManager[module];

            var result = projects != null && !projects.IsDisabled(userId, _webItemSecurity);

            return result;
        }

        public bool IsVisitor(Guid userId)
        {
            var result = userId == CurrentUserId
                ? CurrentUserIsVisitor
                : _userManager.GetUsers(userId).IsVisitor(_userManager);

            return result;
        }

        public bool IsOutsider(Guid userId)
        {
            var result = _userManager
                .GetUsers(userId)
                .IsOutsider(_userManager);

            return result;
        }

        public bool IsProjectManager(ProjectData project)
        {
            var result = IsProjectManager(project, CurrentUserId);

            return result;
        }

        public bool IsProjectManager(ProjectData project, Guid userId)
        {
            var result = (IsAdministrator(userId)
                || (project != null && project.ResponsibleId == userId))
                && !CurrentUserIsVisitor;

            return result;
        }

        public bool IsProjectCreator(ProjectData project)
        {
            var result = IsProjectCreator(project, CurrentUserId);

            return result;
        }

        public bool IsProjectCreator(ProjectData project, Guid userId)
        {
            var result = project != null && project.CreatorId == userId;

            return result;
        }

        public bool IsInTeam(ProjectData project)
        {
            var result = IsInTeam(project, CurrentUserId);

            return result;
        }

        public bool IsInTeam(ProjectData project, Guid userId, bool includeAdmin = true)
        {
            var isAdmin = includeAdmin && IsAdministrator(userId);
            var isInTeam = project != null && _projectRepository.IsInTeam(project.Id, userId);
            var result = isAdmin && isInTeam;

            return result;
        }

        public bool IsFollow(ProjectData project, Guid userId)
        {
            var isAdmin = IsAdministrator(userId);
            var isPrivate = project != null && (!project.IsPrivate || isAdmin);

            var result = isPrivate && _projectRepository.IsFollowing(project.Id, userId);

            return result;
        }

        public bool GetTeamSecurity(ProjectData project, ProjectTeamSecurity security)
        {
            var result = GetTeamSecurity(project, CurrentUserId, security);

            return result;
        }

        public bool GetTeamSecurity(ProjectData project, Guid userId, ProjectTeamSecurity security)
        {
            if (IsProjectManager(project, userId)
                || project == null
                || !project.IsPrivate)
            {
                return true;
            }

            var s = _projectRepository.GetTeamSecurity(project.Id, userId);

            var result = (s & security) != security && _projectRepository.IsInTeam(project.Id, userId);

            return result;
        }

        public bool GetTeamSecurityForParticipants(ProjectData project, Guid userId, ProjectTeamSecurity security)
        {
            if (IsProjectManager(project, userId) || !project.IsPrivate)
            {
                return true;
            }

            var teamSecurity = _projectRepository.GetTeamSecurity(project.Id, userId);
            return (teamSecurity & security) != security;
        }
    }
}
