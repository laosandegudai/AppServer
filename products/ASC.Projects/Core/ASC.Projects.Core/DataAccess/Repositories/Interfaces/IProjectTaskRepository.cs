using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Projects.Core.DataAccess.Domain.Entities;

namespace ASC.Projects.Core.DataAccess.Repositories.Interfaces
{
    public interface IProjectTaskRepository : IRepository<DbProjectTask, int>
    {
    }
}
