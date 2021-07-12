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
using ASC.Projects.Core.DataAccess.Domain.Entities.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// An interface of base repository with typical CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity which repository is working with.</typeparam>
    /// <typeparam name="TKey">Type of entity key which repository is working with.</typeparam>
    public interface IRepository<TEntity, in TKey>
        where TKey : struct
        where TEntity : IBaseDbEntity<TKey>
    {
        /// <summary>
        /// Receives a full list of items expression.
        /// </summary>
        /// <returns>Full list of items <see cref="IQueryable{TEntity}"/> expression.</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Receives an item by id.
        /// </summary>
        /// <param name="id">Id of needed item.</param>
        /// <returns>Item <see cref="TEntity"/> having specified id.</returns>
        TEntity GetById(TKey id);

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="newItem">New item data.</param>
        /// <returns>Just created item <see cref="TEntity"/>.</returns>
        TEntity Create(TEntity newItem);

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="updatedItem">Updating item data.</param>
        /// <returns>Just updated item <see cref="TEntity"/>.</returns>
        TEntity Update(TEntity updatedItem);

        /// <summary>
        /// Removes a specified item.
        /// </summary>
        /// <param name="removalItem">Item to remove.</param>
        void Delete(TEntity removalItem);

        /// <summary>
        /// Removes an item having specified id.
        /// </summary>
        /// <param name="id">Id of item to remove.</param>
        void DeleteById(TKey id);

        /// <summary>
        /// Checks an existence of item with specified id.
        /// </summary>
        /// <param name="id">Id of item.</param>
        /// <returns>true if item with specified id exists, otherwise - false.</returns>
        bool Exists(TKey id);

        /// <summary>
        /// Calculates amount of items.
        /// </summary>
        /// <returns>Amount of items <see cref="int"/>.</returns>
        int Count();
    }
}
