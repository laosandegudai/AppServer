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
using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Core.Tenants;
using ASC.Notify.Model;
using ASC.Notify.Recipients;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    /// <summary>
    /// Business logic manager responsible for messages processing.
    /// </summary>
    public class MessageManager : ProjectEntityManager, IMessageManager
    {
        #region Fields and .ctor

        private readonly IMessageRepository _messageRepository;
        
        private readonly IMapper _mapper;

        public MessageManager(INotifySource notifySource,
            INotifyAction notifyAction,
            IMessageRepository messageRepository,
            IMapper mapper,
            SecurityContext securityContext,
            bool disableNotifications) : base(notifySource, notifyAction, securityContext, disableNotifications)
        {
            _messageRepository = messageRepository.NotNull(nameof(messageRepository));
            _mapper = mapper;
        }

        #endregion Fields and .ctor;

        public override ProjectEntityData GetEntityByID(int id)
        {
            var projectEntity = GetById(id);

            return projectEntity;
        }

        public MessageData GetById(int id)
        {
            var result = GetById(id, true);

            return result;
        }

        public MessageData GetById(int id, bool checkSecurity)
        {
            var message = _messageRepository
                .GetAll()
                .Include(m => m.Comments)
                .FirstOrDefault(m => m.Id == id);

            var messageData = _mapper.Map<DbMessage, MessageData>(message);

            messageData.CommentsCount = message?.Comments?.Count ?? default;

            if (!checkSecurity)
            {
                return messageData;
            }

            var result = CanRead(messageData)
                ? messageData
                : null;

            return result;
        }

        public List<MessageData> GetAll()
        {
            var messages = _messageRepository
                .GetAll()
                .Select(m => _mapper.Map<DbMessage, MessageData>(m))
                .ToList();

            return messages;
        }

        public List<MessageData> GetProjectMessages(int projectId)
        {
            var result = _messageRepository
                .GetProjectMessages(projectId)
                .Select(m => _mapper.Map<DbMessage, MessageData>(m))
                .ToList();

            return result;
        }

        public List<MessageData> GetByFilter()
        {
            throw new NotImplementedException();
        }

        public int GetByFilterCount()
        {
            throw new NotImplementedException();
        }

        public List<Tuple<Guid, int, int>> GetByFilterCountForReport()
        {
            throw new NotImplementedException();
        }

        public bool Exists(int messageId)
        {
            var exists = _messageRepository.Exists(messageId);

            return exists;
        }

        public bool CanRead(MessageData message)
        {
            throw new NotImplementedException();
        }

        public MessageData SaveOrUpdate(MessageData message,
            bool needToNotify,
            List<Guid> participants,
            List<int> fileIds)
        {
            message.NotNull(nameof(message));

            var isNewItem = message.Id == default;

            var entity = _mapper.Map<MessageData, DbMessage>(message);

            entity.LastEditorId = SecurityContext.CurrentAccount.ID;
            entity.LastModificationDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local);

            if (isNewItem)
            {
                if (entity.CreatorId == default)
                {
                    entity.CreatorId = SecurityContext.CurrentAccount.ID;
                }

                if (entity.CreationDate == default)
                {
                    entity.CreationDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local);
                }

                _messageRepository.Create(entity);
            }
            else
            {
                _messageRepository.Update(entity);
            }

            var result = _mapper.Map<DbMessage, MessageData>(entity);

            return result;
        }

        public MessageData ChangeStatus(MessageData message)
        {
            message.NotNull(nameof(message));

            message.LastEditorId = SecurityContext .CurrentAccount.ID;
            message.LastModificationDate = TenantUtil.DateTimeNow(TimeZoneInfo.Local);

            var entity = _mapper.Map<MessageData, DbMessage>(message);

            _messageRepository.Update(entity);

            var result = _mapper.Map<DbMessage, MessageData>(entity);

            return result;
        }

        public void Delete(MessageData message)
        {
            message.NotNull(nameof(message));
            message.Project.NotNull(nameof(message.Project));

            _messageRepository.DeleteById(message.Id);
        }

        public override List<IRecipient> GetSubscribers(ProjectEntityData projectEntity)
        {
            var result = base.GetSubscribers(projectEntity);

            return result;
        }
    }
}
