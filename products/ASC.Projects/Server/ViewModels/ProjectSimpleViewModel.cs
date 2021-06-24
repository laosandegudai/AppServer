using System.Runtime.Serialization;

namespace ASC.Projects.ViewModels
{
    [DataContract(Name = "project", Namespace = "")]
    public class ProjectSimpleViewModel : BaseViewModel
    {
        [DataMember(Order = 31)]
        public bool CanEdit { get; set; }

        [DataMember(Order = 32)]
        public bool IsPrivate { get; set; }
    }
}
