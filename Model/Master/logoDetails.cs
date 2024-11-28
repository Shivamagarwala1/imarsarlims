using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(logoDetails))]
    public class logoDetails :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        [Required, MaxLength(100)]
        public string imageName { get; set; }
        [Required, MaxLength(50)]
        public string logoType { get; set; }
        [MaxLength(50)]
        public string? remarks { get; set; }
        [Required]
        public int status { get; set; }
        [ MaxLength(50)]
        public string? logoDescription { get; set; }
    }
}
