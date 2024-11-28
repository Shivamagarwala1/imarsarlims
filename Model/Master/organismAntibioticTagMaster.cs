using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(organismAntibioticTagMaster))]
    public class organismAntibioticTagMaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short id { get; set; }
        [Required]
        public int organismId { get; set; }
        [Required]
        public int antibiticId { get; set; }
        [Required]
        public int? centreId { get; set; }
    }
}
