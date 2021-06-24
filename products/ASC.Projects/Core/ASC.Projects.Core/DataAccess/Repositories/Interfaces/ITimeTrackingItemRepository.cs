using System.Collections.Generic;
using ASC.Projects.Core.DataAccess.Domain.Entities;

namespace ASC.Projects.Core.DataAccess.Repositories.Interfaces
{
    public interface ITimeTrackingItemRepository : IRepository<DbTimeTrackingItem, int>
    {
        List<DbTimeTrackingItem> GetByProject(int projectId);

        List<DbTimeTrackingItem> GetTaskTimeTrackingItems(int taskId);
    }
}
