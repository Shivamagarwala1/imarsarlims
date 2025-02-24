using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(LegendColorMaster))]
    public class LegendColorMaster:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required,MaxLength(20)]
        public string colourCode { get; set; }
        [Required, MaxLength(30)]
        public string ColourName { get; set; }
        [Required, MaxLength(30)]
        public string contantName { get; set; }
        [Required, MaxLength(15)]
        public string ShortName { get; set; }

    }
}
