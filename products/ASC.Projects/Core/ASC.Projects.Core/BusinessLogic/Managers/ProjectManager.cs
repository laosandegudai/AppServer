/*
 *
 * (c) Copyright Ascensio System Limited 2010-2021
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

using System;
using System.Collections.Generic;
using System.Linq;

using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Core.Tenants;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;
using AutoMapper;

namespace ASC.Projects.Core.BusinessLogic.Managers
{
    public class ProjectManager : IProjectManager
    {
        private readonly IMapper _mapper;

        private readonly IProjectRepository _projectRepository;

        private readonly SecurityContext _securityContext;

        private readonly TenantUtil _tenantUtil;

        // ToDo: user project security
        public ProjectManager(IMapper mapper,
            IProjectRepository projectRepository,
            SecurityContext securityContext,
            TenantUtil tenantUtil)
        {
            _mapper = mapper.NotNull(nameof(mapper));
            _projectRepository = projectRepository.NotNull(nameof(projectRepository));
            _securityContext = securityContext.NotNull(nameof(securityContext));
            _tenantUtil = tenantUtil.NotNull(nameof(tenantUtil));
        }

        public bool Exists(int projectId)
        {
            var result = _projectRepository.Exists(projectId);

            return result;
        }

    }
}
