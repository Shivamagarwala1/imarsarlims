using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(formulaMaster))]
    public class formulaMaster:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int itemId { get; set; }
        [Required]
        public int observationId { get; set; }
        [Required, MaxLength(200)]
        public string formula { get; set; }
        [Required, MaxLength(500)]
        public string FormulaText { get; set; }
    }
}
