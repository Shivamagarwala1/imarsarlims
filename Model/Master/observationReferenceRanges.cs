using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(observationReferenceRanges))]
    public class observationReferenceRanges:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? observationId { get; set; }
        public string? gender { get; set; }
        public string? genderTextValue { get; set; }
        public int? fromAge { get; set; }
        public int? toAge { get; set; }
        public double? minValue { get; set; }
        public double? maxValue { get; set; }
        public string? method { get; set; }
        public string? unit { get; set; }
        public string? reportReading { get; set; }
        public string? defaultValue { get; set; }
        public byte? autoHold { get; set; }
        public double? minCritical { get; set; }
        public double? maxCritical { get; set; }
        public double? minAutoApprovalValue { get; set; }
        public double? maxAutoApprovalValue { get; set; }
        public int? centreId { get; set; }
        public int? machineID { get; set; }
        
    }
}
