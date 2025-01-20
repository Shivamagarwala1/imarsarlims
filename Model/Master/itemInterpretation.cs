using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(itemInterpretation))]
    public class itemInterpretation : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int itemId { get; set; }
        [Required]
        public string interpretation { get; set; }
        [Required]
        public int centreId { get; set; }
        public int showInReport { get; set; }
        public int showinPackages { get; set; }
    }
}
