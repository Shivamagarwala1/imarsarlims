using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_BookingItem))]
    public class tnx_BookingItem : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(20)]
        public string? workOrderId { get; set; } = "";
        [ForeignKey("tnx_Booking")]
        public int transactionId { get; set; }
        [Required, MaxLength(20)]
        public string testcode { get; set; } = "";
        [Required]
        public int itemId { get; set; }
        [Required]
        public int packageID { get; set; } = 0;
        [Required]
        public int deptId { get; set; }
        [MaxLength(12)]
        public string? barcodeNo { get; set; }
        [Required, MaxLength(30)]
        public string departmentName { get; set; } 
        [Required, MaxLength(100)]
        public string investigationName { get; set; } 
        [Required]
        public int isPackage { get; set; }
        [MaxLength(100)]
        public string packageName { get; set; } 
        [Required]
        public int itemType { get; set; }
        [Range(0.000000, 999999999999.000000)]
        public double mrp { get; set; }
        [Range(0.000000, 999999999999.000000)]
        public double rate { get; set; }
        [Range(0.000000, 999999999999.000000)]
        public double discount { get; set; }
        [Range(0.000000, 999999999999.000000)]
        public double netAmount { get; set; }
        [Range(0.000000, 999999999999.000000)]
        public double packMrp { get; set; }
        [Required, Range(0.000000, 999999999999.000000)]
        public double packItemRate { get; set; }
        [Required, Range(0.000000, 999999999999.000000)]
        public double packItemDiscount { get; set; }
        [Required, Range(0.000000, 999999999999.000000)]
        public double packItemNet { get; set; }
        [Required]
        public int reportType { get; set; }
        [Required]
        public int centreId { get; set; }
        [Required]
        public int sessionCentreid { get; set; }
        public byte isSra { get; set; } 
        public byte isMachineOrder { get; set; } 
        public byte isEmailsent { get; set; } 
        [Required]
        public int sampleTypeId { get; set; }
        [Required, MaxLength(50)]
        public string sampleTypeName { get; set; } = "";
        public DateTime? sampleCollectionDate { get; set; }
        [MaxLength(100)]
        public string? sampleCollectedby { get; set; } = "";
        public int? sampleCollectedID { get; set; } = 0;
        public DateTime? sampleReceiveDate { get; set; }
        [MaxLength(100)]
        public string? sampleReceivedBY { get; set; } = "";
        public DateTime? resultDate { get; set; }
        public int? resultDoneByID { get; set; }
        [MaxLength(100)]
        public string?  resutDoneBy { get; set; } = "";
        public byte? isResultDone { get; set; } = 0;
        public byte? isApproved { get; set; } = 0;
        public DateTime? approvedDate { get; set; }
        public int? approvedByID { get; set; } = 0;
        [MaxLength(100)]
        public string? approvedbyName { get; set; } = "";
        [MaxLength(100)]
        public string? notApprovedBy { get; set; } = "";
        public DateTime? notApprovedDate { get; set; }
        public byte? isReporting { get; set; } = 0;
        public byte? isCritical { get; set; }
        public DateTime deliveryDate { get; set; }
        public byte? isInvoiceCreated { get; set; } = 0;
        public int? invoiceNumber { get; set; } = 0;
        public byte isUrgent { get; set; } = 0;
        [Required, MaxLength(2)]
        public string? isSampleCollected { get; set; } = "N";
        [MaxLength(100)]
        public string? samplecollectionremarks { get; set; } = "";
        [MaxLength(100)]
        public string? departmentReceiveRemarks { get; set; } = "";
        public DateTime? departmentReceiveDate { get; set; }
        [MaxLength(100)]
        public string? departmentReceiveBy { get; set; } = "";
        public int? departmentReceiveByID { get; set; } = 0;
        public byte isRemoveItem { get; set; } = 0;
        public int? sampleRejectionBy { get; set; } = 0;
        [MaxLength(100)] public string? sampleRejectionByName { get; set; } = "";
        public DateTime? sampleRejectionOn { get; set; }
        public int? interpretationId { get; set; } = 0;
        public int? approvalDoctor { get; set; } = 0;
        public byte? isOuthouse { get; set; } = 0;
        public int? outhouseLab { get; set; } = 0;
        [MaxLength(100)] 
        public string? labName { get; set; } = "";
        public int? outhouseDoneBy { get; set; } = 0;
        public DateTime? outhouseDoneOn { get; set; }
        public int? sampleRecollectedby { get; set; } = 0;
        public DateTime? sampleRecollectedDate { get; set; }
        public byte? isrerun { get; set; }
        [MaxLength(100)] public string? invoiceNo { get; set; } = "";
        public DateTime? invoiceDate { get; set; }
        [MaxLength(100)] public string? invoiceCycle { get; set; } = "";
        public double? invoiceAmount { get; set; } = 0;
        public int? invoiceCreatedBy { get; set; } = 0;
        [MaxLength(100)] 
        public string? invoiceNoOld { get; set; } = "";
        [MaxLength(100)] 
        public string? remarks { get; set; } = "";
        public DateTime? showonReportdate { get; set; }


    }
}
