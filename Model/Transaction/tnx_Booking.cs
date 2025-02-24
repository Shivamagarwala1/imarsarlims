using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_Booking))]
    public class tnx_Booking :Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey(nameof(transactionId))]
        public int transactionId { get; set; }
        [MaxLength(20)]
        public string workOrderId { get; set; }
        public int billNo { get; set; }
        public DateTime bookingDate { get; set; }
        [Required, MaxLength(20)]
        public string clientCode { get; set; }
        [ForeignKey("tnx_BookingPatient")]
        public int patientId { get; set; }
        [Required]
        public int title_id { get; set; }
        [Required, MaxLength(100)]
        public string name { get; set; }
        [Required, MaxLength(6)]
        public string gender { get; set; }
        public DateTime dob { get; set; }
        [Required]
        public int ageYear { get; set; }
        [Required]
        public int ageMonth { get; set; }
        [Required]
        public int ageDay { get; set; }
        [Required]
        public int totalAge { get; set; }
        [MaxLength(10)]
        public string mobileNo { get; set; }
        [Required, Range(0.000000, 999999999999.000000)]
        public double mrp { get; set; }
        [Required, Range(0.000000, 999999999999.000000)]
        public double grossAmount { get; set; }
        [Required, Range(0.000000, 999999999999.000000)]
        public double discount { get; set; }
        [Required, Range(0.000000, 999999999999.000000)]
        public double netAmount { get; set; }
        [Required, Range(0.000000, 999999999999.000000)]
        public double paidAmount { get; set; }
        [Required]
        public int sessionCentreid { get; set; }
        [Required]
        public int centreId { get; set; }
        [Required]
        public int rateId { get; set; }
        [Required]
        public byte isCredit { get; set; }
        [Required, MaxLength(10)]
        public string paymentMode { get; set; }
        [ MaxLength(50)]
        public string source { get; set; }
        public int discountType { get; set; }
        public int? discountid { get; set; } = 0;
        public string? discountReason { get; set; } = "";
        public int discountApproved { get; set; } 
        public int isDisCountApproved { get; set; }
        [MaxLength(100)]
        public string? patientRemarks { get; set; } = "";
        [MaxLength(100)]
        public string? labRemarks { get; set; } = "";
        [MaxLength(100)]
        public string? otherLabRefer { get; set; } = "";
        public int? OtherLabReferID { get; set; } = 0;
        [Required, MaxLength(100)]
        public string? RefDoctor1 { get; set; } = "";
        public int? refID1 { get; set; } = 0;
        [MaxLength(100)]
        public string? refDoctor2 { get; set; } = "";
        public int? refID2 { get; set; } = 0;
        public int? tempDOCID { get; set; } = 0;
        [MaxLength(100)]
        public string? tempDoctroName { get; set; } = "";
        public string? uploadDocument { get; set; } = "";
        [MaxLength(30)]
        public string? invoiceNo { get; set; } = "";
        public int? salesExecutiveID { get; set; }

        [ForeignKey(nameof(transactionId))]
        public List<tnx_BookingStatus>? addBookingStatus { get; set; }
        [ForeignKey(nameof(transactionId))]
        public List<tnx_BookingItem>? addBookingItem { get; set; }
        [ForeignKey(nameof(transactionId))]
        public List<tnx_ReceiptDetails>? addpaymentdetail { get; set; }
    }
}
