using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(idMaster))]
    public class idMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? centreId { get; set; }
        public int? maxID { get; set; }
        [MaxLength(4)]
        public string? fYear { get; set; }
        public int? typeId { get; set; }
        [MaxLength(10)]
        public string? type { get; set; }
    }
}
