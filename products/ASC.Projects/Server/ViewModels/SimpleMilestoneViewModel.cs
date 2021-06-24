using System;
using System.Runtime.Serialization;

using ASC.Api.Core;

namespace ASC.Projects.ViewModels
{
    [DataContract(Name = "milestone", Namespace = "")]
    public class SimpleMilestoneViewModel
    {
        [DataMember(Order = 60)]
        public int Id { get; set; }

        [DataMember(Order = 61)]
        public string Title { get; set; }

        [DataMember(Order = 62)]
        public ApiDateTime Deadline { get; set; }

        public static SimpleMilestoneViewModel GetSample()
        {
            return new SimpleMilestoneViewModel
            {
                Id = 123,
                Title = "Milestone"
            };
        }
    }
}
