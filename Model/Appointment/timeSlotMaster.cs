using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Appointment
{
    [Table(nameof(timeSlotMaster))]
    public class timeSlotMaster:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int centreId { get; set; }
        public string timeslot { get; set; }

    }
}
