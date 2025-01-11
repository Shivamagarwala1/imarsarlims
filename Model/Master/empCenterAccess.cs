using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(empCenterAccess))]
    public class empCenterAccess : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [ForeignKey("empMaster")]
        public int empId { get; set; }
        [Required]
        [ForeignKey("centreMaster")]
        public int centreId { get; set; }
    }
}
