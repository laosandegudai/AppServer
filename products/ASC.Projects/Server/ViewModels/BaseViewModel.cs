using System;
using System.Runtime.Serialization;

namespace ASC.Projects.ViewModels
{
    [DataContract(Namespace = "")]
    public class BaseViewModel
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 10)]
        public string Title { get; set; }

        [DataMember(Order = 11)]
        public string Description { get; set; }

        [DataMember(Order = 20)]
        public int Status { get; set; }

        [DataMember(Order = 30, EmitDefaultValue = false)]
        public EmployeeViewModel Responsible { get; set; }

        [DataMember(Order = 30, EmitDefaultValue = false)]
        public Guid ResponsibleId { get; set; }
    }
}
