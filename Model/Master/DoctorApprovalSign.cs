using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(DoctorApprovalSign))]
    public class DoctorApprovalSign:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorSign { get; set; }
        public int empid { get; set; }


    }
}
