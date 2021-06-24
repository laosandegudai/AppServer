using System.Runtime.Serialization;

using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.ViewModels
{
    public class TaskLinkViewModel
    {
        [DataMember]
        public int DependenceTaskId { get; set; }

        [DataMember]
        public int ParentTaskId { get; set; }

        [DataMember]
        public TaskLinkType LinkType { get; set; }
    }
}
