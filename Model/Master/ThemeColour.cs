using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(ThemeColour))]
    public class ThemeColour
    {
        public int id { get; set; } 
        public string headerColor { get; set; }
        public string menuColor { get; set; }
        public string subMenuColor { get; set; }
        public string textColor { get; set; }
        public string blockColor { get; set; }
        public int isdefault { get; set; }
    }
}
