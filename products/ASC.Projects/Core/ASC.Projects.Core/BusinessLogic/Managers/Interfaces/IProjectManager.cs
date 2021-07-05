using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Projects.Core.BusinessLogic.Managers.Interfaces
{
    public interface IProjectManager
    {
        bool Exists(int projectId);

        bool IsInTeam(int projectId, Guid participantId);
    }
}
