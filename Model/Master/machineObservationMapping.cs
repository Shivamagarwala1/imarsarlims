using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(machineObservationMapping))]
    public class machineObservationMapping :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public short machineId { get; set; }
        [Required, MaxLength(30)]
        public string assay { get; set; }
        public int? labTestID { get; set; }
        public byte? isOrderable { get; set; }
        [ MaxLength(200)]
        public string? formula { get; set; }
        public byte? roundUp { get; set; }
        public double? multiplication { get; set; }
        [MaxLength(2)]
        public string? suffix { get; set; }
    }
}
