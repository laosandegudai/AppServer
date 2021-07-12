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
using System.Collections.Generic;

using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Security.Data;

using Autofac;

namespace ASC.Projects.Core.BusinessLogic.Security
{
    public class SecurityManager
    {
        #region Fields and .ctor

        private ILifetimeScope _scope { get; set; }

        private readonly ProjectSecurityManager _projectSecurityManager;

        private readonly MilestoneSecurityManager _milestoneSecurityManager;

        private readonly TaskSecurityManager _taskSecurityManager;

        private readonly MessageSecurityManager _messageSecurityManager;

        private readonly TimeTrackingSecurityManager _timeTrackingSecurityManager;

        private readonly CommonSecurityManager _commonSecurityManager;

        private readonly SecurityContext _securityContext;

        public bool CurrentUserAdministrator => _commonSecurityManager.CurrentUserAdministrator;

        public bool IsPrivateDisabled => _commonSecurityManager.IsPrivateDisabled;

        public SecurityManager(ILifetimeScope scope,
            ProjectSecurityManager projectSecurityManager,
            MilestoneSecurityManager milestoneSecurityManager,
            MessageSecurityManager messageSecurityManager,
            TaskSecurityManager taskSecurityManager,
            TimeTrackingSecurityManager timeTrackingSecurityManager,
            CommonSecurityManager commonSecurityManager,
            SecurityContext securityContext)
        {
            _scope = scope.NotNull(nameof(scope));
            _projectSecurityManager = projectSecurityManager.NotNull(nameof(projectSecurityManager));
            _milestoneSecurityManager = milestoneSecurityManager.NotNull(nameof(milestoneSecurityManager));
            _messageSecurityManager = messageSecurityManager.NotNull(nameof(messageSecurityManager));
            _taskSecurityManager = taskSecurityManager.NotNull(nameof(taskSecurityManager));
            _timeTrackingSecurityManager = timeTrackingSecurityManager.NotNull(nameof(timeTrackingSecurityManager));
            _commonSecurityManager = commonSecurityManager.NotNull(nameof(commonSecurityManager));
            _securityContext = securityContext.NotNull(nameof(securityContext));
        }

        #endregion Fields and .ctor

        public bool CanCreate<T>(ProjectData project) where T : BaseData<int>
        {
            return _scope.Resolve<SecurityTemplateManager<T>>().CanCreateEntities(project);
        }

        public bool CanEdit<T>(T entity) where T : BaseData<int>
        {
            return _scope.Resolve<SecurityTemplateManager<T>>().CanUpdateEntity(entity);
        }

        public bool CanRead<T>(T entity) where T : BaseData<int>
        {
            return _scope.Resolve<SecurityTemplateManager<T>>().CanReadEntity(entity);
        }

        public bool CanRead<T>(T entity, Guid userId) where T : BaseData<int>
        {
            return _scope.Resolve<SecurityTemplateManager<T>>().CanReadEntity(entity, userId);
        }

        public bool CanRead<T>(ProjectData project) where T : BaseData<int>
        {
            return _scope.Resolve<SecurityTemplateManager<T>>().CanReadEntities(project);
        }

        public bool CanDelete<T>(T entity) where T : BaseData<int>
        {
            return _scope.Resolve<SecurityTemplateManager<T>>().CanDeleteEntity(entity);
        }

        public bool CanEditComment(BaseData<int> entity, CommentData comment)
        {
            if (entity is ProjectTaskData task)
            {
                return _taskSecurityManager.CanEditComment(task, comment);
            }

            var message = entity as MessageData;

            var result = _messageSecurityManager.CanEditComment(message, comment);

            return result;
        }

        public bool CanEditComment(ProjectData entity, CommentData comment)
        {
            var result = _projectSecurityManager.CanEditComment(entity, comment);

            return result;
        }

