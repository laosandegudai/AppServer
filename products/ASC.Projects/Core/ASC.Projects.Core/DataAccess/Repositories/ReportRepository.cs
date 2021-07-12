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
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    /// <summary>
    /// A repository working with <see cref="DbReport"/> entity.
    /// </summary>
    internal class ReportRepository : BaseTenantRepository<DbReport, int>, IReportRepository
    {
        #region .ctor

        public ReportRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        #endregion .ctor

        /// <summary>
        /// Receives reports of user with specified id.
        /// </summary>
        /// <param name="userId">Id of needed user.</param>
        /// <returns>List of reports <see cref="List{DbReport}"/> of needed user.</returns>
        public List<DbReport> GetUserReports(Guid userId)
        {
            var result = GetAll()
                .Where(r => r.CreatorId == userId)
                .OrderByDescending(r => r.CreationDate)
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives report of user with specified id having specified file id.
        /// </summary>
        /// <param name="userId">Id of needed user.</param>
        /// <param name="fileId">Id of needed file.</param>
        /// <returns>User report <see cref="DbReport"/> having specified file id.</returns>
        public DbReport GetByFileId(Guid userId, int fileId)
        {
            var result = GetAll()
                .SingleOrDefault(r => r.CreatorId == userId && r.FileId == fileId);

            return result;
        }
    }
}
