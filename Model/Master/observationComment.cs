using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(observationComment))]
    public class observationComment: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int testId { get; set; }
        [Required]
        public int observationId { get; set; }
        [Required]
        public string comment { get; set; }
    }
}
