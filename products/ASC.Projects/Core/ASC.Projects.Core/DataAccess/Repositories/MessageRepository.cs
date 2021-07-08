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
using System.Linq.Expressions;
using ASC.Core;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// Repository working with project messages.
    /// </summary>
    internal class MessageRepository : BaseTenantRepository<DbMessage, int>, IMessageRepository
    {
        public MessageRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        /// <summary>
        /// Receives all messages related to project.
        /// </summary>
        /// <param name="projectId">Id of project.</param>
        /// <returns>List of messages <see cref="DbMessage"/> related to project with specified Id.</returns>
        public List<DbMessage> GetProjectMessages(int projectId)
        {
            var result = GetAll()
                .Where(m => m.ProjectId == projectId)
                .OrderByDescending(m => m.CreationDate)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives needed amount of messages.
        /// </summary>
        /// <param name="skip">Amount of messages in result to skip.</param>
        /// <param name="take">Maximal amount of messages.</param>
        /// <returns>List of needed amount of messages <see cref="DbMessage"/>.</returns>
        public List<DbMessage> GetMessages(int startIndex, int maxResult)
        {
            var result = base.GetAll()
                .OrderByDescending(m => m.CreationDate)
                .Skip(startIndex)
                .Take(maxResult)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives needed amount of recent messages of specific projects.
        /// </summary>
        /// <param name="offset">Amount of messages in result to skip.</param>
        /// <param name="max">Maximal amount of messages.</param>
        /// <param name="projectIds">List of needed projects Ids.</param>
        /// <returns>List of needed amount of messages <see cref="DbMessage"/>.</returns>
        public List<DbMessage> GetRecentMessages(int offset, int max, params int[] projectIds)
        {
            var predicate = GetAll();

            if (projectIds?.Any() == true)
            {
                predicate = predicate.Where(m => projectIds.Contains(m.ProjectId));
            }

            var result = predicate
                .Skip(offset)
                .Take(max)
                .OrderByDescending(m => m.CreationDate)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives messages which are satisfying specified filter.
        /// </summary>
        /// <returns>List of messages satisfying specified filter.</returns>
        /// ToDo: think about and implement this later.
        public List<DbMessage> GetByFilter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives amount of messages which are satisfying specified filter.
        /// </summary>
        /// <returns>Amount of messages which are satisfying specified filter.</returns>
        /// ToDo: think about and implement this later.
        public int GetByFilterCount()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Receives amount of messages which are satisfying specified filter.
        /// </summary>
        /// <returns>Amount of messages which are satisfying specified filter.</returns>
        /// ToDo: think about and implement this later.
        public List<Tuple<Guid, int, int>> GetByFilterCountForReport()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing message.
        /// </summary>
        /// <param name="updatedItem">Updated message.</param>
        /// <returns>Just updated message.</returns>
        public override DbMessage Update(DbMessage updatedItem)
        {
            var entity = GetById(updatedItem.Id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Message with ID = {updatedItem.Id} does not exists.");
            }

            entity.ProjectId = updatedItem.ProjectId;
            entity.Title = updatedItem.Title;
            entity.Status = updatedItem.Status;
            entity.CreationDate = updatedItem.CreationDate;
            entity.CreatorId = updatedItem.CreatorId;
            entity.LastModificationDate = updatedItem.LastModificationDate;
            entity.LastEditorId = updatedItem.LastEditorId;
            entity.Content = updatedItem.Content;

            var result = base.Update(updatedItem);

            return result;
        }

        /// <summary>
        /// Receives a list of messages which are satisfying specified expression.
        /// </summary>
        /// <param name="predicate">An expression with condition.</param>
        /// <returns>Receives a list of messages which are satisfying specified expression.</returns>
        public List<DbMessage> Where(Expression<Func<DbMessage, bool>> predicate)
        {
            var result = GetAll()
                .Where(predicate)
                .ToList();

            return result;
        }
    }
}
