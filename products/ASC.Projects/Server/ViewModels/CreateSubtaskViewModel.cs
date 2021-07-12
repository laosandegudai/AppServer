using System;

namespace ASC.Projects.ViewModels
{
    public class CreateSubtaskViewModel
    {
        public int TaskId { get; set; }

        public Guid ResponsibleId { get; set; }

        public string Title { get; set; }
    }
}