        public bool CanCreateComment(BaseData<int> entity)
        {
            if (entity is ProjectTaskData task)
            {
                return _taskSecurityManager.CanCreateComment(task);
            }

            var message = entity as MessageData;

            var result = _messageSecurityManager.CanCreateComment(message);

            return result;
        }

        public bool CanCreateComment(ProjectData project)
        {
            var result = _projectSecurityManager.CanCreateComment(project);

            return result;
        }

        public bool CanEdit(ProjectTaskData task, ProjectSubtaskData subtask)
        {
            var result = _taskSecurityManager.CanEdit(task, subtask);

            return result;
        }

        public bool CanReadFiles(ProjectData project, Guid userId)
        {
            var result = _projectSecurityManager.CanReadFiles(project, userId);

            return result;
        }

        public bool CanReadFiles(ProjectData project)
        {
            return _projectSecurityManager.CanReadFiles(project);
        }

        public bool CanEditFiles<T>(T entity) where T : BaseData<int>
        {
            var result = _scope.Resolve<SecurityTemplateManager<T>>().CanEditFiles(entity);

            return result;
        }

        public bool CanEditTeam(ProjectData project)
        {
            var result = _projectSecurityManager.CanEditTeam(project);

            return result;
        }

        public bool CanLinkContact(ProjectData project)
        {
            var result = _projectSecurityManager.CanLinkContact(project);

            return result;
        }

        public bool CanReadContacts(ProjectData project)
        {
            var result = _projectSecurityManager.CanReadContacts(project);

            return result;
        }

        public bool CanEditPaymentStatus(TimeTrackingItemData timeSpend)
        {
            var result = _timeTrackingSecurityManager.CanEditPaymentStatus(timeSpend);

            return result;
        }

        public bool CanCreateSubtask(ProjectTaskData task)
        {
            var result = _taskSecurityManager.CanCreateSubtask(task);

            return result;
        }

        public bool CanCreateTimeSpend(ProjectTaskData task)
        {
            var result = _taskSecurityManager.CanCreateTimeSpend(task);

            return result;
        }

        public bool CanGoToFeed<T>(T entity, Guid userId) where T : BaseData<int>
        {
            var result = _scope.Resolve<SecurityTemplateManager<T>>().CanGoToFeed(entity, userId);

            return result;
        }

        public bool CanGoToFeed(ParticipantData participant, Guid userId)
        {
            if (participant == null || !IsProjectsEnabled(userId))
            {
                return false;
            }

            var result = _commonSecurityManager.IsInTeam(participant.Project, userId, false)
                || _commonSecurityManager.IsFollow(participant.Project, userId);

            return result;
        }

        public bool IsInTeam(ProjectData project, Guid userId, bool includeAdmin = true)
        {
            var result = _commonSecurityManager.IsInTeam(project, userId, includeAdmin);

            return result;
        }

