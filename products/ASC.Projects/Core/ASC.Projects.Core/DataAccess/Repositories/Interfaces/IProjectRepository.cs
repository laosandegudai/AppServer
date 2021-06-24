using System.Collections.Generic;
using ASC.Projects.Core.DataAccess.Domain.Entities;

namespace ASC.Projects.Core.DataAccess.Repositories.Interfaces
{
    public interface IProjectRepository : IRepository<DbProject, int>
    {
        List<DbProject> GetProjectByIds(List<int> ids);
    }
}
