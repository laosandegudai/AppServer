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
using System.Linq;
using System.Web;
using ASC.Core;
using ASC.Core.Common.Extensions;
using ASC.Core.Common.Utils;
using ASC.Core.Tenants;
using ASC.ElasticSearch;
using ASC.MessagingSystem;
using ASC.Projects.Core.BusinessLogic.Converters.Interfaces;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.BusinessLogic.Notifications.Data;
using ASC.Projects.Core.BusinessLogic.Notifications.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    /// <summary>
    /// Business logic manager working with milestones.
    /// </summary>
    public class MilestoneManager : IMilestoneManager
    {
        private readonly IMilestoneRepository _milestoneRepository;

        private readonly IFactoryIndexer _factoryIndexer;

        private readonly IProjectNotificationSender _projectNotificationSender;

        private readonly IRecipientConverter _recipientConverter;
        
        private readonly IMapper _mapper;

        private readonly MessageService _messageService;

        private readonly SecurityContext _securityContext;

        public MilestoneManager(IMilestoneRepository milestoneRepository,
            IFactoryIndexer factoryIndexer,
            IProjectNotificationSender projectNotificationSender,
            IRecipientConverter recipientConverter,
            IMapper mapper,
            MessageService messageService,
            SecurityContext securityContext)
        {
            _milestoneRepository = milestoneRepository.NotNull(nameof(milestoneRepository));
            _factoryIndexer = factoryIndexer.NotNull(nameof(factoryIndexer));
            _projectNotificationSender = projectNotificationSender.NotNull(nameof(projectNotificationSender));
            _recipientConverter = recipientConverter.NotNull(nameof(recipientConverter));
            _mapper = mapper.NotNull(nameof(mapper));
            _messageService = messageService.NotNull(nameof(messageService));
            _securityContext = securityContext.NotNull(nameof(securityContext));
        }

        /// <summary>
        /// Receives a milestone with specified Id.
        /// </summary>
        /// <param name="id">Id of needed milestone.</param>
        /// <returns>Milestone <see cref="MilestoneData"/> with specified Id.</returns>
        public MilestoneData GetById(int id)
        {
            var milestone = _milestoneRepository.GetById(id);

            var result = _mapper.Map<DbMilestone, MilestoneData>(milestone);

            return result;
        }

        /// <summary>
        /// Receives all existing milestones.
        /// </summary>
        /// <returns>List of all existing milestones <see cref="MilestoneData"/>.</returns>
        public List<MilestoneData> GetAll()
        {
            var result = _milestoneRepository
                .GetAll()
                .Select(m => _mapper.Map<DbMilestone, MilestoneData>(m))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives all existing milestones which are satisfies specified filter.
        /// </summary>
        /// <returns>List of filtered milestones <see cref="MilestoneData"/>.</returns>
        // ToDo: implement this later.
        public List<MilestoneData> GetByFilter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates amount of milestones which are satisfies specified filter.
        /// </summary>
        /// <returns>Amount of filtered milestones <see cref="int"/>.</returns>
        // ToDo: implement this later.
        public int GetByFilterCount()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates amount of milestones which are satisfies specified filter. for report.
        /// </summary>
        /// <returns>Amount of filtered milestones <see cref="int"/>.</returns>
        // ToDo: implement this later.
        public List<Tuple<Guid, int, int>> GetByFilterCountForReport()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives a list of milestones, related to project with specified Id.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <returns>List of milestones <see cref="MilestoneData"/> related to project with specified Id.</returns>
        public List<MilestoneData> GetProjectMilestones(int projectId)
        {
            var milestones = _milestoneRepository
                .GetProjectMilestones(projectId)
                .Select(m => _mapper.Map<DbMilestone, MilestoneData>(m))
                .ToList();

            milestones.Sort((x, y) => x.Status != y.Status
                ? x.Status.CompareTo(y.Status)
                : x.Status == MilestoneStatus.Open
                    ? x.Deadline.CompareTo(y.Deadline)
                    : y.Deadline.CompareTo(x.Deadline));

            return milestones;
        }

        /// <summary>
        /// Receives a list of milestones, which are related to project with specified Id and having a specific status.
        /// </summary>
        /// <param name="projectId">Id of needed project.</param>
        /// <param name="milestoneStatus">Status of needed milestones.</param>
        /// <returns>List of milestones, which are related to specific project and having a specific status.</returns>
        public List<MilestoneData> GetProjectMilestonesWithStatus(int projectId, MilestoneStatus milestoneStatus)
        {
            var milestones = _milestoneRepository
                .GetProjectMilestonesWithStatus(projectId, milestoneStatus)
                .Select(m => _mapper.Map<DbMilestone, MilestoneData>(m))
                .ToList();

            milestones.Sort((x, y) => x.Status != y.Status
                ? x.Status.CompareTo(y.Status)
                : x.Status == MilestoneStatus.Open
                    ? x.Deadline.CompareTo(y.Deadline)
                    : y.Deadline.CompareTo(x.Deadline));

            return milestones;
        }

        /// <summary>
        /// Receives an upcoming milestones.
        /// </summary>
        /// <param name="max">Maximal amount of items at result.</param>
        /// <param name="projectIds">Project ids.</param>
        /// <returns>List of upcoming milestones <see cref="MilestoneData"/> of projects with specified ids.</returns>
        public List<MilestoneData> GetProjectUpcomingMilestones(int max, params int[] projectIds)
        {
            var offset = default(int);

            var milestones = new List<MilestoneData>();

            while (true)
            {
                var package = _milestoneRepository
                    .GetUpcomingMilestones(offset, 2 * max, projectIds)
                    .Select(m => _mapper.Map<DbMilestone, MilestoneData>(m))
                    .ToList();

                milestones.AddRange(package);

                if (max <= milestones.Count || package.Count < 2 * max)
                {
                    break;
                }

                offset += 2 * max;
            }

            var result = milestones.Count <= max
                ? milestones
                : milestones.GetRange(0, max);

            return result;
        }

        /// <summary>
        /// Receives a missed milestones.
        /// </summary>
        /// <param name="max">Maximal amount of items at result.</param>
        /// <returns>List of missed milestones <see cref="MilestoneData"/>.</returns>
        public List<MilestoneData> GetLateMilestones(int max)
        {
            var offset = 0;

            var milestones = new List<MilestoneData>();

            while (true)
            {
                var package = _milestoneRepository
                    .GetOverdueMilestones(offset, 2 * max)
                    .Select(m => _mapper.Map<DbMilestone, MilestoneData>(m))
                    .ToList();
                
                milestones.AddRange(package);

                if (max <= milestones.Count || package.Count < 2 * max)
                {
                    break;
                }

                offset += 2 * max;
            }

            var result = milestones.Count <= max
                ? milestones
                : milestones.GetRange(0, max);

            return result;
        }

        /// <summary>
        /// Receives milestones having specified deadline.
        /// </summary>
        /// <param name="deadline">Date of supposed deadline.</param>
        /// <returns>List of milestones <see cref="MilestoneData"/> having specified deadline.</returns>
        public List<MilestoneData> GetMilestonesWithDeadline(DateTime deadline)
        {
            var result = _milestoneRepository.GetMilestonesByDeadline(deadline)
                .Select(m => _mapper.Map<DbMilestone, MilestoneData>(m))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives tasks assigned to milestone with specified Id.
        /// </summary>
        /// <param name="milestoneId">Id of needed milestone.</param>
        /// <returns>List of tasks <see cref="TaskData"/> assigned to milestone with specified Id.</returns>
        public List<TaskData> GetMilestoneTasks(int milestoneId)
        {
            milestoneId.IsPositive(nameof(milestoneId));

            var tasks = _milestoneRepository
                .GetMilestoneTasks(milestoneId);

            var result = tasks
                ?.Select(t => _mapper.Map<DbProjectTask, TaskData>(t))
                .ToList();

            return result;
        }

        /// <summary>
        /// Creates new milestone or updates an existing milestone.
        /// </summary>
        /// <param name="milestone">Milestone.</param>
        /// <returns>Just created or updated milestone <see cref="MilestoneData"/>.</returns>
        public MilestoneData SaveOrUpdate(MilestoneData milestone)
        {
            var result = SaveOrUpdate(milestone, false);

            return result;
        }

        /// <summary>
        /// Creates new milestone or updates an existing milestone.
        /// </summary>
        /// <param name="milestone">Milestone.</param>
        /// <param name="isNotificationForResponsibleNeeded">Determines need of sending notifications after creation or update.</param>
        /// <returns>Just created or updated milestone <see cref="MilestoneData"/>.</returns>
        public MilestoneData SaveOrUpdate(MilestoneData milestone, bool isNotificationForResponsibleNeeded)
        {
            milestone.NotNull(nameof(milestone));

            if (milestone.Project == null)
            {
                throw new ArgumentException($"{nameof(milestone.Project)} is empty.");
            }

            if (milestone.ResponsibleId.Equals(Guid.Empty))
            {
                throw new ArgumentException($"{nameof(milestone.ResponsibleId)} is empty.");
            }
            
            // ToDo: implement this later.
            // check guest responsible
            //if (ProjectSecurity.IsVisitor(milestone.Responsible))
            //{
            //    ProjectSecurity.CreateGuestSecurityException();
            //}

            var isNew = milestone.Id == default;
            var previousResponsibleId = default(Guid);

            milestone.LastModificationDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local);
            milestone.LastEditorId = _securityContext.CurrentAccount.ID;

            if (isNew)
            {
                milestone.CreatorId = milestone.CreatorId == default
                    ? _securityContext.CurrentAccount.ID
                    : milestone.CreatorId;

                milestone.CreationDate = milestone.CreationDate == default
                    ? TenantUtil.DateTimeNow(TimeZoneInfo.Local)
                    : milestone.CreationDate;

                // ToDo: implement this later.
                //ProjectSecurity.DemandCreate<Milestone>(milestone.Project);

                var entity = _mapper.Map<MilestoneData, DbMilestone>(milestone);

                _milestoneRepository.Create(entity);

                milestone.Id = entity.Id;
            }
            else
            {
                var entity = _milestoneRepository
                    .GetById(milestone.Id);

                if (entity == null)
                {
                    throw new InvalidOperationException($"Milestone with ID = {milestone.Id} does not exists");
                }

                previousResponsibleId = milestone.ResponsibleId;

                // ToDo: implement this later.
                //ProjectSecurity.DemandEdit(milestone);

                entity.Description = milestone.Description;
                entity.Title = milestone.Title;
                entity.Deadline = milestone.Deadline;
                entity.ResponsibleId = milestone.ResponsibleId;
 
                _milestoneRepository.Update(entity);
            }

            var notificationData = new MilestoneNotificationData
            {
                InitiatorId = _securityContext.CurrentAccount.ID.ToString(),
                MilestoneId = milestone.Id,
                MilestoneTitle = milestone.Title,
                
                Description = milestone.Description.IsNullOrWhiteSpace()
                    ? string.Empty
                    : HttpUtility.HtmlEncode(milestone.Description),
                
                NotificationId = milestone.NotificationId,
                ProjectId = milestone.Project.Id,
                ProjectTitle = milestone.Project.Title,
                Recipients = new[] { _recipientConverter.Convert(milestone.Project.ResponsibleId) }
            };

            if (milestone.ResponsibleId == Guid.Empty)
            {
                return milestone;
            }

            if (isNew
                && milestone.ResponsibleId != _securityContext.CurrentAccount.ID
                && milestone.Project.ResponsibleId != milestone.ResponsibleId)
            {
                _projectNotificationSender.SendMilestoneCreatedNotification(notificationData);
            }

            if (!isNotificationForResponsibleNeeded || milestone.ResponsibleId == _securityContext.CurrentAccount.ID)
            {
                return milestone;
            }

            if (isNew || previousResponsibleId != milestone.ResponsibleId)
            {
                _projectNotificationSender.SendMilestoneResponsibleChangedNotification(notificationData);
            }
            else
            {
                _projectNotificationSender.SendMilestoneUpdatedNotification(notificationData);
            }

            // ToDo: implement this later.
            //FactoryIndexer<MilestonesWrapper>.IndexAsync(milestone);

            return milestone;
        }

        /// <summary>
        /// Changes milestone status.
        /// </summary>
        /// <param name="milestone">Updating milestone Id.</param>
        /// <param name="newStatus">New status for milestone.</param>
        /// <returns>Just updated milestone <see cref="MilestoneData"/>.</returns>
        public MilestoneData SetMilestoneStatus(int milestoneId, MilestoneStatus newStatus)
        {
            milestoneId.IsPositive(nameof(milestoneId));

            var milestone = _milestoneRepository
                .GetAll()
                .Include(m => m.Project)
                .FirstOrDefault(m => m.Id == milestoneId);

            if (milestone == null)
            {
				throw new InvalidOperationException($"A milestone with Id = {milestoneId} does not exists");
            }

            var milestoneData = _mapper.Map<DbMilestone, MilestoneData>(milestone);

            if (milestone.Status == newStatus)
            {
                return milestoneData;
            }

            if (milestone.Project.Status == ProjectStatus.Closed)
            {
                throw new InvalidOperationException();
            }

            if (milestoneData.ActiveTaskCount != default && newStatus == MilestoneStatus.Closed)
            {
                throw new Exception("Cannot close a milestone with open tasks");
            }

            milestone.Status = newStatus;
            milestone.LastEditorId = _securityContext.CurrentAccount.ID;
            milestone.LastModificationDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local);
            milestone.StatusChangeDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local);

            _milestoneRepository.Update(milestone);

            var recipients = new[] { milestone.Project.ResponsibleId, milestone.CreatorId, milestone.ResponsibleId }
                .Select(r => _recipientConverter.Convert(r.GetValueOrDefault()))
                .ToArray();

            var notificationData = new MilestoneNotificationData
            {
                InitiatorId = _securityContext.CurrentAccount.ID.ToString(),
                ProjectId = milestone.ProjectId,
                ProjectTitle = milestone.Project.Title,
                MilestoneId = milestone.Id,
                MilestoneTitle = milestone.Title,
                NotificationId = milestoneData.NotificationId,

                Description = milestone.Description.IsNullOrWhiteSpace()
                    ? string.Empty
                    : HttpUtility.HtmlEncode(milestone.Description),

                Recipients = recipients
            };

            switch (newStatus)
            {
                case MilestoneStatus.Closed when recipients.Length != 0:
                {
                    _projectNotificationSender.SendMilestoneClosingNotification(notificationData);

                    break;
                }
                case MilestoneStatus.Open when recipients.Length != 0:
                {
                    _projectNotificationSender.SendMilestoneResumedNotification(notificationData);

                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
                }
            }

            var result = _mapper.Map<DbMilestone, MilestoneData>(milestone);

            return result;
        }

        /// <summary>
        /// Removes an existing milestone.
        /// </summary>
        /// <param name="milestone">Removal milestone.</param>
        public void Delete(MilestoneData milestone)
        {
            milestone.NotNull(nameof(milestone));

            var entity = _milestoneRepository
                .GetAll()
                .Include(m => m.Project)
                .SingleOrDefault(m => m.Id == milestone.Id);

            if (entity == null)
            {
                throw new InvalidOperationException($"A milestone with ID = {milestone.Id} does not exists");
            }

            _milestoneRepository.Delete(entity);

            var recipients = new [] { entity.Project.ResponsibleId, entity.ResponsibleId }
                .Select(r => _recipientConverter.Convert(r.GetValueOrDefault()))
                .ToArray();

            var notificationData = new MilestoneNotificationData()
            {
                InitiatorId = _securityContext.CurrentAccount.ID.ToString(),
                MilestoneId = entity.Id,
                MilestoneTitle = entity.Title,
                ProjectId = entity.ProjectId,
                ProjectTitle = entity.Project.Title,
                Recipients = recipients,

                Description = entity.Description.IsNullOrWhiteSpace()
                    ? string.Empty
                    : HttpUtility.HtmlEncode(entity.Description)
            };

            _projectNotificationSender.SendMilestoneRemovalNotification(notificationData);

            // ToDo: implement this later.
            //FactoryIndexer<MilestonesWrapper>.DeleteAsync(milestone);
        }
    }
}
