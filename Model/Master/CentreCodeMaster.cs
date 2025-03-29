using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    public class CentreCodeMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(5)]
        public string Prefix { get; set; }
        [MaxLength(10)]
        public string type { get; set; }
        public int maxId { get; set; }

    }
}
