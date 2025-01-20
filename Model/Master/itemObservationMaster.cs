using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(itemObservationMaster))]
    public class itemObservationMaster :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required, MaxLength(100)]
        public string? labObservationName { get; set; }
        public byte? dlcCheck { get; set; }
        [MaxLength(15)]
        public string? gender { get; set; }
        public byte? printSeparate { get; set; }
        [MaxLength(50)]
        public string? shortName { get; set; }
        public double? roundUp { get; set; }
        [MaxLength(50)]
        public string? method { get; set; }
        [MaxLength(2)]
        public string? suffix { get; set; }
        [MaxLength(100)]
        public string? formula { get; set; }
        [MaxLength(1000)]
        public string? observationWiseInterpretation { get; set; }
        public byte? resultRequired { get; set; }
        public byte? collectionRequire { get; set; }
        public int? displaySequence { get; set; }
    }
}
