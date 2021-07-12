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

using ASC.Core.Common.Utils;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Security
{
    public sealed class MilestoneSecurityManager : SecurityTemplateManager<MilestoneData>
    {
        #region Fields and .ctor

        public ProjectSecurityManager _projectSecurityManager { get; set; }

        public MilestoneSecurityManager(CommonSecurityManager securityCommonManager,
            ProjectSecurityManager projectSecurityManager) : base(securityCommonManager)
        {
            _projectSecurityManager = projectSecurityManager.NotNull(nameof(projectSecurityManager));
        }

        #endregion Fields and .ctor

        public override bool CanCreateEntities(ProjectData project)
        {
            var result = base.CanCreateEntities(project) && CommonSecurityManager.IsProjectManager(project);

            return result;
        }

        public override bool CanReadEntities(ProjectData project, Guid userId)
        {
            var result = base.CanReadEntities(project, userId)
                && CommonSecurityManager.GetTeamSecurity(project, userId, ProjectTeamSecurity.Milestone);

            return result;
        }

        public override bool CanReadEntity(MilestoneData entity, Guid userId)
        {
            if (entity == null || !_projectSecurityManager.CanReadEntity(entity.Project, userId))
            {
                return false;
            }

            var result = entity.ResponsibleId == userId || CanReadEntities(entity.Project, userId);

            return result;
        }

        public override bool CanUpdateEntity(MilestoneData milestone)
        {
            if (!base.CanUpdateEntity(milestone)
                || milestone.Project.Status == ProjectStatus.Closed)
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(milestone.Project))
            {
                return true;
            }

            if (!CanReadEntity(milestone)) return false;

            return CommonSecurityManager.IsInTeam(milestone.Project)
                && (milestone.CreatorId == CommonSecurityManager.CurrentUserId
                    || milestone.ResponsibleId == CommonSecurityManager.CurrentUserId);
        }

        public override bool CanDeleteEntity(MilestoneData milestone)
        {
            if (!base.CanDeleteEntity(milestone))
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(milestone.Project))
            {
                return true;
            }

            var result = CommonSecurityManager.IsInTeam(milestone.Project) && milestone.CreatorId == CommonSecurityManager.CurrentUserId;

            return result;
        }

        public override bool CanGoToFeed(MilestoneData milestone, Guid userId)
        {
            if (milestone == null || !CommonSecurityManager.IsProjectsEnabled(userId))
            {
                return false;
            }

            if (!CommonSecurityManager.IsInTeam(milestone.Project, userId, false)
                && !CommonSecurityManager.IsFollow(milestone.Project, userId))
            {
                return false;
            }
            var result = milestone.ResponsibleId == userId
                || CommonSecurityManager.GetTeamSecurityForParticipants(milestone.Project, userId, ProjectTeamSecurity.Milestone);

            return result;
        }
    }
}
