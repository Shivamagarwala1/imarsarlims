using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_Observations_Log))]
    public class tnx_Observations_Log : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? testId { get; set; }
        public int? labObservationId { get; set; }
        public string? observationName { get; set; }
        public string? value { get; set; }
        public string? flag { get; set; }
        public double? min { get; set; }
        public double? max { get; set; }
        public double? minCritical { get; set; }
        public double? maxCritical { get; set; }
        public byte? isCritical { get; set; }
        public string? readingformat { get; set; }
        public string? unit { get; set; }
        public string? testMethod { get; set; }
        public string? displayReading { get; set; }
        public string? machineReading { get; set; }
        public byte? machineID { get; set; }
        public byte? printseperate { get; set; }
        public byte? isBold { get; set; }
        public string? machineName { get; set; }
        public DateTime dtEntry { get; set; }

    }
}
