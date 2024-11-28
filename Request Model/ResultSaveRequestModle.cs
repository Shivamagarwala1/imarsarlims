using System.ComponentModel.DataAnnotations;

namespace iMARSARLIMS.Request_Model
{
    public class ResultSaveRequestModle
    {
        public int transactionId { get; set; }
        public int patientId { get; set; }
        public string? barcodeNo { get; set; }
        public int? testId { get; set; }
        public int? labObservationId { get; set; }
        public string? observationName { get; set; }
        public string? value { get; set; }
        public string? flag { get; set; }
        public double? minVal { get; set; }
        public double? maxVal { get; set; }
        public double? minCritical { get; set; }
        public double? maxCritical { get; set; }
        public byte? isCritical { get; set; }
        public string? readingFormat { get; set; }
        public string? unit { get; set; }
        public string? displayReading { get; set; }
        public string? machineReading { get; set; }
        public byte? machineID { get; set; }
        public byte? printSeperate { get; set; }
        public byte? isBold { get; set; }
        public string? machineName { get; set; }
        public byte? showInReport { get; set; }
        public string method { get; set; }
        public byte isResultDone { get; set; }
        public byte isApproved { get; set; }
        public string createdBy { get; set; }
        public int createdById  { get; set; }
        public DateTime createdDate { get; set; }

    }
}
