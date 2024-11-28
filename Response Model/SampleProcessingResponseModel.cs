using System.ComponentModel.DataAnnotations;

namespace iMARSARLIMS.Response_Model
{
    public class SampleProcessingResponseModel
    {
        public int patientId { get; set; }
        public int transactionId { get; set; }
        public string? workOrderId { get; set; }
        public string? name { get; set; }
        public int itemId { get; set; }
        public string? investigationName { get; set; }
        public int sampleTypeId { get; set; }
        public string? sampleTypeName { get; set; }
        public string? isSampleCollected { get; set; }
        public DateTime? createdDateTime { get; set; }
        public int deptId { get; set; }
        public string? departmentName { get; set; }
        [Required, MaxLength(12)]
        public string barcodeNo { get; set; }
        public int centreId { get; set; }
        [MaxLength(100)]
        public string? rejectionReason { get; set; }


    }
}
