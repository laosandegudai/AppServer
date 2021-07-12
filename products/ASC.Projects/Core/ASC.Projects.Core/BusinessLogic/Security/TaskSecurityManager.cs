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
    public class TaskSecurityManager : SecurityTemplateManager<ProjectTaskData>
    {
        #region Fields and .ctor

        private readonly ProjectSecurityManager _projectSecurityManager;

        private readonly MilestoneSecurityManager _milestoneSecurityManager;

        private readonly SecurityContext _securityContext;

        private readonly IMilestoneManager _milestoneManager;

        public TaskSecurityManager(CommonSecurityManager securityCommonManager,
            ProjectSecurityManager projectSecurityManager,
            MilestoneSecurityManager milestoneSecurityManager,
            SecurityContext securityContext,
            IMilestoneManager milestoneManager) : base(securityCommonManager)
        {
            _milestoneSecurityManager = milestoneSecurityManager.NotNull(nameof(milestoneSecurityManager));
            _projectSecurityManager = projectSecurityManager.NotNull(nameof(projectSecurityManager));
            _securityContext = securityContext.NotNull(nameof(securityContext));
            _milestoneManager = milestoneManager.NotNull(nameof(milestoneManager));
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
                && CommonSecurityManager.GetTeamSecurity(project, userId, ProjectTeamSecurity.Tasks);

            return result;
        }

        public override bool CanReadEntity(ProjectTaskData task, Guid userId)
        {
            if (task == null || !_projectSecurityManager.CanReadEntity(task.Project, userId))
            {
                return false;
            }

            if (task.ResponsibleIds.Contains(userId))
            {
                return true;
            }

            if (!CanReadEntities(task.Project, userId))
            {
                return false;
            }

            if (task.MilestoneId == default || _milestoneSecurityManager.CanReadEntities(task.Project, userId))
            {
                return true;
            }

            var milestone = _milestoneManager.GetById(task.MilestoneId.GetValueOrDefault());

            var result = _milestoneSecurityManager.CanReadEntity(milestone, userId);

            return result;
        }

        public override bool CanUpdateEntity(ProjectTaskData task)
        {
            if (!base.CanUpdateEntity(task) || task.Project.Status == ProjectStatus.Closed)
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(task.Project))
            {
                return true;
            }

            var result = CommonSecurityManager.IsInTeam(task.Project)
                && (task.CreatorId == CommonSecurityManager.CurrentUserId
                    || !task.ResponsibleIds.Any()
                    || task.ResponsibleIds.Contains(CommonSecurityManager.CurrentUserId));

            return result;
        }

        public override bool CanDeleteEntity(ProjectTaskData task)
        {
            if (!base.CanDeleteEntity(task))
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(task.Project))
            {
                return true;
            }

            var result = CommonSecurityManager.IsInTeam(task.Project) && task.CreatorId == CommonSecurityManager.CurrentUserId;

            return result;
        }

        public override bool CanCreateComment(ProjectTaskData entity)
        {
            var result = CanReadEntity(entity)
                && CommonSecurityManager.IsProjectsEnabled()
                && _securityContext.IsAuthenticated
                && !CommonSecurityManager.CurrentUserIsOutsider;

            return result;
        }

        public bool CanEdit(ProjectTaskData task, ProjectSubtaskData subtask)
        {
            if (subtask == null || !CommonSecurityManager.Can())
            {
                return false;
            }

            if (CanUpdateEntity(task))
            {
                return true;
            }

            var result = CommonSecurityManager.IsInTeam(task.Project)
                && (subtask.CreatorId == CommonSecurityManager.CurrentUserId
                    || subtask.ResponsibleId == CommonSecurityManager.CurrentUserId);

            return result;
        }

        public override bool CanGoToFeed(ProjectTaskData task, Guid userId)
        {
            if (task == null || !CommonSecurityManager.IsProjectsEnabled(userId))
            {
                return false;
            }

            if (task.CreatorId == userId)
            {
                return true;
            }

            if (!CommonSecurityManager.IsInTeam(task.Project, userId, false)
                && !CommonSecurityManager.IsFollow(task.Project, userId))
            {
                return false;
            }

            if (task.ResponsibleIds.Contains(userId))
            {
                return true;
            }

            if (task.MilestoneId == default || _milestoneSecurityManager.CanReadEntities(task.Project, userId))
            {
                return CommonSecurityManager.GetTeamSecurityForParticipants(task.Project, userId,
                    ProjectTeamSecurity.Tasks);
            }

            var milestone = _milestoneManager.GetById(task.MilestoneId.GetValueOrDefault());

            var result = milestone.ResponsibleId == userId
                || CommonSecurityManager.GetTeamSecurityForParticipants(task.Project, userId, ProjectTeamSecurity.Tasks);

            return result;
        }

        public override bool CanEditFiles(ProjectTaskData entity)
        {
            if (!CommonSecurityManager.IsProjectsEnabled()
                || entity.Project.Status == ProjectStatus.Closed)
            {
                return false;
            }

            var result = CommonSecurityManager.IsProjectManager(entity.Project) || CanUpdateEntity(entity);

            return result;
        }

        public override bool CanEditComment(ProjectTaskData entity, CommentData comment)
        {
            var result = entity != null && _projectSecurityManager.CanEditComment(entity.Project, comment);

            return result;
        }

        public bool CanCreateSubtask(ProjectTaskData task)
        {
            if (task == null || !CommonSecurityManager.Can())
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(task.Project))
            {
                return true;
            }

            var result = CommonSecurityManager.IsInTeam(task.Project)
                && ((task.CreatorId == CommonSecurityManager.CurrentUserId)
                    || !task.ResponsibleIds.Any()
                    || task.ResponsibleIds.Contains(CommonSecurityManager.CurrentUserId));

            return result;
        }

        public bool CanCreateTimeSpend(ProjectTaskData task)
        {
            if (task == null
                || !CommonSecurityManager.Can()
                || (task.Project.Status != ProjectStatus.Open))
            {
                return false;
            }

            if (CommonSecurityManager.IsInTeam(task.Project))
            {
                return true;
            }

            var result = task.ResponsibleIds.Contains(CommonSecurityManager.CurrentUserId)
                || task.Subtasks.SelectMany(r => r.ResponsibleIds).Contains(CommonSecurityManager.CurrentUserId);

            return result;
        }
    }
}
