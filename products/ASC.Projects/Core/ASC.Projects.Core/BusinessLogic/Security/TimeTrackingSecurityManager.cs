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
using ASC.Projects.Core.Data.Domain.Entities;
using ASC.Projects.Core.Data.Domain.Enums;

namespace ASC.Projects.Core.BusinessLogic.Security
{
    public class TimeTrackingSecurityManager : SecurityTemplateManager<DbTimeTrackingItem>
    {
        private readonly TaskSecurityManager _taskSecurityManager;

        public TimeTrackingSecurityManager(CommonSecurityManager commonSecurityManager,
            TaskSecurityManager taskSecurityManager) : base(commonSecurityManager)
        {
            _taskSecurityManager = taskSecurityManager.NotNull(nameof(taskSecurityManager));
        }

        public override bool CanCreateEntities(DbProject project)
        {
            if (!base.CanCreateEntities(project)
                || project.Status == ProjectStatus.Closed)
            {
                return false;
            }

            var result = CommonSecurityManager.IsInTeam(project);

            return result;
        }

        public override bool CanReadEntities(DbProject project, Guid userId)
        {
            var result = _taskSecurityManager.CanReadEntities(project, userId);

            return result;
        }

        public override bool CanReadEntity(DbTimeTrackingItem entity, Guid userId)
        {
            var result = _taskSecurityManager.CanReadEntity(entity.RelativeTask, userId);

            return result;
        }

        public override bool CanUpdateEntity(DbTimeTrackingItem timeSpend)
        {
            if (!base.CanUpdateEntity(timeSpend) || timeSpend.PaymentStatus == PaymentStatus.Billed)
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(timeSpend.RelativeTask.Project))
            {
                return true;
            }

            var result = timeSpend.PersonId == CommonSecurityManager.CurrentUserId
                   || timeSpend.CreatorId == CommonSecurityManager.CurrentUserId;

            return result;
        }

        public override bool CanDeleteEntity(DbTimeTrackingItem timeSpend)
        {
            if (!base.CanDeleteEntity(timeSpend) || timeSpend.PaymentStatus == PaymentStatus.Billed)
            {
                return false;
            }

            if (CommonSecurityManager.IsProjectManager(timeSpend.RelativeTask.Project))
            {
                return true;
            }
            var result = CommonSecurityManager.IsInTeam(timeSpend.RelativeTask.Project)
                && (timeSpend.CreatorId == CommonSecurityManager.CurrentUserId
                    || timeSpend.PersonId== CommonSecurityManager.CurrentUserId);

            return result;
        }

        public bool CanEditPaymentStatus(DbTimeTrackingItem timeSpend)
        {
            var result = timeSpend != null
                && CommonSecurityManager.Can()
                && CommonSecurityManager.IsProjectManager(timeSpend.RelativeTask.Project);

            return result;
        }
    }
}
