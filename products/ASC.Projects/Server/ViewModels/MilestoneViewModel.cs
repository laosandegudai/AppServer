using System;
using System.Runtime.Serialization;

using ASC.Projects.Core.DataAccess.Domain.Enums;

namespace ASC.Projects.ViewModels
{
    public class MilestoneViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public MilestoneStatus Status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public EmployeeViewModel Responsible { get; set; }

        public DateTime CreationDate { get; set; }

        public EmployeeViewModel Creator { get; set; }

        public DateTime LastModificationDate { get; set; }

        public EmployeeViewModel LastEditor { get; set; }

        public DateTime Deadline { get; set; }

        public ProjectSimpleViewModel ProjectOwner { get; set; }

        public bool IsKey { get; set; }

        public bool IsNotify { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public int ActiveTaskCount { get; set; }

        public int ClosedTaskCount { get; set; }
    }
}
