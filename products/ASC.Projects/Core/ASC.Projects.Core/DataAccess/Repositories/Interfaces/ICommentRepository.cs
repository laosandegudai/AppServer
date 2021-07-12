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
using System.Linq.Expressions;
using ASC.Projects.Core.DataAccess.Domain.Entities;

namespace ASC.Projects.Core.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// An interface of repository working with <see cref="DbComment"/> entity.
    /// </summary>
    public interface ICommentRepository : IRepository<DbComment, Guid>
    {
        /// <summary>
        /// Receives a list of comments left for target with specified id.
        /// </summary>
        /// <param name="targetUniqueId">Id of needed target.</param>
        /// <returns>List of comments <see cref="DbComment"/> left for target with specified id.</returns>
        List<DbComment> GetAll(string targetUniqueId);

        /// <summary>
        /// Receives a list of comments, which are satisfies specified condition expression.
        /// </summary>
        /// <param name="where">Condition expression.</param>
        /// <returns>List of comments <see cref="DbComment"/>, which are satisfies specified condition expression</returns>
        List<DbComment> Where(Expression<Func<DbComment, bool>> where);

        /// <summary>
        /// Calculates an amount of comments with specified target id.
        /// </summary>
        /// <param name="targetUniqueId">Unique id of needed target.</param>
        /// <returns>An amount of comments with specified target id</returns>
        int Count(string targetUniqueId);
    }
}
