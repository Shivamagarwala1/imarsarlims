using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(rateTypeTagging))]
    public class rateTypeTagging
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int rateTypeId { get; set; }
        [Required]
        public int centreId { get; set; }
    }
}
