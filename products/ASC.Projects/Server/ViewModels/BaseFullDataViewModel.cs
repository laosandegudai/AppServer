using System;
using System.Runtime.Serialization;

using ASC.Api.Core;

namespace ASC.Projects.ViewModels
{
    public class BaseFullDataViewModel : BaseViewModel
    {
        [DataMember(Order = 50)]
        public ApiDateTime Created { get; set; }

        [DataMember(Order = 51, EmitDefaultValue = false)]
        public EmployeeViewModel CreatedBy { get; set; }

        [DataMember(Order = 51, EmitDefaultValue = false)]
        public Guid CreatedById { get; set; }

        private ApiDateTime updated;

        [DataMember(Order = 52, EmitDefaultValue = false)]
        public ApiDateTime Updated
        {
            get => updated < Created
                ? Created
                : updated;

            set => updated = value;
        }

        [DataMember(Order = 41, EmitDefaultValue = false)]
        public EmployeeViewModel UpdatedBy { get; set; }

        [DataMember(Order = 41, EmitDefaultValue = false)]
        public Guid UpdatedById { get; set; }
    }
}
