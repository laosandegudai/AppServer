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
using ASC.Projects.Core.DataAccess.Domain.Entities.Interfaces;
using ASC.Projects.Core.DataAccess.EF;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    public class BaseTenantRepository<TEntity, TKey> : BaseRepository<TEntity, TKey>
        where TEntity : class, ITenantEntity<TKey>
        where TKey : struct
    {
        protected readonly TenantManager TenantManager;

        protected int TenantId => TenantManager.GetCurrentTenant()
            ?.TenantId
            ?? throw new InvalidOperationException("Cannot determine user tenant");

        public BaseTenantRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext)
        {
            TenantManager = tenantManager.NotNull(nameof(tenantManager));
        }

        public override IQueryable<TEntity> GetAll()
        {
            var predicate = DbContext
                .Set<TEntity>()
                .Where(e => e.TenantId == TenantId);

            return predicate;
        }

        public override TEntity GetById(TKey id)
        {
            var result = DbContext
                .Set<TEntity>()
                .FirstOrDefault(e => e.Id.Equals(id) && e.TenantId == TenantId);

            return result;
        }

        public override TEntity Create(TEntity newItem)
        {
            newItem.TenantId = TenantId;

            base.Create(newItem);

            return newItem;
        }

        public override TEntity Update(TEntity updatedItem)
        {
            DbContext.UpdateRange(DbContext
                .Set<TEntity>()
                .Where(e => e.TenantId == updatedItem.TenantId && e.Id.Equals(updatedItem.Id)));

            DbContext.SaveChanges();

            return updatedItem;
        }

        public override void DeleteById(TKey id)
        {
            DbContext.Set<TEntity>()
                .RemoveRange(DbContext.Set<TEntity>()
                    .Where(e => e.TenantId == TenantId && e.Id.Equals(id)));
        }
    }
}
