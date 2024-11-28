using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_Observations_Micro_Flowcyto))]
    public class tnx_Observations_Micro_Flowcyto : Audit
    {
        public int id { get; set; }
        public int testId { get; set; }
        public int? labTestId { get; set; }
        public int? transactionId { get; set; }
        public string? observationName { get; set; }
        public string? result { get; set; }
        public short machineID { get; set; }
        public string? flag { get; set; }
        public byte? isBold { get; set; }
        public byte? reportType { get; set; }
        public short organismId { get; set; }
        public string? organismName { get; set; }
        public short antibiticId { get; set; }
        public string? antibitiName { get; set; }
        public string? colonyCount { get; set; }
        public string? interpretation { get; set; }
        public string? mic { get; set; }
        public string? positivity { get; set; }
        public string? intensity { get; set; }
        public byte? reportStatus { get; set; }
        public int? approvedBy { get; set; }
        public string? approvedName { get; set; }
        public string? comments { get; set; }
    }
}
