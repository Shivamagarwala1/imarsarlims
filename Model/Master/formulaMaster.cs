using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(formulaMaster))]
    public class formulaMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        [Required]
        public int itemId { get; set; }
        [Required]
        public int labTestId { get; set; }
        [Required, MaxLength(200)]
        public string formula { get; set; }
    }
}
