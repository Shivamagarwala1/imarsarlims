using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(itemCommentMaster))]
    public class itemCommentMaster : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required, MaxLength(20)]
        public string? type { get; set; }
        [Required]
        public int? itemId { get; set; }
        [Required]
        public int? observationId { get; set; }
        [Required, MaxLength(30)]
        public string? templateName { get; set; }
        [Required ,MaxLength]
        public string? template { get; set; }
    }
}
