using System;
using System.Runtime.Serialization;

namespace ASC.Projects.ViewModels
{
    [DataContract(Name = "person", Namespace = "")]
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Title { get; set; }

        public string AvatarSmall { get; set; }

        public string ProfileUrl { get; set; }
    }
}
