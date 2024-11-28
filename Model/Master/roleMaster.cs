using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(roleMaster))]
    public class roleMaster: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required, MaxLength(30)]
        public string? roleName { get; set; }
        [Required]
        public byte isSalesRole { get; set; }
    }
}
