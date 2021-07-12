using ASC.Core;
using ASC.Core.Common.Utils;
using ASC.Projects.Configuration;
using ASC.Projects.Core.BusinessLogic.Managers.Interfaces;
using AutoMapper;

namespace ASC.Projects.Controllers
{
    public class ProjectTaskApiController : BaseApiController
    {
        #region Fields and .ctor

        private readonly IProjectTaskManager _taskManager;

        private readonly ISubtaskManager _subtaskManager;

        public ProjectTaskApiController(ProductEntryPoint productEntryPoint,
            SecurityContext securityContext,
            IProjectTaskManager taskManager,
            ISubtaskManager subtaskManager,
            IMapper mapper) : base(productEntryPoint, securityContext, mapper)
        {
            _taskManager = taskManager.NotNull(nameof(taskManager));
            _subtaskManager = subtaskManager.NotNull(nameof(subtaskManager));
        }

        #endregion Fields and .ctor
    }
}
