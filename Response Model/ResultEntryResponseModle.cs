using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Response_Model
{
    public class ResultEntryResponseModle
    {
        public string? workOrderId { get; set; } = "";
        public int transactionId { get; set; }
        public int patientId { get; set; }
        [Required, MaxLength(10)]
        public string title { get; set; }
        [Required, MaxLength(100)]
        public string Pname { get; set; }
        [Required, MaxLength(6)]
        public string gender { get; set; }
        public string investigationName { get; set; } = "";
        public int? testId { get; set; }
        public int? labObservationId { get; set; }
        public string? observationName { get; set; }
        public string? value { get; set; }
        public string? flag { get; set; }
        public string? minVal { get; set; }
        public string? maxVal { get; set; }
        public string? minCritical { get; set; }
        public string? maxCritical { get; set; }
        public  int? isCritical { get; set; }
        public string? readingFormat { get; set; }
        public string? unit { get; set; }
        public string? displayReading { get; set; }
        public string? machineReading { get; set; }
        public int? machineID { get; set; }
        public int? printSeperate { get; set; }
        public int? isBold { get; set; }
        public string? machineName { get; set; }
        public int? showInReport { get; set; }
        public string method { get; set; }
        public string oldreading { get; set; }
    }
}
