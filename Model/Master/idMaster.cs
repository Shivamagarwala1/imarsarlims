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
        public string? fYear { get; set; }
        public int? typeId { get; set; }
        public string? type { get; set; }
    }
}
