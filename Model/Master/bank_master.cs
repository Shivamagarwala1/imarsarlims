using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(bank_master))]
    public class bank_master: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int id { get; set; }
        [Required, MaxLength(50)]
        public string bankName { get; set; }
        [Required]
        public byte type { get; set; }
    }
}
