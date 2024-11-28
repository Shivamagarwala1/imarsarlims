using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(machine_result))]
    public class machine_result :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? observationId { get; set; }
        public int? testId { get; set; }
        public string? workOrderId { get; set; }
        public string? barcodeNo { get; set; }
        public int? labCentreId { get; set; }
        public string? investigationName { get; set; }
        public string? labObservationName { get; set; }
        public string? macReading1 { get; set; }
        public byte? macId1 { get; set; }
        public string? machineName1 { get; set; }
        public string? MacReading2 { get; set; }
        public byte? macId2 { get; set; }
        public string? machineName2 { get; set; }
        public string? MacReading3 { get; set; }
        public byte? macId3 { get; set; }
        public string? machineName3 { get; set; }
        public string? patientName { get; set; }
        public DateTime? dob { get; set; }
        public string? gender { get; set; }
        public string? machineComments { get; set; }
        public string? status { get; set; }
        
    }
}
