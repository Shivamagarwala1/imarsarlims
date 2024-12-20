using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(menuIconMaster))]
    public class menuIconMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(30)]

        public string icon { get; set; }
        [MaxLength(30)]
        public string reactLibrery { get; set; } 

    }
}
