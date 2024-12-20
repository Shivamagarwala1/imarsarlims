using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(menuIconMaster))]
    public class menuIconMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string icon { get; set; }
        public string reactLibrery { get; set; } 

    }
}
