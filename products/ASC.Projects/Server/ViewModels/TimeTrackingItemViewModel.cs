using System;
using System.Runtime.Serialization;
using ASC.Api.Core;

namespace ASC.Projects.ViewModels
{
    [DataContract(Name = "time", Namespace = "")]
    public class TimeTrackingItemViewModel
    {
        public int Id { get; set; }

        public ApiDateTime Date { get; set; }

        public decimal Hours { get; set; }

        public string Note { get; set; }

        public int RelatedProjectId { get; set; }

        public int RelatedTaskId { get; set; }

        public string RelatedTaskTitle { get; set; }

        public TaskViewModel Task { get; set; }

        public EmployeeViewModel Creator { get; set; }

        public Guid PersonId { get; set; }

        public EmployeeViewModel Person { get; set; }

        public bool CanEdit { get; set; }

        public string PaymentStatus { get; set; }

        public ApiDateTime StatusChangeDate { get; set; }

        public bool CanEditPaymentStatus { get; set; }

        public ApiDateTime CreationDate { get; set; }

    }
}
