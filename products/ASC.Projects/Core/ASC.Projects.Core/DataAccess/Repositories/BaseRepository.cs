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

using System.Linq;
using ASC.Core.Common.Utils;
using ASC.Projects.Core.DataAccess.Domain.Entities.Interfaces;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    internal class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IBaseDbEntity<TKey>
        where TKey : struct
    {
        protected readonly ProjectsDbContext DbContext;

        public BaseRepository(ProjectsDbContext dbContext)
        {
            DbContext = dbContext.NotNull(nameof(dbContext));
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            var set = DbContext.Set<TEntity>()
                .AsQueryable();

            return set;
        }

        public virtual TEntity GetById(TKey id)
        {
            var result = DbContext.Set<TEntity>()
                .FirstOrDefault(e => e.Id.Equals(id));

            return result;
        }

        public virtual TEntity Create(TEntity newItem)
        {
            DbContext.Set<TEntity>().Add(newItem);

            DbContext.SaveChanges();

            return newItem;
        }

        public virtual TEntity Update(TEntity updatedItem)
        {
            DbContext.Set<TEntity>().Update(updatedItem);

            DbContext.SaveChanges();

            return updatedItem;
        }

        public virtual void Delete(TEntity removalItem)
        {
            DbContext.Set<TEntity>()
                .Remove(removalItem);

            DbContext.SaveChanges();
        }

        public virtual void DeleteById(TKey id)
        {
            DbContext.Set<TEntity>()
                .RemoveRange(DbContext.Set<TEntity>()
                    .Where(e => e.Id.Equals(id)));

            DbContext.SaveChanges();
        }

        public bool Exists(TKey itemId)
        {
            var item = GetById(itemId);

            return item != null;
        }

        public int Count()
        {
            var result = GetAll().Count();

            return result;
        }
    }
}
