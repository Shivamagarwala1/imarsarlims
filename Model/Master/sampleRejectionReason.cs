using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(sampleRejectionReason))]
    public class sampleRejectionReason: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        [Required, MaxLength(200)]
        public string? rejectionReason { get; set; }
    }
}
