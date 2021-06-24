using System.Runtime.Serialization;

namespace ASC.Projects.ViewModels
{
    [DataContract(Name = "subtask", Namespace = "")]
    public class SubtaskViewModel : BaseFullDataViewModel
    {
        [DataMember]
        public bool CanEdit { get; set; }

        [DataMember]
        public int TaskId { get; set; }
    }
}