        public void DemandCreate<T>(ProjectData project) where T : BaseData<int>
        {
            if (!CanCreate<T>(project))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandEdit<T>(T entity) where T : BaseData<int>
        {
            if (!CanEdit(entity))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandEdit(ProjectTaskData task, ProjectSubtaskData subtask)
        {
            if (!CanEdit(task, subtask))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandDelete<T>(T entity) where T : ProjectEntityData
        {
            if (!CanDelete(entity))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandEditComment(ProjectEntityData entity, CommentData comment)
        {
            if (!CanEditComment(entity, comment))
            {
                throw CreateSecurityException();
            }
        }


        public void DemandCreateComment(ProjectData project)
        {
            if (!CanCreateComment(project))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandCreateComment(ProjectEntityData entity)
        {
            if (!CanCreateComment(entity))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandLinkContact(ProjectData project)
        {
            if (_projectSecurityManager.CanLinkContact(project))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandEditTeam(ProjectData project)
        {
            if (!CanEditTeam(project))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandReadFiles(ProjectData project)
        {
            if (!CanReadFiles(project))
            {
                throw CreateSecurityException();
            }
        }

        public void DemandAuthentication()
        {
            if (!_securityContext.CurrentAccount.IsAuthenticated)
            {
                throw CreateSecurityException();
            }
        }

        public bool IsVisitor()
        {
            var result = IsVisitor(_securityContext.CurrentAccount.ID);

            return result;
        }

        public bool IsVisitor(Guid userId)
        {
            var result = _commonSecurityManager.IsVisitor(userId);

            return result;
        }

        public bool IsAdministrator()
        {
            return IsAdministrator(_securityContext.CurrentAccount.ID);
        }

        public bool IsAdministrator(Guid userId)
        {
            var result = _commonSecurityManager.IsAdministrator(userId);

            return result;
        }

        public bool IsProjectsEnabled()
        {
            return IsProjectsEnabled(_securityContext.CurrentAccount.ID);
        }

        public bool IsProjectsEnabled(Guid userId)
        {
            var result = _commonSecurityManager.IsProjectsEnabled(userId);

            return result;
        }


        public void GetProjectSecurityInfo(ProjectData project)
        {
            project.Security = GetProjectSecurityInfoWithSecurity(project);
        }

        public void GetProjectSecurityInfo(List<ProjectData> projects)
        {
            foreach (var project in projects)
            {
                project.Security = GetProjectSecurityInfoWithSecurity(project);
            }
        }

        public void GetTaskSecurityInfo(ProjectTaskData task)
        {
            task.Security = GetTaskSecurityInfoWithSecurity(task);
        }

        public void GetTaskSecurityInfo(List<ProjectTaskData> tasks)
        {
            foreach (var task in tasks)
            {
                task.Security = GetTaskSecurityInfoWithSecurity(task);
            }
        }

        private ProjectSecurityInfo GetProjectSecurityInfoWithSecurity(ProjectData project)
        {
            return new ProjectSecurityInfo
            {
                CanCreateMilestone = _milestoneSecurityManager.CanCreateEntities(project),
                CanCreateMessage = _messageSecurityManager.CanCreateEntities(project),
                CanCreateTask = _taskSecurityManager.CanCreateEntities(project),
                CanCreateTimeSpend = _timeTrackingSecurityManager.CanCreateEntities(project),

                CanEditTeam = _projectSecurityManager.CanEditTeam(project),
                CanReadFiles = _projectSecurityManager.CanReadFiles(project),
                CanReadMilestones = _milestoneSecurityManager.CanReadEntities(project),
                CanReadMessages = _messageSecurityManager.CanReadEntities(project),
                CanReadTasks = _taskSecurityManager.CanReadEntities(project),
                IsInTeam = _commonSecurityManager.IsInTeam(project, _securityContext.CurrentAccount.ID, false),
                CanLinkContact = _projectSecurityManager.CanLinkContact(project),
                CanReadContacts = _projectSecurityManager.CanReadContacts(project),

                CanEdit = _projectSecurityManager.CanUpdateEntity(project),
                CanDelete = _projectSecurityManager.CanDeleteEntity(project),
            };
        }

        private TaskSecurityInfo GetTaskSecurityInfoWithSecurity(ProjectTaskData task)
        {
            return new TaskSecurityInfo
            {
                CanEdit = _taskSecurityManager.CanUpdateEntity(task),
                CanCreateSubtask = _taskSecurityManager.CanCreateSubtask(task),
                CanCreateTimeSpend = _taskSecurityManager.CanCreateTimeSpend(task),
                CanDelete = _taskSecurityManager.CanDeleteEntity(task),
                CanReadFiles = _projectSecurityManager.CanReadFiles(task.Project)
            };
        }

        public static Exception CreateSecurityException()
        {
            throw new System.Security.SecurityException("Access denied.");
        }

        public static Exception CreateGuestSecurityException()
        {
            throw new System.Security.SecurityException("A guest cannot be appointed as responsible.");
        }
    }
}
