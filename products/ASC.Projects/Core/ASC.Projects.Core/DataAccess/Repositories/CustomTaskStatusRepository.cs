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
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// Repository working with <see cref="DbCustomTaskStatus"/> entity.
    /// </summary>
    internal class CustomTaskStatusRepository : BaseTenantRepository<DbCustomTaskStatus, int>, ICustomTaskStatusRepository
    {
        #region .ctor

        public CustomTaskStatusRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        #endregion .ctor

        /// <summary>
        /// Updates an existing custom task status.
        /// </summary>
        /// <param name="updatedItem">Updating item data.</param>
        /// <returns>Just updated item <see cref="DbCustomTaskStatus"/>.</returns>
        public override DbCustomTaskStatus Update(DbCustomTaskStatus updatedItem)
        {
            var entity = GetById(updatedItem.Id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Custom task status with ID = {updatedItem.Id} does not exists.");
            }

            entity.Title = updatedItem.Title;
            entity.Description = updatedItem.Description;
            entity.Image = updatedItem.Image;
            entity.ImageType = updatedItem.ImageType;
            entity.Color = updatedItem.Color;
            entity.Order = updatedItem.Order;
            entity.StatusType = updatedItem.StatusType;
            entity.IsAvailable = updatedItem.IsAvailable;

            var result = base.Update(updatedItem);

            return result;
        }
    }
}
