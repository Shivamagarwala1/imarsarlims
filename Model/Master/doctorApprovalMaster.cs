using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(doctorApprovalMaster))]
    public class doctorApprovalMaster: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int centreId { get; set; }
        [Required]
        public int empId { get; set; }
        [Required, MaxLength(50)]
        public string empName { get; set; }
        [Required, MaxLength(50)]
        public string doctorName { get; set; }
        public string signature { get; set; }
        public byte approve { get; set; }
        public byte notApprove { get; set; }
        public byte hold { get; set; }
        public byte unHold { get; set; }
        [Required]
        public int doctorId { get; set; }
        public int? isSign { get; set; }
    }
}
