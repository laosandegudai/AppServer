using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using ASC.Api.Core;
using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.ViewModels
{
    public class TaskViewModel : BaseFullDataViewModel
    {
        [DataMember]
        public bool CanEdit { get; set; }

        [DataMember]
        public bool CanCreateSubtask { get; set; }

        [DataMember]
        public bool CanCreateTimeSpend { get; set; }

        [DataMember]
        public bool CanDelete { get; set; }

        [DataMember]
        public bool CanReadFiles { get; set; }

        [DataMember(Order = 12, EmitDefaultValue = false)]
        public ApiDateTime Deadline { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ApiDateTime StartDate { get; set; }

        [DataMember(Order = 13, EmitDefaultValue = false)]
        public int MilestoneId { get; set; }

        [DataMember(Order = 12)]
        public TaskPriority Priority { get; set; }

        [DataMember(Order = 14)]
        public ProjectSimpleViewModel ProjectOwner { get; set; }

        [DataMember(Order = 15, EmitDefaultValue = false)]
        public int Progress { get; set; }

        [DataMember(Order = 20, EmitDefaultValue = false)]
        public List<SubtaskViewModel> Subtasks { get; set; }

        [DataMember(Order = 21, EmitDefaultValue = false)]
        public IEnumerable<TaskLinkViewModel> Links { get; set; }

        [DataMember(Order = 53)]
        public List<EmployeeViewModel> Responsibles { get; set; }

        [DataMember(Order = 53, EmitDefaultValue = false)]
        public List<Guid> ResponsibleIds { get; set; }

        [DataMember(Order = 54, EmitDefaultValue = false)]
        public SimpleMilestoneViewModel Milestone { get; set; }

        [DataMember(Order = 55, EmitDefaultValue = false)]
        public int? CustomTaskStatus { get; set; }
    }
}
