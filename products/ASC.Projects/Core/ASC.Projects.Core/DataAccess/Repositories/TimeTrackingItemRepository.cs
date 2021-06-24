using System.Collections.Generic;
using System.Linq;
using ASC.Core;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.EF;
using ASC.Projects.Core.DataAccess.Repositories.Interfaces;

namespace ASC.Projects.Core.DataAccess.Repositories
{
    internal class TimeTrackingItemRepository : BaseTenantRepository<DbTimeTrackingItem, int>, ITimeTrackingItemRepository
    {
        public TimeTrackingItemRepository(ProjectsDbContext dbContext,
            TenantManager tenantManager) : base(dbContext, tenantManager) { }

        public List<DbTimeTrackingItem> GetByProject(int projectId)
        {
            throw new System.NotImplementedException();
        }

        public List<DbTimeTrackingItem> GetTaskTimeTrackingItems(int taskId)
        {
            var result = GetAll()
                .Where(t => t.RelativeTaskId == taskId)
                .ToList();

            return result;
        }
    }
}
