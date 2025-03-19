using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Appointment
{
    [Table(nameof(appointmentBooking))]
    public class appointmentBooking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int appointmentId { get; set; }
        [ForeignKey("tnx_Booking")]
        public int transactionId { get; set; }
        [MaxLength(15)]
        public string WorkorderId { get; set; }
        public int AppointmentType { get; set; }
        public DateTime AppointmentScheduledOn { get; set; }
        [MaxLength(6)]
        public string Pincode { get; set; }
        public int Status { get; set; }
        public int? AssignedPhlebo {  get; set; }
        public int? assignedBy { get; set; }
        public int? cancleBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? rescheduleDate { get; set; }
        public int rescheduleBy { get; set; }

    }
}
