using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(centreLedgerRemarks))]
    public class centreLedgerRemarks: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int? centreId { get; set; }
        [Required,MaxLength(100)]
        public string? remarks { get; set; }
    }
}
