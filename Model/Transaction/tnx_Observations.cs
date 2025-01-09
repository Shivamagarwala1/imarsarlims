using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_Observations))]
    public class tnx_Observations : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? testId { get; set; }
        public int? labObservationId { get; set; }
        [MaxLength(50)]
        public string? observationName { get; set; }
        [MaxLength(20)]
        public string? value { get; set; }
        [MaxLength(2)]
        public string? flag { get; set; }
        public double? min { get; set; }
        public double? max { get; set; }
        public double? minCritical { get; set; }
        public double? maxCritical { get; set; }
        public byte? isCritical { get; set; }
        [MaxLength(50)]
        public string? readingformat { get; set; }
        [MaxLength(50)]
        public string? unit { get; set; }
        [MaxLength(50)]
        public string? testMethod { get; set; }
        [MaxLength(500)]
        public string? displayReading { get; set; }
        [MaxLength(10)]
        public string? machineReading { get; set; }
        public byte? machineID { get; set; }
        public byte? printseperate { get; set; }
        public byte? isBold { get; set; }
        [MaxLength(50)]
        public string? machineName { get; set; }
        public byte? showInReport { get; set; }
        public byte isHeader { get; set; }
    }
}
