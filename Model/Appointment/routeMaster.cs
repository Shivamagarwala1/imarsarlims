using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Appointment
{
    [Table(nameof(routeMaster))]
    public class routeMaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(100)]
        public string routeName { get; set; }
        public int pincode { get; set; }
    }
}
