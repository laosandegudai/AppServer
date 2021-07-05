using System.Runtime.Serialization;

namespace ASC.Projects.ViewModels
{
    [DataContract(Name = "project", Namespace = "")]
    public class ProjectSimpleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public bool IsPrivate { get; set; }
    }
}
