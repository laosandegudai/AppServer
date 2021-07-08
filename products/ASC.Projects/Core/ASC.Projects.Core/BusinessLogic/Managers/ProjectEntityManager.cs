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
using ASC.Notify.Model;
using ASC.Notify.Recipients;
using ASC.Projects.Core.BusinessLogic.Data;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    public class ProjectEntityManager
    {
        protected readonly ISubscriptionProvider SubscriptionProvider;

        protected readonly IRecipientProvider RecipientProvider;

        protected readonly INotifyAction NotifyAction;

        protected readonly SecurityContext SecurityContext;

        protected bool DisableNotifications;

        public ProjectEntityManager(INotifySource notifySource,
            INotifyAction notifyAction,
            SecurityContext securityContext,
            bool disableNotifications)
        {
            SubscriptionProvider = notifySource.GetSubscriptionProvider();
            RecipientProvider = notifySource.GetRecipientsProvider();
            NotifyAction = notifyAction.NotNull(nameof(notifyAction));
            SecurityContext = securityContext.NotNull(nameof(securityContext));
            DisableNotifications = disableNotifications;
        }

        public virtual ProjectEntityData GetEntityByID(int id)
        {
            return null;
        }

        public void Subscribe(ProjectEntityData entity, Guid recipientId)
        {
            var recipient = RecipientProvider.GetRecipient(recipientId.ToString());

            if (recipient == null)
            {
                return;
            }

            if (!IsUnsubscribed(entity, recipientId) || entity.CanEdit())
            {
                SubscriptionProvider.Subscribe(NotifyAction, entity.NotifyId, recipient);
            }
        }

        public void UnSubscribe(ProjectEntityData entity)
        {
            UnSubscribe(entity, SecurityContext.CurrentAccount.ID);
        }

        public void UnSubscribe(ProjectEntityData entity, Guid recipientId)
        {
            var recipient = RecipientProvider.GetRecipient(recipientId.ToString());

            if (recipient == null)
            {
                return;
            }

            SubscriptionProvider.UnSubscribe(NotifyAction, entity.NotifyId, recipient);
        }

        public void UnSubscribeAll<T>(T entity) where T : ProjectEntityData
        {
            SubscriptionProvider.UnSubscribe(NotifyAction, entity.NotifyId);
        }

        public void UnSubscribeAll<T>(List<T> entity) where T : ProjectEntityData
        {
            entity.ForEach(UnSubscribeAll);
        }

        public bool IsSubscribed(ProjectEntityData entity)
        {
            var result = IsSubscribed(entity, SecurityContext.CurrentAccount.ID);

            return result;
        }

        public bool IsSubscribed(ProjectEntityData entity, Guid recipientId)
        {
            var recipient = RecipientProvider.GetRecipient(recipientId.ToString());

            var objects = SubscriptionProvider.GetSubscriptions(NotifyAction, recipient);

            var result = objects
                .Any(item => string.Compare(item, entity.NotifyId, StringComparison.OrdinalIgnoreCase) == default);

            return result;
        }

        public bool IsUnsubscribed(ProjectEntityData entity, Guid recipientId)
        {
            var recipient = RecipientProvider.GetRecipient(recipientId.ToString());

            var result = recipient != null
                && SubscriptionProvider.IsUnsubscribe((IDirectRecipient)recipient, NotifyAction, entity.NotifyId);

            return result;
        }

        public void Follow(ProjectEntityData entity)
        {
            Follow(entity, SecurityContext.CurrentAccount.ID);
        }

        public void Follow(ProjectEntityData entity, Guid recipientId)
        {
            var recipient = RecipientProvider.GetRecipient(recipientId.ToString());

            if (recipient == null)
            {
                return;
            }

            if (!IsSubscribed(entity, recipientId))
            {
                SubscriptionProvider.Subscribe(NotifyAction, entity.NotifyId, recipient);
            }
            else
            {
                SubscriptionProvider.UnSubscribe(NotifyAction, entity.NotifyId, recipient);
            }
        }

        public List<IRecipient> GetSubscribers(ProjectEntityData entity)
        {
            var result = SubscriptionProvider
                .GetRecipients(NotifyAction, entity.NotifyId)
                .ToList();

            return result;
        }
    }
}
