using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(ThemeColour))]
    public class ThemeColour
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(20)]
        public string headerColor { get; set; }
        [MaxLength(20)]
        public string menuColor { get; set; }
        [MaxLength(20)]
        public string subMenuColor { get; set; }
        [MaxLength(20)]
        public string textColor { get; set; }
        [MaxLength(20)]
        public string blockColor { get; set; }
        [MaxLength(20)]
        public string color { get; set; }
        public int order { get; set; }
        public byte isdefault { get; set; }
    }
}
