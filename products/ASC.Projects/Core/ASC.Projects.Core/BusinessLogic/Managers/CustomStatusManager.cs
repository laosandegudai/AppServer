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

using System.Collections.Generic;
using System.Linq;
using ASC.Core.Common.Utils;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.BusinessLogic.Security;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    /// <summary>
    /// Business logic manager responsible for custom statuses processing.
    /// </summary>
    public class CustomStatusManager : ICustomStatusManager
    {
        #region Fields and .ctor

        private readonly ICustomTaskStatusRepository _customTaskStatusRepository;

        private readonly IMapper _mapper;

        private readonly SecurityManager _securityManager;

        public CustomStatusManager(ICustomTaskStatusRepository customTaskStatusRepository,
            IMapper mapper,
            SecurityManager securityManager)
        {
            _customTaskStatusRepository = customTaskStatusRepository.NotNull(nameof(customTaskStatusRepository));
            _mapper = mapper.NotNull(nameof(mapper));
            _securityManager = securityManager.NotNull(nameof(securityManager));
        }


        #endregion Fields and .ctor

        /// <summary>
        /// Receives a full list of statuses.
        /// </summary>
        /// <returns>Full list of statuses <see cref="CustomTaskStatusData"/>.</returns>
        public List<CustomTaskStatusData> GetAll()
        {
            var result = _customTaskStatusRepository
                .GetAll()
                .Select(st => _mapper.Map<DbCustomTaskStatus, CustomTaskStatusData>(st))
                .ToList();

            return result;
        }

        /// <summary>
        /// Receives a list of default statuses.
        /// </summary>
        /// <returns>List of statuses <see cref="CustomTaskStatusData"/>.</returns>
        public List<CustomTaskStatusData> GetWithDefaults()
        {
            var customStatuses = GetAll();

            if (!customStatuses.Any(st => st.StatusType == TaskStatus.Open && st.IsDefault))
            {
                customStatuses.Add(CustomTaskStatusData.GetDefault(TaskStatus.Open));
            }

            if (!customStatuses.Any(st => st.StatusType == TaskStatus.Closed && st.IsDefault))
            {
                customStatuses.Add(CustomTaskStatusData.GetDefault(TaskStatus.Closed));
            }

            return customStatuses;
        }

        /// <summary>
        /// Creates a new status.
        /// </summary>
        /// <param name="status">Status to create.</param>
        /// <returns>Just created status <see cref="CustomTaskStatusData"/>.</returns>
        public CustomTaskStatusData Create(CustomTaskStatusData status)
        {
            status.NotNull(nameof(status));

            if (!_securityManager.CurrentUserAdministrator)
            {
                SecurityManager.CreateSecurityException();
            }

            var statuses = _customTaskStatusRepository
                .GetAll()
                .Where(st => st.StatusType == (int) status.StatusType)
                .OrderBy(st => st.Order)
                .ToList();

            var lastStatus = statuses.LastOrDefault();

            status.Order = lastStatus?.Order + 1 ?? 1;

            status.IsAvailable = statuses
                .All(st => st.IsAvailable);

            var entity = _mapper.Map<CustomStatusData, DbCustomTaskStatus>(status);

            var newStatus = _customTaskStatusRepository.Create(entity);

            var result = _mapper.Map<DbCustomTaskStatus, CustomTaskStatusData>(newStatus);

            return result;
        }

        /// <summary>
        /// Updates an existing status.
        /// </summary>
        /// <param name="status">Status to update.</param>
        /// <returns>Just updated status <see cref="CustomStatusData"/>.</returns>
        public CustomTaskStatusData Update(CustomTaskStatusData status)
        {
            status.NotNull(nameof(status));

            if (!_securityManager.CurrentUserAdministrator)
            {
                SecurityManager.CreateSecurityException();
            }

            var statuses = _customTaskStatusRepository
                .GetAll()
                .Where(st => st.StatusType == (int) status.StatusType)
                .OrderBy(st => st.Order);

            status.IsAvailable = statuses
                .All(st => st.IsAvailable);

            var updateEntity = _mapper.Map<CustomStatusData, DbCustomTaskStatus>(status);

            _customTaskStatusRepository.Update(updateEntity);

            return status;
        }

        /// <summary>
        /// Removes a status having specified id.
        /// </summary>
        /// <param name="id">Id of status to remove.</param>
        public void DeleteById(int id)
        {
            if (!_securityManager.CurrentUserAdministrator)
            {
                SecurityManager.CreateSecurityException();
            }

            var defaultTask = CustomTaskStatusData
                .GetDefaults()
                .FirstOrDefault(r => r.Id == id);

            if (defaultTask != null)
            {
                return;
            }

            _customTaskStatusRepository.DeleteById(id);
        }
    }
}
