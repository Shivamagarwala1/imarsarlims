using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(doctorApprovalDepartments))]
    public class doctorApprovalDepartments : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int doctorId { get; set; }
        [Required]
        public int centreId { get; set; }
        [Required]
        public int departmentId { get; set; }
        [Required]
        public int empId { get; set; }
        public TimeSpan? fromTime { get; set; }

        public TimeSpan? toTime { get; set; }
    }
}
