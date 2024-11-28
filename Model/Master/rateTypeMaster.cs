using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(rateTypeMaster))]
    public class rateTypeMaster: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        [Required, MaxLength(50)]
        public string rateName { get; set; }
        [Required]
        public int rateTypeId { get; set; }
        [Required, MaxLength(50)]
        public string rateType { get; set; }
    }
}
