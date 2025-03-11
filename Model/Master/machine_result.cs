using System.ComponentModel.DataAnnotations;
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
        [MaxLength(20)]
        public string? workOrderId { get; set; }
        [MaxLength(15)]
        public string? barcodeNo { get; set; }
        public int? labCentreId { get; set; }
        [MaxLength(100)]
        public string? investigationName { get; set; }
        [MaxLength(100)]
        public string? labObservationName { get; set; }
        [MaxLength(30)]
        public string? macReading1 { get; set; }
        public byte? macId1 { get; set; }
        [MaxLength(30)]
        public string? machineName1 { get; set; }
        [MaxLength(30)]
        public string? MacReading2 { get; set; }
        public byte? macId2 { get; set; }
        [MaxLength(30)]
        public string? machineName2 { get; set; }
        [MaxLength(30)]
        public string? MacReading3 { get; set; }
        public byte? macId3 { get; set; }
        [MaxLength(30)]
        public string? machineName3 { get; set; }
        [MaxLength(100)]
        public string? patientName { get; set; }
        public DateTime? dob { get; set; }
        [MaxLength(10)]
        public string? gender { get; set; }
        [MaxLength(100)]
        public string? machineComments { get; set; }
        [MaxLength(10)]
        public string? status { get; set; }
        
    }
}
