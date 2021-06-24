using System.Collections.Generic;
using System.Linq;
using ASC.Core;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    internal class ProjectRepository : BaseTenantRepository<DbProject, int>, IProjectRepository
    {
        public ProjectRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        public List<DbProject> GetProjectByIds(List<int> ids)
        {
            var projects = GetAll()
                .Where(p => ids.Contains(p.Id))
                .ToList();

            return projects;
        }
    }
}
